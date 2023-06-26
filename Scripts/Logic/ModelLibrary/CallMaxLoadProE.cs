using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TriLibCore.Samples;
using UnityEngine;

public class CallMaxLoadProE : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// 调起3DMax
    /// </summary>
    public void Call3DMax(string path)
    {
        // ProE模型文件路径
        string proEFilePath = path;//"D:/Model/kg.prt";
       
        string firstName = path.Split('.')[0];
        // FBX文件输出路径
        string fbxFilePath = firstName+".fbx"; //"D:/Model/kg.fbx"
        // 构造3dMax命令行参数
       
        string arguments = string.Format("-mxs \"resetMaxFile #noPrompt; importFile \\\"{0}\\\"; exportFile \\\"{1}\\\" #noPrompt selectedOnly:true; quitMax #noPrompt;\"", proEFilePath, fbxFilePath);
       
        // 启动3dMax进程
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = @"C:\Program Files\Autodesk\3ds Max 2024\3dsmax.exe";
        startInfo.Arguments = arguments;
        startInfo.RedirectStandardOutput = true;
        startInfo.UseShellExecute = false;
        Process process = Process.Start(startInfo);
      
        // 等待3dMax进程结束
        process.WaitForExit();
        //System.Windows.Forms.SendKeys.SendWait("%{F}I");
        SaveData(fbxFilePath);
    }


    void SaveData(string modelFilename)
    {
        UnityEngine.Debug.Log("获取到的最终的路径:"+modelFilename);
        SaveModeData(modelFilename);
        string name = Path.GetFileName(modelFilename);

        ///插入数据到本地数据库
        //IocContainer_InstanceMgr.GetInstance().GetInstance<SQLiteDemo>().Insert((name.Split('.')[0]), modelFilename, "model");

        IocContainer_InstanceMgr.GetInstance().GetInstance<LoadModelFromFilePickerSample>().WaidCallLoadModel(modelFilename);
    }

    /// <summary>
    /// 将模型数据保存到本地
    /// </summary>
    /// <param name="filePath"></param>
    void SaveModeData(string filePath)
    {
        byte[] fileBytes = File.ReadAllBytes(filePath);
        //string savePath = Path.Combine(Application.persistentDataPath, Path.GetFileName(filePath));
        string savePath = UnityEngine.Application.persistentDataPath + "/" + Path.GetFileName(filePath);

        File.WriteAllBytes(savePath, fileBytes);
        PlayerPrefs.SetString("NewSate", savePath);
    }

    private void Awake()
    {
        IocContainer_InstanceMgr.GetInstance().RegistInstance(this);
    }
   
}
