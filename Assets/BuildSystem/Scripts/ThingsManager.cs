using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ThingsManager : Singleton<ThingsManager>
{
    List<string> prefabs = new List<string>();
    public override void Init()
    {

    }
    /// <summary>
    /// 打开/关闭重力
    /// </summary>
    internal void SetGravity(Rigidbody thing, bool flag)
    {
        if (flag)
        {
            thing.useGravity = true;
        }
        else
        {
            thing.useGravity = false;
        }
    }
    /// <summary>
    /// 打开/关闭电视
    /// </summary>
    /// <param name="thing"></param>
    /// <param name="flag"></param>
    internal void OpenTV(Television tv, bool flag)
    {
        if (flag)
        {
            tv.Open();
        }
        else
        {
            tv.Close();
        }
    }
    /// <summary>
    /// 设置电视相机拍摄的位置
    /// </summary>
    /// <param name="thing"></param>
    /// <param name="flag"></param>
    internal void SetTVCamPos(Television tv, Vector3 pos, Quaternion rot)
    {
        tv.transform.position = pos;
        tv.transform.rotation = rot;
    }

    //解析json
    public string JsonLoad(string FullName)
    {
        //操作文件写入信息
        StreamReader sr = new StreamReader(FullName);
        if (sr != null)
        {
            string json = sr.ReadToEnd();
            if (json.Length > 0)
            {
                sr.Close();
                return json;
            }
        }
        sr.Close();
        return null;
    }
    //写入json
    public void JsonModify(string FullName, string content)
    {
        //操作文件写入信息
        StreamWriter sw = new StreamWriter(FullName);
        sw.Write(content);
        sw.Close();
    }

    internal void initCameraState()
    {
        CameraState.Instance.setPos(Camera.main.transform.position);
        CameraState.Instance.setRot(Camera.main.transform.rotation);
    }
    internal void SetCameraState()
    {
        //相机恢复
        Camera.main.transform.position = CameraState.Instance.getPos();
        Camera.main.transform.rotation = CameraState.Instance.getRot();
    }
    /// <summary>
    /// 添加配置
    /// </summary>
    /// <param name="jpath">配置文件路径</param>
    /// <param name="sIndex">场景id</param>
    /// <param name="item">添加的物品</param>
    public void AddBuildThings(string jpath, int sIndex, BuildSceneItem item)
    {
        string content = JsonLoad(jpath);
        var loadModle = JsonConvert.DeserializeObject<BuildScenes>(content);
        loadModle.buildScene[sIndex].buildThings.Add(item);
        var writeMode = JsonConvert.SerializeObject(loadModle, Formatting.Indented);
        JsonModify(jpath, writeMode);
    }
    /// <summary>
    /// 删除配置
    /// </summary>
    /// <param name="jpath">配置文件路径</param>
    /// <param name="sIndex">场景id</param>
    /// <param name="tIndex">物品id</param>
    public void DelBuildThings(string jpath, int sIndex, int tIndex)
    {
        string content = JsonLoad(jpath);
        var loadModle = JsonConvert.DeserializeObject<BuildScenes>(content);
        BuildSceneItem item = eachItem(loadModle.buildScene[sIndex].buildThings, tIndex);
        loadModle.buildScene[sIndex].buildThings.Remove(item);
        var writeMode = JsonConvert.SerializeObject(loadModle, Formatting.Indented);
        JsonModify(jpath, writeMode);
    }
    /// <summary>
    /// 查找物体
    /// </summary>
    /// <param name="buildThings">物品列表</param>
    /// <param name="tIndex">物品id</param>
    /// <returns></returns>
    public BuildSceneItem eachItem(List<BuildSceneItem> buildThings, int tIndex)
    {
        foreach (var item in buildThings)
        {
            if (item.thingID == tIndex)
            {
                return item;
            }
        }
        return null;
    }

    public List<string> getThingsName()
    {
        return prefabs;
    }
    /// <summary>
    /// 更换材质球
    /// </summary>
    /// <param name="m_replaces"></param>
    /// <param name="material"></param>
    public void relaceMaterial(GameObject m_replaces, Material material)
    {
        if (material == null)
        {
            return;
        }
        m_replaces.GetComponent<Renderer>().material = material;//代表这个对象的共享材质资源（这个是替换材质球）
    }
    /// <summary>
    /// 更换shader
    /// </summary>
    /// <param name="m_replaces"></param>
    /// <param name="index"></param>
    /// <param name="shaderName"></param>
    public void relaceMaterialsForIndex(GameObject m_replaces,int index,string shaderName)
    {
        m_replaces.GetComponent<Renderer>().materials[index] = new Material(Shader.Find(shaderName)) ;
    }
}
/// <summary>
/// 相机的属性
/// </summary>
public class CameraState:Singleton<CameraState>
{
    public Vector3 pos;
    public Quaternion Rot;
    internal void setPos(Vector3 posValue)
    {
        pos = posValue;
    }
    internal void setRot(Quaternion RotValue)
    {
        Rot = RotValue;
    }
    internal Vector3 getPos()
    {
        Debug.Log(pos);
        return pos;
    }
    internal Quaternion getRot()
    {
        Debug.Log(Rot);
        return Rot;
    }
}

