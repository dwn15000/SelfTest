using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedureSetting
{
    /// <summary>
    /// ����������
    /// </summary>
    public const string MainSceneName = "Home";

    /// <summary>
    /// �Ƿ��ñ�������
    /// </summary>
    public const bool IsUseLocalData = true;  //�����Matlab�ϵ���ʵ�������ݣ��Ͱ�IsUseLocalData��Ϊfalse;����ñ���CSV�������ݣ��Ͱ�IsUseLocalData��Ϊtrue

    /// <summary>
    /// �Ƿ��������ӽ�
    /// </summary>
    public static bool IsSatelliteView = false;

    /// <summary>
    /// �������ݵ�ģʽ
    /// </summary>
    public static int UseSatelliteStype = 1;


    /// <summary>
    /// �����IP
    /// </summary>
    public static string ip = "";

    /// <summary>
    /// �˿ں�
    /// </summary>
    public static int port = 0;

    public static int NetModel = 1;

}
