using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class Television : MonoBehaviour
{
    public Camera mainCam;           //目标摄像机
    RenderTexture rt;                //声明一个中间变量 
    Texture2D t2d;
    public int Width = 800;
    public int Height = 600;
    public bool State = false;
    void Start()
    {
        t2d = new Texture2D(Width, Height, TextureFormat.RGB24, false);
        rt = new RenderTexture(Width, Height, 24);
        mainCam.targetTexture = rt;
    }

    void Update()
    {
        if(State)
        {
            //显示视频到物体上
            transform.GetComponent<Renderer>().material.mainTexture = rt;
            //截图到t2d中
            RenderTexture.active = rt;
            t2d.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            t2d.Apply();
            RenderTexture.active = null;
        }
        else
        {
            //显示视频到物体上
            transform.GetComponent<Renderer>().material.mainTexture = null;
        }
    }

    internal void Close()
    {
        State = false;
    }

    internal void Open()
    {
        State = true;
    }
}