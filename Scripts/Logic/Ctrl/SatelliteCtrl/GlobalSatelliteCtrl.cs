using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSatelliteCtrl
{
    private static GlobalSatelliteCtrl _instance = null;

    public static GlobalSatelliteCtrl GetInstance()
    {
        if (_instance==null)
        {
            _instance = new GlobalSatelliteCtrl();
        }
        return _instance;
    }


    /// <summary>
    /// 所有的卫星名字
    /// </summary>
    private List<SetSatelliteName> allSatelliteNames = new List<SetSatelliteName>();

    public List<SetSatelliteName> GetAllSatelliteName
    {
        get { return allSatelliteNames; }
        set { allSatelliteNames = value; }
    }

    /// <summary>
    /// 添加卫星名字组件到集合
    /// </summary>
    /// <param name="saName"></param>
    public void AddSatelliteName(SetSatelliteName saName)
    {
        allSatelliteNames.Add(saName);
    }
}
