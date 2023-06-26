using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour
{
    
    /// <summary>
    /// 生成的文本需要用到的字体
    /// </summary>
    public Font font;

    private Canvas canvas;


    public bool MsgBoxIsShow = false;
    /// <summary>
    /// 当前选择的模型名字
    /// </summary>
    private string CurrSelectModelName;

    /// <summary>
    /// 生成的text
    /// </summary>
    private Text txt_ObjName;

    private ModelDataConfig modeDataConfig;

    public bool isDestory = true;


    /// <summary>
    /// 当前操作的飞行器模型数据
    /// </summary>
    private SelectModelAttribute curr_HaveOperateModel;

    private bool isCanExplosion = false;

    /// <summary>
    /// 内转子速度组件
    /// </summary>
    public ModelRotate[] CMGRotateArr = new ModelRotate[6];

    /// <summary>
    /// 外壳转速组件
    /// </summary>
    public CMGRotate[] CMGAngArr = new CMGRotate[6];

    /// <summary>
    /// 是否可以爆炸
    /// </summary>
    public bool  GetCanExplosion
    {

        set { isCanExplosion = value; }
        get { return isCanExplosion; }
    }


    /// <summary>
    ///当前选择的模型名字
    /// </summary>
    public string GetCurrSelectModelName
    {

        set { CurrSelectModelName = value; }
        get { return CurrSelectModelName; }
    }


    /// <summary>
    /// 当前操作的飞行器模型数据
    /// </summary>
    public SelectModelAttribute GetCurrOperateModel
    {

        set { curr_HaveOperateModel = value; }
        get { return curr_HaveOperateModel; }
    }


    /// <summary>
    /// 配置文件组件
    /// </summary>
    public ModelDataConfig GetModelDataConfig
    {

        set { modeDataConfig = value; }
        get { return modeDataConfig; }
    }


    /// <summary>
    /// Canvas组件
    /// </summary>
    public Canvas GetCanvas {

        set { canvas = value; }
        get { return canvas; } 
    }

    /// <summary>
    /// 鼠标移入物体的名字txt
    /// </summary>
    public Text GetObjName
    {

        set { txt_ObjName = value; }
        get { return txt_ObjName; }

    }

    public string fileName;
    public string ModelRotatefileName;
    public string ModelShaderParamName;

    private string filePath;

    string[] datas;

    /// <summary>
    /// 需要用到的模型旋转的名字
    /// </summary>
    public List<string> allModelRotateName = new List<string>();

    /// <summary>
    /// 模型shader参数数据
    /// </summary>
    public List<string> allModelShaderData = new List<string>();

    private void Awake()
    {
        Debug.LogError("需要用到的模型旋转的名字");

        //GlobalAction.GetInsatance().Init();
        if (isDestory)
        {
            GameObject.DontDestroyOnLoad(this.gameObject);
        }
        
        GetCanvas = GameObject.Find("Canvas").gameObject.GetComponent<Canvas>();
        IocContainer_InstanceMgr.GetInstance().RegistInstance(this);
    }
    void Start()
    {
       
        InitConfig();
        LoadModelRotateData();
        LoadModelShaderParamData();
    }

    /// <summary>
    ///给卫星添加本地数据驱动
    /// </summary>
    public void GiveSatelliteAddLocalDrive(GameObject go)
    {
        //if (go.GetComponent<LoadQuests>()==null)
        //{
        //    go.AddComponent<LoadQuests>().trans = go.transform;
        //}
    }



    public void InitConfig()
    {
        // 获取文件路径
        filePath = Path.Combine(Application.dataPath, "Resources/" + fileName + ".txt");
        // 加载文本文件
        string text = Resources.Load<TextAsset>(fileName).text;
        Debug.Log(text);
        datas = text.Split('\n');
        string c_time = ParsingIndividualData(datas[0], 1);
        string e_time = ParsingIndividualData(datas[1], 1);
        Debug.Log("c_time=" + c_time + ",,e_time=" + e_time);
        IocContainer_InstanceMgr.GetInstance().GetInstance<MainUIController>().GetModelDataConfig = new ModelDataConfig(c_time, e_time);
    }


    void LoadModelRotateData()
    {
        // 获取文件路径
        filePath = Path.Combine(Application.dataPath, "Resources/" + ModelRotatefileName + ".txt");
        // 加载文本文件
        string text = Resources.Load<TextAsset>(ModelRotatefileName).text;
        //Debug.Log(text);
        datas = text.Split('\n');
        for (int i = 0; i < datas.Length; i++)
        {
            allModelRotateName.Add(datas[i]);
        }
    }

    void LoadModelShaderParamData()
    {
        // 获取文件路径
        filePath = Path.Combine(Application.dataPath, "Resources/" + ModelShaderParamName + ".txt");
        // 加载文本文件
        string text = Resources.Load<TextAsset>(ModelShaderParamName).text;
        //Debug.Log(text);
        datas = text.Split('\n');
        for (int i = 0; i < datas.Length; i++)
        {
            allModelShaderData.Add(datas[i]);
        }
    }



    string ParsingIndividualData(string data, int inx)
    {

        return data.Split(':')[inx];
    }
    private void OnDestroy()
    {
        IocContainer_InstanceMgr.GetInstance().RemoveInstance<MainUIController>();
    }
   
}
