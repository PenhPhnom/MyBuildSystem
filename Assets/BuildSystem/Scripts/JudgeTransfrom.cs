using System;
using UnityEngine;

/// <summary>
/// 判断前后左右的封装
/// </summary>
public class JudgeTransfrom
{
    /// <summary>
    /// 判断在基于位置的是否在前后
    /// </summary>
    /// <param name="baseTrans"></param>
    /// <param name="dir"></param>
    /// <returns>true:前；false:后</returns>
    public static bool IsUp(Transform baseTrans, Vector3 pos)
    {
        return GetIsUpValue(baseTrans, pos) > 0 ? true : false;
    }

    /// <summary>
    /// 判断在基于位置的是否在前后
    /// </summary>
    /// <param name="baseTrans"></param>
    /// <param name="dir"></param>
    /// <returns>true:前；false:后</returns>
    public static bool IsForward(Transform baseTrans, Vector3 pos)
    {
        return GetIsForwardValue(baseTrans, pos) >= 0 ? true : false;
    }

    /// <summary>
    /// 判断在基于位置的是否在左右
    /// </summary>
    /// <param name="baseTrans"></param>
    /// <param name="dir"></param>
    /// <returns>true:右；false:左</returns>
    public static bool IsRight(Transform baseTrans, Vector3 pos)
    {
        return GetIsRightValue(baseTrans, pos) >= 0 ? true : false;
    }

    // 大于0，前
    // pos 为 0 ，结果会出错 (做好是相对位置)
    public static float GetIsForwardValue(Transform baseTrans, Vector3 pos)
    {
        return Vector3.Dot(baseTrans.forward, pos - baseTrans.position);
    }

    // 大于0，右边
    // pos 为 0 ，结果会出错(做好是相对位置)
    public static float GetIsRightValue(Transform baseTrans, Vector3 pos)
    {
        return Vector3.Cross(baseTrans.forward, pos - baseTrans.position).y;
    }
    // 大于0，上
    // pos 为 0 ，结果会出错 (做好是相对位置)
    public static float GetIsUpValue(Transform baseTrans, Vector3 pos)
    {
        return Vector3.Dot(baseTrans.up, pos - baseTrans.position);
    }
}
