using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int thingID;
    public int thingType;
    public bool isCPush = false;

    public Color normalColor;
    /// <summary>
    /// 父节点
    /// </summary>
    public GameObject parentObj;

    public bool isPushed = false;
    /// <summary>
    /// 碰撞的自己和对方
    /// </summary>
    private GameObject other = null;
    private List<GameObject> otherList = new List<GameObject>();
    private GameObject self = null;

    private BuildSystem bSystem;
    private void Start()
    {
        bSystem = GameObject.Find("BuildSystem").GetComponent<BuildSystem>();
    }
    /// <summary>
    /// 是否限制周围不能摆放
    /// </summary>
    public bool limitAround = false;
    /// <summary>
    /// 是否是地图
    /// </summary>
    public bool isTerrin = false;

    public bool IsGravity = false;

    internal void setIsCanPush(bool push)
    {
        isCPush = push;
    }
    internal bool getIsCanPush()
    {
        return isCPush;
    }
    internal void setNowParent(GameObject obj)
    {
        parentObj = obj;
    }
    internal GameObject getNowParent()
    {
        return parentObj;
    }

    internal void setIsPushed(bool Pushed)
    {
        isPushed = Pushed;
    }
    internal bool getIsPushed()
    {
        return isPushed;
    }
    internal bool getlimitAround()
    {
        return limitAround;
    }
    private void OnCollisionStay(Collision collision)
    {
        //作用于物体和物体之间
        if (collision.collider.gameObject.layer == 0)
        {
            //正持有的物体
            self = transform.gameObject;
            //放置的物品或者地板墙面
            other = collision.collider.gameObject;
            if (other && self && !self.GetComponent<Building>().getIsPushed())
            {
                //如果你物体旋转,这个y会自动切换
                var offset = other.GetComponent<Renderer>().bounds.size.y / 2;
                bool ISF = JudgeTransfrom.IsUp(other.transform, self.transform.position - new Vector3(0, offset, 0));
                //添加碰撞过的物品
                if (!otherList.Contains(other))
                {
                    otherList.Add(other);
                }
                if (!self.GetComponent<Building>().getIsPushed())
                {
                    if (other.GetComponent<Building>().isTerrin)
                    {
                        if (ISF)
                        {
                            if (!other.GetComponent<Building>().getIsCanPush())
                            {
                                self.GetComponent<Renderer>().material.color = bSystem.colorList[1];
                            }
                            else
                            {
                                self.GetComponent<Renderer>().material.color = bSystem.colorList[0];
                            }
                        }
                        else
                        {
                            self.GetComponent<Renderer>().material.color = bSystem.colorList[1];
                        }
                    }
                    else
                    {
                        //判断是否接触两个物体。如果接触两个物体会出现穿模的情况，需要去掉这种接触两个物体穿模的情况
                        if (otherList.Count > 1)
                        {
                            self.GetComponent<Renderer>().material.color = bSystem.colorList[1];
                        }
                        else
                        {
                            self.GetComponent<Renderer>().material.color = bSystem.colorList[0];
                        }
                    }
                }
            }
        }
        else
        {

            if (collision.collider.GetComponent<Building>().getNowParent() != transform.gameObject)
            {
                collision.collider.GetComponent<Renderer>().material.color = bSystem.colorList[1];
            }
            else
            {
                if (parentObj)
                {
                    var offset = collision.collider.GetComponent<Renderer>().bounds.size.y;
                    float upValue = JudgeTransfrom.GetIsUpValue(parentObj.transform, collision.collider.transform.position - new Vector3(0, offset, 0));
                    //判断是否是地板或者墙面
                    if (isTerrin)
                    {
                        collision.collider.GetComponent<Renderer>().material.color = bSystem.colorList[0];
                    }
                    else
                    {
                        if (upValue > offset / 2)
                        {
                            collision.collider.GetComponent<Renderer>().material.color = bSystem.colorList[0];
                        }
                        else
                        {
                            //判断是否是放置的物品，并且还不是地形，手持物体变为红色，不可以放置
                            if (parentObj.GetComponent<Building>().getIsPushed() && !parentObj.GetComponent<Building>().isTerrin)
                            {
                                collision.collider.GetComponent<Renderer>().material.color = bSystem.colorList[1];
                            }
                            else
                            {
                                collision.collider.GetComponent<Renderer>().material.color = bSystem.colorList[0];
                            }
                        }
                        //可以允许周围摆放
                        if (limitAround)
                        {
                            collision.collider.GetComponent<Renderer>().material.color = bSystem.colorList[0];
                        }
                    }
                }
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.GetComponent<Building>())
        {
            if (!collision.collider.GetComponent<Building>().getIsPushed())
            {
                collision.collider.GetComponent<Renderer>().material.color = bSystem.colorList[0];
            }
        }
        self = null;
        other = null;
        otherList.Clear();
    }


    private void OnDestroy()
    {
        parentObj = null;
        otherList.Clear();
        bSystem = null;
        self = null;
        other = null;
    }
}
