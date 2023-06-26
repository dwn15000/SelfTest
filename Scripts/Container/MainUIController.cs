using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour
{
    
    /// <summary>
    /// ���ɵ��ı���Ҫ�õ�������
    /// </summary>
    public Font font;

    private Canvas canvas;


    public bool MsgBoxIsShow = false;
    /// <summary>
    /// ��ǰѡ���ģ������
    /// </summary>
    private string CurrSelectModelName;

    /// <summary>
    /// ���ɵ�text
    /// </summary>
    private Text txt_ObjName;

    private ModelDataConfig modeDataConfig;

    public bool isDestory = true;


    /// <summary>
    /// ��ǰ�����ķ�����ģ������
    /// </summary>
    private SelectModelAttribute curr_HaveOperateModel;

    private bool isCanExplosion = false;

    /// <summary>
    /// ��ת���ٶ����
    /// </summary>
    public ModelRotate[] CMGRotateArr = new ModelRotate[6];

    /// <summary>
    /// ���ת�����
    /// </summary>
    public CMGRotate[] CMGAngArr = new CMGRotate[6];

    /// <summary>
    /// �Ƿ���Ա�ը
    /// </summary>
    public bool  GetCanExplosion
    {

        set { isCanExplosion = value; }
        get { return isCanExplosion; }
    }


    /// <summary>
    ///��ǰѡ���ģ������
    /// </summary>
    public string GetCurrSelectModelName
    {

        set { CurrSelectModelName = value; }
        get { return CurrSelectModelName; }
    }


    /// <summary>
    /// ��ǰ�����ķ�����ģ������
    /// </summary>
    public SelectModelAttribute GetCurrOperateModel
    {

        set { curr_HaveOperateModel = value; }
        get { return curr_HaveOperateModel; }
    }


    /// <summary>
    /// �����ļ����
    /// </summary>
    public ModelDataConfig GetModelDataConfig
    {

        set { modeDataConfig = value; }
        get { return modeDataConfig; }
    }


    /// <summary>
    /// Canvas���
    /// </summary>
    public Canvas GetCanvas {

        set { canvas = value; }
        get { return canvas; } 
    }

    /// <summary>
    /// ����������������txt
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
    /// ��Ҫ�õ���ģ����ת������
    /// </summary>
    public List<string> allModelRotateName = new List<string>();

    /// <summary>
    /// ģ��shader��������
    /// </summary>
    public List<string> allModelShaderData = new List<string>();

    private void Awake()
    {
        Debug.LogError("��Ҫ�õ���ģ����ת������");

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
    ///��������ӱ�����������
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
        // ��ȡ�ļ�·��
        filePath = Path.Combine(Application.dataPath, "Resources/" + fileName + ".txt");
        // �����ı��ļ�
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
        // ��ȡ�ļ�·��
        filePath = Path.Combine(Application.dataPath, "Resources/" + ModelRotatefileName + ".txt");
        // �����ı��ļ�
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
        // ��ȡ�ļ�·��
        filePath = Path.Combine(Application.dataPath, "Resources/" + ModelShaderParamName + ".txt");
        // �����ı��ļ�
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
