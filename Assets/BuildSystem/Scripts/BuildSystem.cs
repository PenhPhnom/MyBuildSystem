using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildSystem : MonoBehaviour
{
    /// <summary>
    /// 本地自增键值
    /// </summary>
    public int LoopID = 3;
    /// <summary>
    /// 可用的物体
    /// </summary>
    public List<GameObject> Buildings;
    /// <summary>

    /// <summary>
    /// 当前移动的物品
    /// </summary>
    private GameObject m_flyCube;
    private Renderer m_flyCubeRender;
    private Building m_flyCubeBuilding;
    /// <summary>
    /// 标志物
    /// </summary>
    public GameObject socket;
    /// <summary>
    /// 选定的移动层
    /// </summary>
    public int maskLayer = 1;

    /// <summary>
    /// 列表
    /// </summary>
    public GameObject content;
    /// <summary>
    /// 偏移值
    /// </summary>
    internal float offset = 0;
    /// <summary>
    /// 放置的节点
    /// </summary>
    private Transform m_buildRoot;

    private float m_distance = 0;
    public float RotateAngle = 90;

    public Vector3 lastPos;
    internal int pt;
    public float trunkSize = 0.1f;

    //颜色0是允许 1是不允许
    public List<Color> colorList = new List<Color>();
    //默认颜色
    public bool colorNormal = false;
    public bool isReplace = false;
    public bool isObserveState = false;
    private GameObject m_replaces;

    public Material meshRender = null;
    public Renderer rend;
    public float suspendOffset = 0.1f;
    public Text t;
    public bool isHold = false;
    public ThirdCamera mainCameraCom;
    string buildStr = "";
    string thingStr = "";
    public void Start()
    {
        m_buildRoot = transform.Find("buildings");
        ThingsManager.Instance.initCameraState();
        //通过配置初始化按钮信息
        mainCameraCom = Camera.main.GetComponent<ThirdCamera>();
        //绑定参数方法
        for (int i = 0; i < content.transform.childCount; i++)
        {
            int index = i;
            content.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate ()
            {
                bringThings(index);
            });
        }
        if (colorNormal)
        {
            colorList.Clear();
            colorList.Add(colorList[0]);
            colorList.Add(colorList[1]);
        }
        mainCameraCom.Center = socket.transform;
        //test 写死json
        addTestJsonthings();
        addTestJsonBuild();
    }

    private void addTestJsonBuild()
    {
        BuildScenes buildScenes = new BuildScenes();
        buildScenes.buildScene.Clear();
        buildScenes.buildScene.Add(new BuildScene
        {
            buildSceneName = "room1",
            buildThings = {}
        });
        var writeMode = JsonConvert.SerializeObject(buildScenes, Formatting.Indented);
        buildStr = writeMode;
    }

    private void addTestJsonthings()
    {
        Things things = new Things();
        things.thingsArr.Clear();
        things.thingsArr.Add(new BuildSceneItem
        {
            thingName = "Cube",
            thingParentName = "",
            thingType = 0,
            thingID = 0,
            postionX = 0,
            postionY = 0,
            postionZ = 0,
            rotationX = 0,
            rotationY = 0,
            rotationZ = 0,
            IsUseGravity = false,
            FreezePostion = true,
            FreezeRotation = true,
            IsCPush = true,
            IsPushed = true,
            LimitAround = true,
            IsTerrin = false
        });
        things.thingsArr.Add(new BuildSceneItem
        {
            thingName = "Capsule",
            thingParentName = "",
            thingType = 1,
            thingID = 0,
            postionX = 0,
            postionY = 0,
            postionZ = 0,
            rotationX = 0,
            rotationY = 0,
            rotationZ = 0,
            IsUseGravity = false,
            FreezePostion = true,
            FreezeRotation = true,
            IsCPush = true,
            IsPushed = true,
            LimitAround = true,
            IsTerrin = false
        }); things.thingsArr.Add(new BuildSceneItem
        {
            thingName = "Cylinder",
            thingParentName = "",
            thingType = 2,
            thingID = 0,
            postionX = 0,
            postionY = 0,
            postionZ = 0,
            rotationX = 0,
            rotationY = 0,
            rotationZ = 0,
            IsUseGravity = false,
            FreezePostion = true,
            FreezeRotation = true,
            IsCPush = true,
            IsPushed = true,
            LimitAround = true,
            IsTerrin = false
        }); things.thingsArr.Add(new BuildSceneItem
        {
            thingName = "Sphere",
            thingParentName = "",
            thingType = 3,
            thingID = 0,
            postionX = 0,
            postionY = 0,
            postionZ = 0,
            rotationX = 0,
            rotationY = 0,
            rotationZ = 0,
            IsUseGravity = false,
            FreezePostion = true,
            FreezeRotation = true,
            IsCPush = true,
            IsPushed = true,
            LimitAround = true,
            IsTerrin = false
        }); things.thingsArr.Add(new BuildSceneItem
        {
            thingName = "Plane",
            thingParentName = "",
            thingType = 4,
            thingID = 0,
            postionX = 0,
            postionY = 0,
            postionZ = 0,
            rotationX = 0,
            rotationY = 0,
            rotationZ = 0,
            IsUseGravity = false,
            FreezePostion = true,
            FreezeRotation = true,
            IsCPush = true,
            IsPushed = true,
            LimitAround = true,
            IsTerrin = false
        }); things.thingsArr.Add(new BuildSceneItem
        {
            thingName = "Cube1",
            thingParentName = "",
            thingType = 5,
            thingID = 0,
            postionX = 0,
            postionY = 0,
            postionZ = 0,
            rotationX = 0,
            rotationY = 0,
            rotationZ = 0,
            IsUseGravity = false,
            FreezePostion = true,
            FreezeRotation = true,
            IsCPush = true,
            IsPushed = true,
            LimitAround = true,
            IsTerrin = false
        }); 
        var writeMode = JsonConvert.SerializeObject(things, Formatting.Indented);
        thingStr = writeMode;
    }

    /// <summary>
    /// 观察者模式
    /// </summary>
    public void btnObserveState()
    {
        isObserveState = !isObserveState;
        if (isObserveState)
        {
            t.text = "观察者模式打开";
            mainCameraCom.newPos = transform.position;
            mainCameraCom.distanceY = mainCameraCom.newPos.y;
            mainCameraCom.distanceZ = mainCameraCom.newPos.z;
            mainCameraCom.transform.LookAt(mainCameraCom.Target);
        }
        else
        {
            t.text = "观察者模式关闭";

            ThingsManager.Instance.SetCameraState();

            mainCameraCom.Target = null;
        }
    }

    public void btnUnLoadJSON()
    {
        t.text = "卸载成功";
        var loadModle = JsonConvert.DeserializeObject<BuildScenes>(buildStr);
        loadModle.buildScene[0].buildThings.Clear();
        var writeMode = JsonConvert.SerializeObject(loadModle, Formatting.Indented);
        ThingsManager.Instance.SetStr(ref buildStr, writeMode);
        for (int i = 0; i < m_buildRoot.childCount; i++)
        {
            Destroy(m_buildRoot.GetChild(i).gameObject);
        }
    }
    //public void updateLoop()
    //{
    //    if (mainCameraCom.target)
    //    {
    //        //为了调式时看的清楚画的线
    //        Debug.DrawLine(mainCameraCom.target.position, Camera.main.transform.position, Color.red);
    //        RaycastHit hit;

    //        if (Physics.Linecast(mainCameraCom.target.position, Camera.main.transform.position, out hit))
    //        {
    //            last_obj = hit.collider.gameObject;
    //            //让遮挡物变半透明
    //            Color obj_color = last_obj.GetComponent<Renderer>().material.color;
    //            obj_color.a = 0.5f;
    //            last_obj.GetComponent<Renderer>().material.SetColor("_Color", obj_color);
    //        }//还原
    //        else if (last_obj != null)
    //        {
    //            Color obj_color = last_obj.GetComponent<Renderer>().material.color;
    //            obj_color.a = 1.0f;
    //            last_obj.GetComponent<Renderer>().material.SetColor("_Color", obj_color);
    //            last_obj = null;
    //        }
    //    }
    //}
    public void btnLoadJSON()
    {
        var loadModle = JsonConvert.DeserializeObject<BuildScenes>(buildStr);
        List<BuildSceneItem> things = loadModle.buildScene[0].buildThings;
        if (m_buildRoot.childCount == 0)
        {
            LoopID = LoopID + things.Count;
            for (int i = 0; i < things.Count; i++)
            {
                LoopID++;
                GameObject thing = Instantiate(Buildings[things[i].thingType], m_buildRoot);
                thing.transform.position = new Vector3(things[i].postionX, things[i].postionY, things[i].postionZ);
                thing.transform.rotation = Quaternion.Euler(things[i].rotationX, things[i].rotationY, things[i].rotationZ);
                thing.name = things[i].thingName;
                AddMissCom(thing);
                Building b = thing.GetComponent<Building>();
                b.IsCPush = things[i].IsCPush;
                b.limitAround = things[i].LimitAround;
                b.isPushed = things[i].IsPushed;
                b.thingID = LoopID;
                Rigidbody r = thing.GetComponent<Rigidbody>();
                r.useGravity = things[i].IsUseGravity;
                r.constraints =
                    RigidbodyConstraints.FreezePositionZ |
                    RigidbodyConstraints.FreezeRotationZ |
                    RigidbodyConstraints.FreezePositionY |
                    RigidbodyConstraints.FreezeRotationY |
                    RigidbodyConstraints.FreezePositionX |
                    RigidbodyConstraints.FreezeRotationX;
                //设置颜色
                b.normalColor = thing.GetComponent<Renderer>().material.color;
                b.thingType = things[i].thingType;
                thing.layer = 0;
            }
        }
    }
    /// <summary>
    /// 这块是测试数据
    /// </summary>
    /// <param name="id">物品的id</param>
    public void setm_flyCubeTest(int id)
    {
        //判断是否越界
        if (Buildings.Count > id)
        {
            //判断物品是否为空
            if (Buildings[id])
            {
                m_flyCube = Instantiate(Buildings[id], m_buildRoot);
                switch (id)
                {
                    case 0:
                        m_flyCubeRender.material.color = Color.cyan;
                        m_flyCubeBuilding.normalColor = Color.cyan;
                        break;
                    case 1:
                        m_flyCubeRender.material.color = colorList[0];
                        m_flyCubeBuilding.normalColor = colorList[0];
                        break;
                    case 2:
                        m_flyCubeRender.material.color = Color.blue;
                        m_flyCubeBuilding.normalColor = Color.blue;
                        break;
                    case 3:
                        m_flyCubeRender.material.color = Color.gray;
                        m_flyCubeBuilding.normalColor = Color.gray;
                        break;
                    case 4:
                        m_flyCubeRender.material.color = Color.black;
                        m_flyCubeBuilding.normalColor = Color.black;
                        break;
                }
            }
            //设置颜色
            m_flyCubeRender.material.color = colorList[0];
        }
    }
    public void relaceThing()
    {
        //替换状态
        isReplace = true;
        t.text = "替换模式打开";
    }
    public void relaceMaterial()
    {
        if (meshRender == null)
        {
            return;
        }
        rend = m_replaces.GetComponent<Renderer>();
        //rend.enabled = true;
        rend.sharedMaterial = meshRender;//代表这个对象的共享材质资源（这个是替换材质球）
        m_replaces = null;
        isReplace = false;
        t.text = "替换模式关闭";
    }
    public void setm_flyCube(int id)
    {
        if (!isReplace)
        {
            //判断是否越界
            if (Buildings.Count > id)
            {
                //判断物品是否为空
                if (Buildings[id])
                {
                    t.text = "选择物品=" + Buildings[id].name;
                    m_flyCube = Instantiate(Buildings[id], m_buildRoot);
                    m_flyCube.transform.position = Vector3.zero;

                }
                AddMissCom(m_flyCube);
                Building b = m_flyCubeBuilding;
                b.IsCPush = true;
                b.limitAround = true;

                Rigidbody r = m_flyCube.GetComponent<Rigidbody>();
                r.useGravity = false;
                r.constraints =
                    RigidbodyConstraints.FreezePositionZ |
                    RigidbodyConstraints.FreezeRotationZ |
                    RigidbodyConstraints.FreezePositionY |
                    RigidbodyConstraints.FreezeRotationY |
                    RigidbodyConstraints.FreezePositionX |
                    RigidbodyConstraints.FreezeRotationX;
                //设置颜色
                m_flyCubeBuilding.normalColor = m_flyCubeRender.material.color;
                m_flyCubeBuilding.thingType = id;
                m_flyCubeRender.material.color = colorList[0];
            }
        }
        else
        {
            GameObject m_new = Instantiate(Buildings[id], m_buildRoot);
            m_new.transform.position = m_replaces.transform.position;
            m_new.transform.rotation = m_replaces.transform.rotation;
            m_new.GetComponent<Building>().SetIsPushed(true);
            m_new.layer = 0;
            m_new.GetComponent<Renderer>().material.color = m_replaces.GetComponent<Building>().normalColor;
            DestroyImmediate(m_replaces);
            isReplace = false;
            t.text = "替换模式关闭";
        }
    }
    public void AddMissCom(GameObject m_flyCube)
    {
        if (m_flyCube.GetComponent<Building>() == null)
        {
            m_flyCube.AddComponent<Building>();
        }
        if (m_flyCube.GetComponent<BoxCollider>() == null)
        {
            m_flyCube.AddComponent<BoxCollider>();
        }
        if (m_flyCube.GetComponent<Rigidbody>() == null)
        {
            m_flyCube.AddComponent<Rigidbody>();
        }
        m_flyCubeRender = m_flyCube.GetComponent<Renderer>();
        m_flyCubeBuilding = m_flyCube.GetComponent<Building>();
    }
    private void bringThings(int id)
    {
        setm_flyCube(id);
        //setm_flyCubeTest(id);
        isHold = true;
    }

    void Update()
    {
        // 以摄像机所在位置为起点，创建一条发射的射线  
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (isObserveState)
        {
            //选择相机跟随的物品
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, maskLayer))
                {
                    var hitThings = hit.collider;
                    //判断不是地板
                    if (!hitThings.GetComponent<Building>().isTerrin)
                    {
                        mainCameraCom.Target = hitThings.transform;
                    }
                }
            }
        }
        else
        {
            //判断物品是否持有，如果不持有，不往下执行操作
            if (!m_flyCube)
            {
                //放置物品
                if (Input.GetMouseButtonDown(0))
                {
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, maskLayer))
                    {
                        if (!hit.collider.GetComponent<Building>().isTerrin)
                        {
                            if (!isReplace)
                            {
                                m_flyCube = hit.collider.gameObject;
                                m_flyCube.layer = 1;
                                m_flyCubeRender = m_flyCube.GetComponent<Renderer>();
                                m_flyCubeBuilding = m_flyCube.GetComponent<Building>();
                                m_flyCubeBuilding.SetIsPushed(false);
                                //设置颜色
                                m_flyCubeRender.material.color = colorList[0];
                                //去掉编号
                                m_flyCube.name = m_flyCube.name.Split("_")[0];
                                var thing = new BuildSceneItem();
                                isHold = true;
                                //设置物件属性
                                setThingAttributes(thing);
                                ThingsManager.Instance.DelBuildThingsStr(ref buildStr, 0, m_flyCubeBuilding.thingID);
                            }
                            else
                            {
                                m_replaces = hit.collider.gameObject;
                                m_replaces.GetComponent<Building>().SetIsPushed(true);
                                m_replaces.layer = 0;
                            }
                        }
                    }
                }
                return;
            }
            //放置物品
            if (Input.GetMouseButtonDown(0))
            {
                //判断物体是否可以放置
                if (m_flyCubeRender.material.color == colorList[1])
                {
                    Debug.Log("不可以放置");
                }
                else
                {
                    m_flyCubeRender.material.color = m_flyCubeBuilding.normalColor;
                    m_flyCubeBuilding.SetIsPushed(true);
                    LoopID++;
                    m_flyCubeBuilding.thingID = LoopID;
                    m_flyCube.name = m_flyCube.name.Replace("(Clone)", "") + "_" + LoopID;
                    var thing = new BuildSceneItem();
                    //设置物件属性
                    setThingAttributes(thing);
                    ThingsManager.Instance.AddBuildThingsStr(ref buildStr, 0, thing);
                    m_flyCube.layer = 0;
                    //恢复正常的位置
                    offsetBuildingReset(pt);
                    m_flyCube = null;
                    isHold = false;
                }
                return;
            }
            //取消物品
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(m_flyCube);
                m_flyCube = null;
                isHold = false;
                return;
            }
            //这个是在端上测试的点击中键进行旋转
            if (Input.GetMouseButtonDown(2))
            {
                m_distance += RotateAngle;
                m_flyCube.transform.rotation = Quaternion.AngleAxis(m_distance, Vector3.up);
            }

            //选定某一个层，进行移动
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, maskLayer))
            {
                m_flyCubeBuilding.SetNowParent(hit.collider.GetComponent<Building>());
                // 判断socket与物品的方向
                pt = posType(hit.collider.transform);
                //判断是否可以堆叠摆放

                //如果物体穿墙，则不进行偏移

                //根据关系进行偏移
                offsetBuilding(pt, hit);
            }
            /*
    * 判断哪个轴是贴着墙走的
    * 原理：
    * 贴着墙走的那根轴不会动，
    * 其他的会一直变动，
    * 如果三个轴都没用变动证明这个物体处于静止状态，
    * 如果其中两条轴没用动证明物体处于变速直线运动
    * 只要一条轴不动，证明这条轴是贴着墙面或者地面走的
    */
            //如果两个坐标相等，证明这个物体静止状态
            if (lastPos != m_flyCube.transform.position)
            {
                //几条轴相等
                int sameVNum = 0;
                int VType = 0;
                if (lastPos.x == m_flyCube.transform.position.x)
                {
                    sameVNum++;
                    VType = 1;
                }
                if (lastPos.y == m_flyCube.transform.position.y)
                {
                    sameVNum++;
                    VType = 2;
                }
                if (lastPos.z == m_flyCube.transform.position.z)
                {
                    sameVNum++;
                    VType = 3;
                }
                lastPos = m_flyCube.transform.position;
                if (sameVNum == 1)
                {
                    switch (VType)
                    {
                        case 1:
                            offset = m_flyCubeRender.bounds.size.x / 2;
                            break;
                        case 2:
                            offset = m_flyCubeRender.bounds.size.y / 2;
                            break;
                        case 3:
                            offset = m_flyCubeRender.bounds.size.z / 2;
                            break;
                    }
                }
            }
            else
            {

            }
        }
        //updateLoop();
    }
    /// <summary>
    /// 设置物品属性
    /// </summary>
    /// <param name="thing">物品</param>
    private void setThingAttributes(BuildSceneItem thing)
    {
        thing.thingID = m_flyCubeBuilding.thingID;
        thing.thingName = m_flyCube.name;
        thing.thingType = m_flyCubeBuilding.thingType;
        if (m_flyCubeBuilding.parentObj)
        {
            thing.thingParentName = m_flyCubeBuilding.parentObj.name;
        }
        thing.IsUseGravity = m_flyCubeBuilding.IsGravity;
        thing.IsCPush = m_flyCubeBuilding.IsCPush;
        thing.IsPushed = m_flyCubeBuilding.isPushed;
        thing.IsTerrin = m_flyCubeBuilding.isTerrin;
        thing.LimitAround = m_flyCubeBuilding.limitAround;
        thing.postionX = m_flyCube.transform.position.x;
        thing.postionY = m_flyCube.transform.position.y;
        thing.postionZ = m_flyCube.transform.position.z;
        thing.rotationX = m_flyCube.transform.rotation.eulerAngles.x;
        thing.rotationY = m_flyCube.transform.rotation.eulerAngles.y;
        thing.rotationZ = m_flyCube.transform.rotation.eulerAngles.z;
        //暂时不管这东西，先设置为xyz全部为true
        thing.FreezePostion = true;
        thing.FreezeRotation = true;
    }

    private void offsetBuildingReset(int pt)
    {
        switch (pt)
        {
            //上
            case 1:
                m_flyCube.transform.position = new Vector3(m_flyCube.transform.position.x, m_flyCube.transform.position.y - suspendOffset, m_flyCube.transform.position.z);
                break;
            //左
            case 2:
                m_flyCube.transform.position = new Vector3(m_flyCube.transform.position.x - suspendOffset, m_flyCube.transform.position.y, m_flyCube.transform.position.z);
                break;
            //右
            case 3:
                m_flyCube.transform.position = new Vector3(m_flyCube.transform.position.x - suspendOffset, m_flyCube.transform.position.y, m_flyCube.transform.position.z);
                break;
            //前
            case 4:
                m_flyCube.transform.position = new Vector3(m_flyCube.transform.position.x, m_flyCube.transform.position.y, m_flyCube.transform.position.z - suspendOffset);
                break;
        }
    }

    ///// <summary>
    ///// 判断三维哪个加减
    ///// </summary>
    ///// <returns></returns>
    private int posType(Transform nearThing)
    {

        //地面
        if (nearThing.name.IndexOf("Floor") != -1)
        {
            //在地板
            //先不考虑天花板的情况
            return 1;
        }
        //墙面
        else if (nearThing.name.IndexOf("Wall") != -1)
        {
            float f = JudgeTransfrom.GetIsRightValue(socket.transform, nearThing.position);
            if (f > 0)
            {//右
                return 3;
            }
            else if (f < 0)
            {//左
                return 2;
            }
            else//前
            {
                return 4;
            }
        }
        //现在除了墙面就是地面，其他情况先不考虑，所以返回值为1
        else
        {
            return 1;
        }
    }

    public void offsetBuilding(int pt, RaycastHit hit)
    {
        switch (pt)
        {
            //上
            case 1:
                m_flyCube.transform.position = new Vector3(hit.point.x, hit.point.y + (offset + suspendOffset), hit.point.z);
                break;
            //左
            case 2:
                m_flyCube.transform.position = new Vector3(hit.point.x + (offset + suspendOffset), hit.point.y, hit.point.z);
                break;
            //右
            case 3:
                m_flyCube.transform.position = new Vector3(hit.point.x - (offset + suspendOffset), hit.point.y, hit.point.z);
                break;
            //前
            case 4:
                m_flyCube.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z - (offset + suspendOffset));
                break;
        }
    }
    private void OnDestroy()
    {

    }
}
