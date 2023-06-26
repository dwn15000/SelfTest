using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedureSetting
{
    /// <summary>
    /// 主场景名字
    /// </summary>
    public const string MainSceneName = "Home";

    /// <summary>
    /// 是否用本地数据
    /// </summary>
    public const bool IsUseLocalData = true;  //如果用Matlab上的真实卫星数据，就把IsUseLocalData改为false;如果用本地CSV卫星数据，就把IsUseLocalData改为true

    /// <summary>
    /// 是否是卫星视角
    /// </summary>
    public static bool IsSatelliteView = false;

    /// <summary>
    /// 采用数据的模式
    /// </summary>
    public static int UseSatelliteStype = 1;


    /// <summary>
    /// 服务端IP
    /// </summary>
    public static string ip = "";

    /// <summary>
    /// 端口号
    /// </summary>
    public static int port = 0;

    public static int NetModel = 1;

}
