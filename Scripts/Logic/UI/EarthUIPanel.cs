using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EarthUIPanel : UIBase
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
    static EarthUIPanel self = null;

    /// <summary>
    /// 地球场景主页
    /// </summary>
    Button btn_home = null;

    /// <summary>
    /// 网格
    /// </summary>
    Button btn_grid = null;

    /// <summary>
    /// 链子
    /// </summary>
    Button btn_chain = null;

    /// <summary>
    /// 骨骼
    /// </summary>
    Button btn_bones = null;

    /// <summary>
    /// 图像视图
    /// </summary>
    Button btn_imageView = null;

    /// <summary>
    /// wifi
    /// </summary>
    Button btn_wifi = null;

    /// <summary>
    /// 飞行器
    /// </summary>
    TMP_Dropdown drop_airVehicle = null;

    /// <summary>
    /// 音乐
    /// </summary>
    Button btn_music = null;

    /// <summary>
    /// 控制卫星快慢
    /// </summary>
    Slider slider_SateSpeed = null;


    /// <summary>
    /// 控制卫星大小
    /// </summary>
    Scrollbar slider_sateSize = null;


    /// <summary>
    /// 显示速度时间
    /// </summary>
    Text txt_sexTime = null;

    /// <summary>
    /// 实时时间按钮
    /// </summary>
    Button btn_RealTime = null;

    /// <summary>
    /// 太阳光
    /// </summary>
    Button btn_sunlight = null;

    /// <summary>
    /// 系统
    /// </summary>
    Button btn_window = null;

    /// <summary>
    /// 退出
    /// </summary>
    Button btn_exit = null;

    /// <summary>
    /// 卫星大小增加
    /// </summary>
    Button btn_sateAddSize = null;

    /// <summary>
    /// 卫星大小减小
    /// </summary>
    Button btn_sateReduceSize = null;


    Text txt_content = null;

    /// <summary>
    /// 卫星名字集合
    /// </summary>
    List<string> SatelliteNameList = new List<string>();

    /// <summary>
    /// 卫星速度显示(mins/sec)
    /// </summary>
    TextMeshProUGUI txt_SpeedNumber = null;
    /// <summary>
    /// 获取单件对象
    /// </summary>
    public static EarthUIPanel Self
    {
        get
        {
            if (self == null)
            {
                self = new EarthUIPanel();
                self.LoadGame();
            }
            else
            {
                if (self.JudgeGameObjDestory())
                {
                    self.LoadGame();
                }
            }
            return self;
        }
    }
    //==================================================================================//
    Transform tran = null;

    int timeLimit = 0;

    IProperties pro;
    //==================================================================================//

    protected override void LoadGame()
    {
        formName = "Prefab/Prefab_UI/EarthUIPanel";
        base.LoadGame();
        //==================================================================================//       
        pro = TranSelf.GetComponent<IProperties>();

        btn_home = pro.GetProperty("btn_home").GetComponent<Button>();
        btn_grid = pro.GetProperty("btn_grid").GetComponent<Button>();
        btn_chain = pro.GetProperty("btn_chain").GetComponent<Button>();
        btn_bones = pro.GetProperty("btn_bones").GetComponent<Button>();
        btn_imageView = pro.GetProperty("btn_imageView").GetComponent<Button>();
        btn_wifi = pro.GetProperty("btn_wifi").GetComponent<Button>();
        btn_music = pro.GetProperty("btn_music").GetComponent<Button>();
        btn_exit = pro.GetProperty("btn_exit").GetComponent<Button>();

        btn_RealTime = pro.GetProperty("btn_RealTime").GetComponent<Button>();
        btn_sunlight = pro.GetProperty("btn_sunlight").GetComponent<Button>();
        btn_window = pro.GetProperty("btn_window").GetComponent<Button>();

        btn_sateAddSize = pro.GetProperty("btn_sateAddSize").GetComponent<Button>();
        btn_sateReduceSize = pro.GetProperty("btn_sateReduceSize").GetComponent<Button>();

        drop_airVehicle = pro.GetProperty("drop_airVehicle").GetComponent<TMP_Dropdown>();
        slider_SateSpeed = pro.GetProperty("slider_SateSpeed").GetComponent<Slider>();
        slider_sateSize = pro.GetProperty("slider_sateSize").GetComponent<Scrollbar>();

        Debug.Log("选择卫星数据-drop_airVehicle=" + drop_airVehicle);
        txt_SpeedNumber = pro.GetProperty("SpeedNumber").GetComponent<TextMeshProUGUI>();
        btn_home.onClick.AddListener(() => { 
            Debug.Log("回到地球视角主页");
            IocContainer_InstanceMgr.GetInstance().GetInstance<UseDataEarthRotate>().SwitchSatelliteView(false);
            IocContainer_InstanceMgr.GetInstance().GetInstance<LoadQuests>().ShowSingleSatellite(-1,true);
            foreach (var item in GlobalSatelliteCtrl.GetInstance().GetAllSatelliteName)
            {
                item.isClickZoom = false;
                item.text_Name.text = item.satelliteName;
            }
        });
        btn_grid.onClick.AddListener(() => { Debug.Log("网格"); });
        btn_chain.onClick.AddListener(() => { Debug.Log("链子"); });
        btn_bones.onClick.AddListener(() => { Debug.Log("骨骼"); });
        btn_imageView.onClick.AddListener(() => { 
            Debug.Log("切换到星下点模式");
            //IocContainer_InstanceMgr.GetInstance().GetInstance<LoadQuests>().count = 1;
            //IocContainer_InstanceMgr.GetInstance().GetInstance<UseDataEarthRotate>().IsEnterMinMapPanel = true;
            UISatelliteSubstellar.Self.substellarGame.gameObject.SetActive(true);
            IocContainer_InstanceMgr.GetInstance().GetInstance<MainCameraOverallControl>().CanControl = false;
            Camera.main.transform.localPosition = new Vector3(14.7403f, -58.17964f, -221.3122f);
            Camera.main.transform.localEulerAngles = Vector3.zero;
            Camera.main.transform.parent.transform.localPosition = new Vector3(188f, 61f, 32f);

            //隐藏地球
            IocContainer_InstanceMgr.GetInstance().GetInstance<UseDataEarthRotate>().EarthObject.gameObject.SetActive(false);

            if (UISatelliteSubstellar.Self.subStarPointLine != null)
            {
                UISatelliteSubstellar.Self.subStarPointLine.layer = 5;
                //UISatelliteSubstellar.Self.subStarPointLine.SetActive(true);
            }
            SetCanvasRenderMode(true);
            //隐藏卫星（后面卫星多了就是卫星列表）
            for (int i = 0; i < IocContainer_InstanceMgr.GetInstance().GetInstance<LoadQuests>().SatelliteTrans.Length; i++)
            {
                IocContainer_InstanceMgr.GetInstance().GetInstance<LoadQuests>().SatelliteTrans[i].gameObject.SetActive(false);
            }
        }
        );
        btn_wifi.onClick.AddListener(() => { Debug.Log("wifi"); });
        btn_music.onClick.AddListener(() => { Debug.Log("音乐"); });
        btn_exit.onClick.AddListener(() => { 
            Debug.Log("退出");
            Hide();
            UISatelliteSubstellar.Self.Hide();
            UISceceLoadAni.Self.Show();
            MainUI.Self.LoadScene2("Main");           
        });

        btn_RealTime.onClick.AddListener(() => { Debug.Log("实时时间"); });
        btn_sunlight.onClick.AddListener(() => { Debug.Log("太阳光"); });
        btn_window.onClick.AddListener(() => { Debug.Log("系统"); });

        btn_sateAddSize.onClick.AddListener(() => { Debug.Log("增加卫星大小"); });
        btn_sateReduceSize.onClick.AddListener(() => { Debug.Log("减小卫星大小"); });

        drop_airVehicle.onValueChanged.AddListener((int idx)=> {
            Debug.Log("选的第几个卫星:" + idx);
            SatelliteNameAndPath sNamePath = SatelliteData.GetInstance().GetSateInfo[idx];
            if (sNamePath != null)
            {
                Debug.Log("获取到了数据:" + sNamePath.satelliteName);
            }
        });
        slider_SateSpeed.onValueChanged.AddListener((float value)=> {
            IocContainer_InstanceMgr.GetInstance().GetInstance<LoadQuests>().FastSpeed = Mathf.FloorToInt(value);
            txt_SpeedNumber.text = Mathf.FloorToInt(value) + " mins/sec";
        });

        slider_sateSize.onValueChanged.AddListener((float value) => {
           Debug.Log("当前卫星大小值:" + value);
        });
       
    }


    /// <summary>
    /// 添加下来卫星名字数据
    /// </summary>
    public void AddSatelliteDropOptions(string SaName)
    {
        SatelliteNameList.Add(SaName);
        drop_airVehicle.AddOptions(SatelliteNameList);
    }



    /// <summary>
    /// 显示或者隐藏Slider
    /// </summary>
    public void ShowOrHideSlider(bool is_show)
    {
        slider_SateSpeed.gameObject.SetActive(!is_show);
        slider_sateSize.gameObject.SetActive(!is_show);
    }


    public override bool JudgeGameObjDestory()
    {
        return base.JudgeGameObjDestory();
    }


    public void SetCanvasRenderMode(bool IsSubStarPoint)
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
        return base.IsVisible(obj, str);
    }


    public override void Hide()
    {
        base.Hide();
    }

}
