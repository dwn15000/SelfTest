using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteAssembly
{
    private static SatelliteAssembly _instance = null;

    /// <summary>
    /// 得到单例
    /// </summary>
    /// <returns></returns>
    public static SatelliteAssembly GetInstance()
    {
        if (_instance == null)
        {
            _instance = new SatelliteAssembly();
        }

        return _instance;
    }


    /// <summary>
    /// 参数名字
    /// </summary>
    private string paramName;
    public string ParamName
    {
        get { return paramName; }
        set { paramName = value; }
    }

    /// <summary>
    /// 参数值
    /// </summary>
    private float paramValue;
    public float ParamValue
    {
        get { return paramValue; }
        set { paramValue = value; }
    }

    private Dictionary<string, string> SateInfoDic = new Dictionary<string, string>();


    /// <summary>
    /// 获取所有卫星信息
    /// </summary>
    public Dictionary<string, string> GetSateAssemblyInfo
    {
        get { return SateInfoDic; }
        set { SateInfoDic = value; }
    }


    /// <summary>
    /// 添加卫星参数信息
    /// </summary>
    public void AddSingleSateInfo(string paramName, string paramValue)
    {
        if (!SateInfoDic.ContainsKey(paramName))
        {
            SateInfoDic.Add(paramName,paramValue);
        }
    }
}
