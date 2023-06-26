using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class LoadFileConfig : MonoBehaviour
{
    /// <summary>
    /// ��ʼ������Ϸ������
    /// </summary>
    public GameObject StartCallGameObj;

    public string ModelRotatefileName;

    /// <summary>
    /// ��Ҫ�õ���ģ����ת������
    /// </summary>
    public List<string> allModelRotateName = new List<string>();

    void Awake()
    {
        allModelRotateName.Clear();
        IocContainer_InstanceMgr.GetInstance().RegistInstance(this);

        string exePath = Path.GetDirectoryName(Application.dataPath);
        string filePath = Path.Combine(exePath, "config.txt");
        string configJson = File.ReadAllText(filePath);
        Debug.Log("���������ļ�����:"+configJson);
        string[] strs = configJson.Split('\n');

        ProcedureSetting.UseSatelliteStype = int.Parse(strs[0]);
        ProcedureSetting.ip = strs[1].Split(':')[1];
        ProcedureSetting.port = int.Parse(strs[2].Split(':')[1]);
        ProcedureSetting.NetModel = int.Parse(strs[3]);
        if (int.Parse(strs[4])==3)
        {
            Debug.Log("ɾ���˻�������");
            //PlayerPrefs.DeleteAll();
        }
        //Client_UDP_UsingSatelliteDataFromDigitalTwinMachine.UDPClientIP = ProcedureSetting.ip;
        //Client_UDP_UsingSatelliteDataFromDigitalTwinMachine.UDPServerPort = ProcedureSetting.port;
        //StartCallGameObj.SetActive(true);

        LoadModelRotateData();

        //CopyFiles();  // ���������ļ����ӵ�
    }


    /// <summary>
    /// ��������ģ���õ�������
    /// </summary>
    void LoadModelRotateData()
    {
        // ��ȡ�ļ�·��
        string filePath = Path.Combine(Application.dataPath, "Resources/" + ModelRotatefileName + ".txt");
        // �����ı��ļ�
        string text = Resources.Load<TextAsset>(ModelRotatefileName).text;
        //Debug.Log(text);
       string[] datas = text.Split('\n');
        for (int i = 0; i < datas.Length; i++)
        {
            allModelRotateName.Add(datas[i]);
        }
    }


    static void CopyFiles()
    {
        string sourceDir = Application.dataPath + "/Configs"; // �����ļ�����Ŀ¼
                                                              // string targetDir = Application.dataPath.Replace("Assets", "Builds/Windows"); // Ŀ��Ŀ¼��ԭ����
        string targetDir = Application.dataPath.Replace("Assets", "Apps/Windows��/Windows��_V0.3_2023.05.23"); // Ŀ��Ŀ¼
        if (!Directory.Exists(targetDir)) // ���Ŀ��Ŀ¼�����ڣ��򴴽�
        {
            Directory.CreateDirectory(targetDir);
        }
        DirectoryInfo sourceDirInfo = new DirectoryInfo(sourceDir);
        FileInfo[] files = sourceDirInfo.GetFiles("*.*", SearchOption.AllDirectories); // ��ȡ�����ļ�Ŀ¼�µ������ļ�
        foreach (FileInfo file in files)
        {
            string targetFilePath = Path.Combine(targetDir, file.Name); // Ŀ���ļ�·��
            File.Copy(file.FullName, targetFilePath, true); // �����ļ�
        }
        Debug.Log("���������ļ��ɹ�");
    }

  
}
