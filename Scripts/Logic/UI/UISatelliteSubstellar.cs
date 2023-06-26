using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UISatelliteSubstellar : UIBase
{
    /// <summary>
    /// 静态隐藏
    /// </summary>
    public static void StaticHide()
    {
        if (self != null)
        {
            self.Hide();
        }
    }
    /// <summary>
    /// 单件对象
    /// </summary>
    static UISatelliteSubstellar self = null;

    /// <summary>
    /// 点击显示星下点按钮
    /// </summary>
    Button btn_Substellar = null;

    /// <summary>
    /// 关闭星下点界面
    /// </summary>
    Button btn_CloseSubstellar = null;


    /// <summary>
    /// 星下点面板
    /// </summary>
    public GameObject substellarGame = null;

    Text txt_content = null;
    /// <summary>
    /// 获取单件对象
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
    /// 卫星部件参数Item
    /// </summary>
    GameObject param_Item = null;

    /// <summary>
    /// 卫星部件参数Content
    /// </summary>
    Transform SateContet = null;

    List<GameObject> sateParamList = new List<GameObject>();

    List<Text> listParamInfo = new List<Text>();
    /// <summary>
    /// 星下点线line
    /// </summary>
    public GameObject subStarPointLine = null;


    /// <summary>
    /// 屏幕的宽
    /// </summary>
    private float ScreenWidth;

    /// <summary>
    /// 屏幕宽比列
    /// </summary>
    float screenWidthdifference = 0.0f;
    /// <summary>
    /// 屏幕高比列
    /// </summary>
    float screenHeightdifference = 0.0f;

    /// <summary>
    ///获得实际的太阳直射点经纬度
    /// </summary>
    Vector3 lonLatPos;

    /// <summary>
    /// 用于显示太阳直射点的
    /// </summary>
     GameObject showObjImage;
    /// <summary>
    /// 日照阴影图
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
        //屏幕分辨率配置
        ScreenWidth = PlayerPrefs.GetFloat("CanvasWidth", 0.0f);
        screenWidthdifference = PlayerPrefs.GetFloat("CanvasWidth", 0.0f) / 360;
        screenHeightdifference = PlayerPrefs.GetFloat("Canvasheight", 0.0f) / 180;
        //切换到星下点模式
        btn_Substellar.onClick.AddListener(() => {
            Debug.Log("切换到星下点模式");
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
            //隐藏卫星（后面卫星多了就是卫星列表）
        });
        //切换到普通模式
        btn_CloseSubstellar.onClick.AddListener(() => {
            //IocContainer_InstanceMgr.GetInstance().GetInstance<LoadQuests>().count = 1;
            IocContainer_InstanceMgr.GetInstance().GetInstance<UseDataEarthRotate>().IsEnterMinMapPanel = false;
            IocContainer_InstanceMgr.GetInstance().GetInstance<MainCameraOverallControl>().CanControl = true;
            Camera.main.transform.parent.transform.localPosition = new Vector3(-16.91069f, 35.926f, -97f);
            substellarGame.gameObject.SetActive(false);

            //显示地球
            IocContainer_InstanceMgr.GetInstance().GetInstance<UseDataEarthRotate>().EarthObject.gameObject.SetActive(true);

            if (subStarPointLine!=null)
            {
                subStarPointLine.layer = 9;
                //subStarPointLine.SetActive(false);
            }
            SetCanvasRenderMode(false);
            //显示卫星（后面卫星多了就是卫星列表）
            for (int i = 0; i < IocContainer_InstanceMgr.GetInstance().GetInstance<LoadQuests>().SatelliteTrans.Length; i++)
            {
                IocContainer_InstanceMgr.GetInstance().GetInstance<LoadQuests>().SatelliteTrans[i].gameObject.SetActive(true);
            }
            
        });

        #region 以下是测试用的
        SatelliteAssembly.GetInstance().AddSingleSateInfo("速度","");
        SatelliteAssembly.GetInstance().AddSingleSateInfo("高度", "");
        SatelliteAssembly.GetInstance().AddSingleSateInfo("位置", "");
        SatelliteAssembly.GetInstance().AddSingleSateInfo("经纬度", "");
        SetSatelliteParamData();
        #endregion
    }

    /// <summary>
    /// 计算日照阴影图的移动
    /// </summary>
    /// <param name="lonLat"></param>
    public void CallShowShadow(Vector3 lonLat)
    {
        
        showObjImage.transform.localPosition = GetShowLonLatData(lonLat);
        MoveEarthShadow(showObjImage.transform.localPosition.x);
    }


    /// <summary>
    /// 移动阴影区图
    /// </summary>
    public void MoveEarthShadow(float x)
    {
        float a = (ScreenWidth / 2) + x;
        a = a * (-0.00026f);//最终用于计算
        shadow.uvRect = new Rect(a, 0, 1, 1);
    }


    /// <summary>
    /// 得到显示的经纬度数据
    /// </summary>
    /// <returns></returns>
    public Vector3 GetShowLonLatData(Vector3 pos)
    {

        lonLatPos.x = (pos.x - 180) * screenWidthdifference;
        lonLatPos.y = pos.y * screenHeightdifference;
        return lonLatPos;
    }


    /// <summary>
    /// 设置的当前地球场景Canvas渲染模式
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
    /// 设置卫星等参数显示
    /// </summary>
    public void SetSatelliteParamData()
    {
        int countIndex = 0;
        GameObject obj = null;
        Debug.Log("设置卫星等参数显示:"+ SatelliteAssembly.GetInstance().GetSateAssemblyInfo.Count);
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
    /// 设置参数等信息
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
