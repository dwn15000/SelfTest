using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteData
{

    private static SatelliteData _instance = null;

    /// <summary>
    /// 得到单例
    /// </summary>
    /// <returns></returns>
    public static SatelliteData GetInstance()
    {
        if (_instance==null)
        {
            _instance = new SatelliteData();
        }
        return _instance;
    }


    /// <summary>
    /// 卫星名字
    /// </summary>
    private  string satelliteName;
    public string ParamName
    {
        get { return satelliteName; }
        set { satelliteName = value; }
    }

    /// <summary>
    /// 卫星存放路径
    /// </summary>
    private string satellitePath;
    public string ParamValue
    {
        get { return satellitePath; }
        set { satellitePath = value; }
    }

    private List<SatelliteNameAndPath> satelliteList = new List<SatelliteNameAndPath>();


    /// <summary>
    /// 获取所有卫星数据
    /// </summary>
    public List<SatelliteNameAndPath> GetSateInfo
    {
        get { return satelliteList; }
        set { satelliteList = value; }
    }


    /// <summary>
    /// 添加卫星
    /// </summary>
    public void AddSingleSateInfo(string sateName, string satePath)
    {
        satelliteList.Add(new SatelliteNameAndPath(sateName,satePath));
    }
}

public class SatelliteNameAndPath
{
    /// <summary>
    /// 卫星名字
    /// </summary>
    public string satelliteName;

    /// <summary>
    /// 卫星存放路径
    /// </summary>
    public string satellitePath;

    public SatelliteNameAndPath(string sName,string sPath)
    {
        this.satelliteName = sName;
        this.satellitePath = sPath;
    }
}