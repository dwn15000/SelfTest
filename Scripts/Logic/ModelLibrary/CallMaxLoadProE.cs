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
    /// ����3DMax
    /// </summary>
    public void Call3DMax(string path)
    {
        // ProEģ���ļ�·��
        string proEFilePath = path;//"D:/Model/kg.prt";
       
        string firstName = path.Split('.')[0];
        // FBX�ļ����·��
        string fbxFilePath = firstName+".fbx"; //"D:/Model/kg.fbx"
        // ����3dMax�����в���
       
        string arguments = string.Format("-mxs \"resetMaxFile #noPrompt; importFile \\\"{0}\\\"; exportFile \\\"{1}\\\" #noPrompt selectedOnly:true; quitMax #noPrompt;\"", proEFilePath, fbxFilePath);
       
        // ����3dMax����
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = @"C:\Program Files\Autodesk\3ds Max 2024\3dsmax.exe";
        startInfo.Arguments = arguments;
        startInfo.RedirectStandardOutput = true;
        startInfo.UseShellExecute = false;
        Process process = Process.Start(startInfo);
      
        // �ȴ�3dMax���̽���
        process.WaitForExit();
        //System.Windows.Forms.SendKeys.SendWait("%{F}I");
        SaveData(fbxFilePath);
    }


    void SaveData(string modelFilename)
    {
        UnityEngine.Debug.Log("��ȡ�������յ�·��:"+modelFilename);
        SaveModeData(modelFilename);
        string name = Path.GetFileName(modelFilename);

        ///�������ݵ��������ݿ�
        //IocContainer_InstanceMgr.GetInstance().GetInstance<SQLiteDemo>().Insert((name.Split('.')[0]), modelFilename, "model");

        IocContainer_InstanceMgr.GetInstance().GetInstance<LoadModelFromFilePickerSample>().WaidCallLoadModel(modelFilename);
    }

    /// <summary>
    /// ��ģ�����ݱ��浽����
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
