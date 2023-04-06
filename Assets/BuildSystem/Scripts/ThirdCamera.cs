using UnityEngine;

public class ThirdCamera : MonoBehaviour
{
    public Transform Target = null;     // 围绕的目标
    public Transform Center = null;     // 相机围绕中心
   private Vector2 oldPosition1;
    private Vector2 oldPosition2;
    // 缩放系数  
    internal float distanceY = 0;
    internal float distanceZ = 0;
    public Vector3 newPos = new Vector3(0, 0, 0);
    public BuildSystem m_buildSystem = null;

    void Update()
    {
        if (Target)
        {
            //不是双指就关闭
            if (Input.touchCount > 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
                {
                    //获取第一、二次两次触摸的位置
                    Vector2 tempPosition1 = Input.GetTouch(0).position;
                    Vector2 tempPosition2 = Input.GetTouch(1).position;
                    var y = transform.position.y;
                    var z = transform.position.z;
                    //靠近
                    if (IsEnlarge(oldPosition1, oldPosition2, tempPosition1, tempPosition2))
                    {
                        newPos = new Vector3(0, y - 0.3f, z + 0.3f);
                        transform.position = newPos;
                        transform.LookAt(Target);
                    }
                    else//远离
                    {
                        newPos = new Vector3(0, y + 0.3f, z - 0.3f);
                        transform.position = newPos;
                        transform.LookAt(Target);
                    }
                    //备份上一次触摸点的位置，用于对比   
                    oldPosition1 = tempPosition1;
                    oldPosition2 = tempPosition2;
                }
            }
            else if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Camerarotate();
                }
            }
        }
        //在编辑的时候旋转
        //持有状态下，不起作用，放下物品的时候才可以旋转
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if(!m_buildSystem.isHold)
                {
                    CamerarotateH();
                }
            }
        }
    }

    /// <summary>
    /// 比较两次的位置，大小，来进行放大还是缩小
    /// </summary>
    /// <param name="oP1"></param>
    /// <param name="oP2"></param>
    /// <param name="nP1"></param>
    /// <param name="nP2"></param>
    /// <returns></returns>
    bool IsEnlarge(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2)
    {
        //函数传入上一次触摸两点的位置与本次触摸两点的位置计算出用户的手势   
        var leng1 = Mathf.Sqrt((oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y));
        var leng2 = Mathf.Sqrt((nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y));
        if (leng1 < leng2)
        {
            //放大手势   
            return true;
        }
        else
        {
            //缩小手势   
            return false;
        }
    }

    public float speed = 1;
    private void Camerarotate() //摄像机围绕目标旋转操作
    {
   
            transform.RotateAround(Target.position, Vector3.up, speed * Time.deltaTime); //摄像机围绕目标旋转
            var mouse_x = Input.GetAxis("Mouse X");//获取鼠标X轴移动
            var mouse_y = -Input.GetAxis("Mouse Y");//获取鼠标Y轴移动
            if(Mathf.Abs(mouse_x * 5) <20)
            {
                transform.RotateAround(Target.transform.position, Vector3.up, mouse_x * 5);
            }
            if (Mathf.Abs(mouse_y * 5) < 20)
            {
                transform.RotateAround(Target.transform.position, transform.right, mouse_y * 5);
            }
    }
    private void CamerarotateH() //摄像机围绕目标旋转操作
    {
        //转相机父节点
        //以后可能换成Cinemachine
        transform.RotateAround(Camera.main.transform.parent.position, Vector3.up, speed * Time.deltaTime); //摄像机围绕目标旋转
        var mouse_x = Input.GetAxis("Mouse X");//获取鼠标X轴移动
        if (Mathf.Abs(mouse_x * 5) < 20)
        {
            transform.RotateAround(Camera.main.transform.parent.position, Vector3.up, mouse_x * 5);
        }
    }
        //设置朝向物体
    public void SetForwordTarget(Transform tran)
    {
        Target = tran;
    }
    //设置朝向物体
    public Transform GetForwordTarget()
    {
        return Target;
    }
    //设置相机旋转中心
    public void SetForwordCenter(Transform tran)
    {
        Center = tran;
    }
}