using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class LoadFileConfig : MonoBehaviour
{
    /// <summary>
    /// 开始启动游戏的物体
    /// </summary>
    public GameObject StartCallGameObj;

    public string ModelRotatefileName;

    /// <summary>
    /// 需要用到的模型旋转的名字
    /// </summary>
    public List<string> allModelRotateName = new List<string>();

    void Awake()
    {
        allModelRotateName.Clear();
        IocContainer_InstanceMgr.GetInstance().RegistInstance(this);

        string exePath = Path.GetDirectoryName(Application.dataPath);
        string filePath = Path.Combine(exePath, "config.txt");
        string configJson = File.ReadAllText(filePath);
        Debug.Log("加载配置文件数据:"+configJson);
        string[] strs = configJson.Split('\n');

        ProcedureSetting.UseSatelliteStype = int.Parse(strs[0]);
        ProcedureSetting.ip = strs[1].Split(':')[1];
        ProcedureSetting.port = int.Parse(strs[2].Split(':')[1]);
        ProcedureSetting.NetModel = int.Parse(strs[3]);
        if (int.Parse(strs[4])==3)
        {
            Debug.Log("删除了缓存数据");
            //PlayerPrefs.DeleteAll();
        }
        //Client_UDP_UsingSatelliteDataFromDigitalTwinMachine.UDPClientIP = ProcedureSetting.ip;
        //Client_UDP_UsingSatelliteDataFromDigitalTwinMachine.UDPServerPort = ProcedureSetting.port;
        //StartCallGameObj.SetActive(true);

        LoadModelRotateData();

        //CopyFiles();  // 复制配置文件，加的
    }


    /// <summary>
    /// 加载卫星模型用到的数据
    /// </summary>
    void LoadModelRotateData()
    {
        // 获取文件路径
        string filePath = Path.Combine(Application.dataPath, "Resources/" + ModelRotatefileName + ".txt");
        // 加载文本文件
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
        string sourceDir = Application.dataPath + "/Configs"; // 配置文件所在目录
                                                              // string targetDir = Application.dataPath.Replace("Assets", "Builds/Windows"); // 目标目录，原来的
        string targetDir = Application.dataPath.Replace("Assets", "Apps/Windows版/Windows版_V0.3_2023.05.23"); // 目标目录
        if (!Directory.Exists(targetDir)) // 如果目标目录不存在，则创建
        {
            Directory.CreateDirectory(targetDir);
        }
        DirectoryInfo sourceDirInfo = new DirectoryInfo(sourceDir);
        FileInfo[] files = sourceDirInfo.GetFiles("*.*", SearchOption.AllDirectories); // 获取配置文件目录下的所有文件
        foreach (FileInfo file in files)
        {
            string targetFilePath = Path.Combine(targetDir, file.Name); // 目标文件路径
            File.Copy(file.FullName, targetFilePath, true); // 复制文件
        }
        Debug.Log("复制配置文件成功");
    }

  
}
