using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteAssembly
{
    private static SatelliteAssembly _instance = null;

    /// <summary>
    /// �õ�����
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
    /// ��������
    /// </summary>
    private string paramName;
    public string ParamName
    {
        get { return paramName; }
        set { paramName = value; }
    }

    /// <summary>
    /// ����ֵ
    /// </summary>
    private float paramValue;
    public float ParamValue
    {
        get { return paramValue; }
        set { paramValue = value; }
    }

    private Dictionary<string, string> SateInfoDic = new Dictionary<string, string>();


    /// <summary>
    /// ��ȡ����������Ϣ
    /// </summary>
    public Dictionary<string, string> GetSateAssemblyInfo
    {
        get { return SateInfoDic; }
        set { SateInfoDic = value; }
    }


    /// <summary>
    /// ������ǲ�����Ϣ
    /// </summary>
    public void AddSingleSateInfo(string paramName, string paramValue)
    {
        if (!SateInfoDic.ContainsKey(paramName))
        {
            SateInfoDic.Add(paramName,paramValue);
        }
    }
}
