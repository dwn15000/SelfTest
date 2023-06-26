using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UISatelliteSubstellar : UIBase
{
    /// <summary>
    /// ��̬����
    /// </summary>
    public static void StaticHide()
    {
        if (self != null)
        {
            self.Hide();
        }
    }
    /// <summary>
    /// ��������
    /// </summary>
    static UISatelliteSubstellar self = null;

    /// <summary>
    /// �����ʾ���µ㰴ť
    /// </summary>
    Button btn_Substellar = null;

    /// <summary>
    /// �ر����µ����
    /// </summary>
    Button btn_CloseSubstellar = null;


    /// <summary>
    /// ���µ����
    /// </summary>
    public GameObject substellarGame = null;

    Text txt_content = null;
    /// <summary>
    /// ��ȡ��������
    /// </summary>
    public static UISatelliteSubstellar Self
    {
        get
        {
            if (self == null)
            {
                self = new UISatelliteSubstellar();
                self.LoadGame();
            }
            else
            {
                if (self.JudgeGameObjDestory())
                {
                    self.sateParamList.Clear();
                    self.listParamInfo.Clear();
                    self.LoadGame();
                }
            }
            return self;
        }
        set { self = value; }
    }
    //==================================================================================//
    Transform tran = null;

    GameObject currSelectObj = null;

    /// <summary>
    /// ���ǲ�������Item
    /// </summary>
    GameObject param_Item = null;

    /// <summary>
    /// ���ǲ�������Content
    /// </summary>
    Transform SateContet = null;

    List<GameObject> sateParamList = new List<GameObject>();

    List<Text> listParamInfo = new List<Text>();
    /// <summary>
    /// ���µ���line
    /// </summary>
    public GameObject subStarPointLine = null;


    /// <summary>
    /// ��Ļ�Ŀ�
    /// </summary>
    private float ScreenWidth;

    /// <summary>
    /// ��Ļ�����
    /// </summary>
    float screenWidthdifference = 0.0f;
    /// <summary>
    /// ��Ļ�߱���
    /// </summary>
    float screenHeightdifference = 0.0f;

    /// <summary>
    ///���ʵ�ʵ�̫��ֱ��㾭γ��
    /// </summary>
    Vector3 lonLatPos;

    /// <summary>
    /// ������ʾ̫��ֱ����
    /// </summary>
     GameObject showObjImage;
    /// <summary>
    /// ������Ӱͼ
    /// </summary>
    RawImage shadow;

    IProperties pro;

    int timeLimit = 0;

    //==================================================================================//

    protected override void LoadGame()
    {
        formName = "Prefab/Prefab_UI/SatelliteParamInfo";
        base.LoadGame();
        //==================================================================================//    
        pro = TranSelf.GetComponent<IProperties>();

        substellarGame = pro.GetProperty("SubstellarPointTrajectory");
        btn_Substellar = pro.GetProperty("btn_Substellar").GetComponent<Button>();
        btn_CloseSubstellar = pro.GetProperty("btn_close").GetComponent<Button>();
        param_Item = pro.GetProperty("Item");
        SateContet = pro.GetProperty("Content").transform;
        subStarPointLine = GameObject.Find("UICanvas/Line");
        shadow = pro.GetProperty("shadow").GetComponent<RawImage>();
        showObjImage = pro.GetProperty("showObjImage");

        if (subStarPointLine!=null)
        {
            subStarPointLine.layer = 9;
            subStarPointLine.SetActive(true);
        }
        //��Ļ�ֱ�������
        ScreenWidth = PlayerPrefs.GetFloat("CanvasWidth", 0.0f);
        screenWidthdifference = PlayerPrefs.GetFloat("CanvasWidth", 0.0f) / 360;
        screenHeightdifference = PlayerPrefs.GetFloat("Canvasheight", 0.0f) / 180;
        //�л������µ�ģʽ
        btn_Substellar.onClick.AddListener(() => {
            Debug.Log("�л������µ�ģʽ");
            IocContainer_InstanceMgr.GetInstance().GetInstance<LoadQuests>().count = 1;
            IocContainer_InstanceMgr.GetInstance().GetInstance<UseDataEarthRotate>().IsEnterMinMapPanel = true;
            substellarGame.gameObject.SetActive(true);
            IocContainer_InstanceMgr.GetInstance().GetInstance<MainCameraOverallControl>().CanControl = false;
            Camera.main.transform.localPosition = new Vector3(14.7403f, -58.17964f, -221.3122f);
            Camera.main.transform.localEulerAngles = Vector3.zero;
            Camera.main.transform.parent.transform.localPosition = new Vector3(198.4f, 61f, -97f);
            if (subStarPointLine != null)
            {
                subStarPointLine.SetActive(true);
            }
            SetCanvasRenderMode(true);
            //�������ǣ��������Ƕ��˾��������б�
        });
        //�л�����ͨģʽ
        btn_CloseSubstellar.onClick.AddListener(() => {
            //IocContainer_InstanceMgr.GetInstance().GetInstance<LoadQuests>().count = 1;
            IocContainer_InstanceMgr.GetInstance().GetInstance<UseDataEarthRotate>().IsEnterMinMapPanel = false;
            IocContainer_InstanceMgr.GetInstance().GetInstance<MainCameraOverallControl>().CanControl = true;
            Camera.main.transform.parent.transform.localPosition = new Vector3(-16.91069f, 35.926f, -97f);
            substellarGame.gameObject.SetActive(false);

            //��ʾ����
            IocContainer_InstanceMgr.GetInstance().GetInstance<UseDataEarthRotate>().EarthObject.gameObject.SetActive(true);

            if (subStarPointLine!=null)
            {
                subStarPointLine.layer = 9;
                //subStarPointLine.SetActive(false);
            }
            SetCanvasRenderMode(false);
            //��ʾ���ǣ��������Ƕ��˾��������б�
            for (int i = 0; i < IocContainer_InstanceMgr.GetInstance().GetInstance<LoadQuests>().SatelliteTrans.Length; i++)
            {
                IocContainer_InstanceMgr.GetInstance().GetInstance<LoadQuests>().SatelliteTrans[i].gameObject.SetActive(true);
            }
            
        });

        #region �����ǲ����õ�
        SatelliteAssembly.GetInstance().AddSingleSateInfo("�ٶ�","");
        SatelliteAssembly.GetInstance().AddSingleSateInfo("�߶�", "");
        SatelliteAssembly.GetInstance().AddSingleSateInfo("λ��", "");
        SatelliteAssembly.GetInstance().AddSingleSateInfo("��γ��", "");
        SetSatelliteParamData();
        #endregion
    }

    /// <summary>
    /// ����������Ӱͼ���ƶ�
    /// </summary>
    /// <param name="lonLat"></param>
    public void CallShowShadow(Vector3 lonLat)
    {
        
        showObjImage.transform.localPosition = GetShowLonLatData(lonLat);
        MoveEarthShadow(showObjImage.transform.localPosition.x);
    }


    /// <summary>
    /// �ƶ���Ӱ��ͼ
    /// </summary>
    public void MoveEarthShadow(float x)
    {
        float a = (ScreenWidth / 2) + x;
        a = a * (-0.00026f);//�������ڼ���
        shadow.uvRect = new Rect(a, 0, 1, 1);
    }


    /// <summary>
    /// �õ���ʾ�ľ�γ������
    /// </summary>
    /// <returns></returns>
    public Vector3 GetShowLonLatData(Vector3 pos)
    {

        lonLatPos.x = (pos.x - 180) * screenWidthdifference;
        lonLatPos.y = pos.y * screenHeightdifference;
        return lonLatPos;
    }


    /// <summary>
    /// ���õĵ�ǰ���򳡾�Canvas��Ⱦģʽ
    /// </summary>
    void SetCanvasRenderMode(bool IsSubStarPoint)
    {
        Canvas can = GameObject.Find("Canvas").GetComponent<Canvas>();
        if (IsSubStarPoint)
        {
            can.renderMode = RenderMode.ScreenSpaceCamera;
        }
        else
            can.renderMode = RenderMode.ScreenSpaceOverlay;
    }


    public override bool IsVisible(GameObject obj, string str)
    {
        Show();
        txt_content.text = str;
        currSelectObj = obj;
        return base.IsVisible(obj, str);
    }


    /// <summary>
    /// �������ǵȲ�����ʾ
    /// </summary>
    public void SetSatelliteParamData()
    {
        int countIndex = 0;
        GameObject obj = null;
        Debug.Log("�������ǵȲ�����ʾ:"+ SatelliteAssembly.GetInstance().GetSateAssemblyInfo.Count);
        foreach (var item in SatelliteAssembly.GetInstance().GetSateAssemblyInfo)
        {
            if (countIndex < sateParamList.Count)
            {
               
                obj = sateParamList[countIndex];
            }
            else
            {
               
                obj = GameObject.Instantiate(param_Item) as GameObject;
                obj.transform.parent = SateContet;
                obj.transform.localScale = Vector3.one;
                sateParamList.Add(obj);
                listParamInfo.Add(obj.transform.GetChild(1).GetComponent<Text>());
            }
            countIndex++;
            obj.transform.GetChild(0).GetComponent<Text>().text = item.Key;
            obj.transform.GetChild(1).GetComponent<Text>().text = item.Value.ToString();
        }
    }


    /// <summary>
    /// ���ò�������Ϣ
    /// </summary>
    public void SetPostionSpeedParamInfo(string[] info)
    {
        for (int i = 0; i < info.Length; i++)
        {
            listParamInfo[i].text = info[i];
        }
    }


    public override bool JudgeGameObjDestory()
    {
        return base.JudgeGameObjDestory();
    }


    public override void Hide()
    {
        base.Hide();
    }
}
