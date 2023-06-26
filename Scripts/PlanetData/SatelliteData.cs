using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteData
{

    private static SatelliteData _instance = null;

    /// <summary>
    /// �õ�����
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
    /// ��������
    /// </summary>
    private  string satelliteName;
    public string ParamName
    {
        get { return satelliteName; }
        set { satelliteName = value; }
    }

    /// <summary>
    /// ���Ǵ��·��
    /// </summary>
    private string satellitePath;
    public string ParamValue
    {
        get { return satellitePath; }
        set { satellitePath = value; }
    }

    private List<SatelliteNameAndPath> satelliteList = new List<SatelliteNameAndPath>();


    /// <summary>
    /// ��ȡ������������
    /// </summary>
    public List<SatelliteNameAndPath> GetSateInfo
    {
        get { return satelliteList; }
        set { satelliteList = value; }
    }


    /// <summary>
    /// �������
    /// </summary>
    public void AddSingleSateInfo(string sateName, string satePath)
    {
        satelliteList.Add(new SatelliteNameAndPath(sateName,satePath));
    }
}

public class SatelliteNameAndPath
{
    /// <summary>
    /// ��������
    /// </summary>
    public string satelliteName;

    /// <summary>
    /// ���Ǵ��·��
    /// </summary>
    public string satellitePath;

    public SatelliteNameAndPath(string sName,string sPath)
    {
        this.satelliteName = sName;
        this.satellitePath = sPath;
    }
}