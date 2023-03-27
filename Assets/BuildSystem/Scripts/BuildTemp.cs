using System.Collections.Generic;

public class BuildScene
{
    /// <summary>
    /// 
    /// </summary>
    public string buildSceneName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<BuildSceneItem> buildThings = new List<BuildSceneItem>();
}
public class BuildSceneItem
{
    public string thingName;
    public string thingParentName;
    public int thingType;
    public int thingID;
    public float postionX;
    public float postionY;
    public float postionZ;
    public float rotationX;
    public float rotationY;
    public float rotationZ;
    public bool isUseGravity;
    public bool FreezePostion;
    public bool FreezeRotation;
    public bool isCPush;
    public bool isPushed;
    public bool LimitAround;
    public bool isTerrin;
}

public class BuildScenes
{
    public List<BuildScene> buildScene = new List<BuildScene>();
}
public class Things
{
    public List<BuildSceneItem> thingsArr = new List<BuildSceneItem>();
}