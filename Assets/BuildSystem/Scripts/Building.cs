using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int thingID;
    public int thingType;
    public bool IsCPush = false;

    public Color normalColor;
    /// <summary>
    /// 父节点
    /// </summary>
    public Building parentObj;

    public bool isPushed = false;
    /// <summary>
    /// 碰撞的自己和对方
    /// </summary>
    private Building self = null;
    private Building other = null;
    private Renderer selfRender = null;
    private Renderer otherRender = null;
    private List<GameObject> otherList = new List<GameObject>();

    private BuildSystem bSystem;
    /// <summary>
    /// 是否限制周围不能摆放
    /// </summary>
    public bool limitAround = false;
    /// <summary>
    /// 是否是地图
    /// </summary>
    public bool isTerrin = false;

    public bool IsGravity = false;
    private Building colBuilding;
    private Renderer colRender;
    private void Start()
    {
        bSystem = GameObject.Find("BuildSystem").GetComponent<BuildSystem>();
    }


    internal void SetIsCanPush(bool push)
    {
        IsCPush = push;
    }
    internal bool GetIsCanPush()
    {
        return IsCPush;
    }
    internal void SetNowParent(Building obj)
    {
        parentObj = obj;
    }
    internal Building GetNowParent()
    {
        return parentObj;
    }

    internal void SetIsPushed(bool Pushed)
    {
        isPushed = Pushed;
    }
    internal bool GetIsPushed()
    {
        return isPushed;
    }
    internal bool GetlimitAround()
    {
        return limitAround;
    }
    private void OnCollisionStay(Collision collision)
    {
        //作用于物体和物体之间
        if (collision.collider.gameObject.layer == 0)
        {
            //正持有的物体
            self = transform.GetComponent<Building>();
            selfRender = self.GetComponent<Renderer>();
            //放置的物品或者地板墙面
            other = collision.collider.GetComponent<Building>();
            otherRender = other.GetComponent<Renderer>();
            if (other && self && !self.GetIsPushed())
            {
                //如果你物体旋转,这个y会自动切换
                var offset = otherRender.bounds.size.y / 2;
                bool ISF = JudgeTransfrom.IsUp(other.transform, self.transform.position - new Vector3(0, offset, 0));
                //添加碰撞过的物品
                if (!otherList.Contains(other.gameObject))
                {
                    otherList.Add(other.gameObject);
                }
                if (!self.GetIsPushed())
                {
                    if (other.isTerrin)
                    {
                        if (ISF)
                        {
                            if (!other.GetIsCanPush())
                            {
                                selfRender.material.color = bSystem.colorList[1];
                            }
                            else
                            {
                                selfRender.material.color = bSystem.colorList[0];
                            }
                        }
                        else
                        {
                            selfRender.material.color = bSystem.colorList[1];
                        }
                    }
                    else
                    {
                        //判断是否接触两个物体。如果接触两个物体会出现穿模的情况，需要去掉这种接触两个物体穿模的情况
                        if (otherList.Count > 1)
                        {
                            selfRender.material.color = bSystem.colorList[1];
                        }
                        else
                        {
                            selfRender.material.color = bSystem.colorList[0];
                        }
                    }
                }
            }
        }
        else
        {
            colBuilding = collision.collider.GetComponent<Building>();
            colRender = collision.collider.GetComponent<Renderer>();
            if (colBuilding.GetNowParent() != transform.gameObject)
            {
                colRender.material.color = bSystem.colorList[1];
            }
            else
            {
                if (parentObj)
                {
                    var offset = colRender.bounds.size.y;
                    float upValue = JudgeTransfrom.GetIsUpValue(parentObj.transform, colBuilding.transform.position - new Vector3(0, offset, 0));
                    //判断是否是地板或者墙面
                    if (isTerrin)
                    {
                        colRender.material.color = bSystem.colorList[0];
                    }
                    else
                    {
                        if (upValue > offset / 2)
                        {
                            colRender.material.color = bSystem.colorList[0];
                        }
                        else
                        {
                            //判断是否是放置的物品，并且还不是地形，手持物体变为红色，不可以放置
                            if (parentObj.GetIsPushed() && !parentObj.isTerrin)
                            {
                                colRender.material.color = bSystem.colorList[1];
                            }
                            else
                            {
                                colRender.material.color = bSystem.colorList[0];
                            }
                        }
                        //可以允许周围摆放
                        if (limitAround)
                        {
                            colRender.material.color = bSystem.colorList[0];
                        }
                    }
                }
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        colBuilding = collision.collider.GetComponent<Building>();
        colRender = collision.collider.GetComponent<Renderer>();
        if (colBuilding)
        {
            if (!colBuilding.GetIsPushed())
            {
                colRender.material.color = bSystem.colorList[0];
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
