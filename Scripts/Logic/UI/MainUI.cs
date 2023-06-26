using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class MainUI : UIBase
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
    public static MainUI self = null;

    /// <summary>
    /// 太阳系
    /// </summary>
    Button btn_sun = null;

    /// <summary>
    /// 地球
    /// </summary>
    Button btn_earth = null;

    /// <summary>
    /// 虚拟装配
    /// </summary>
    Button btn_Aircrash = null;

    Text txt_content = null;
    /// <summary>
    /// 获取单件对象
    /// </summary>
    public static MainUI Self
    {
        get
        {
            if (self == null)
            {
                self = new MainUI();
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
        set { self = value; }
    }
    //==================================================================================//
    Transform tran = null;

    GameObject currSelectObj = null;

    int timeLimit = 0;

    /// <summary>
    /// 当前需要加载的场景名字
    /// </summary>
    string CurrSceneName = "";
    //==================================================================================//


   
    protected override void LoadGame()
    {
        formName = "Prefab/Prefab_UI/MainUIPanel";
        base.LoadGame();
        //==================================================================================//
        btn_sun = TranSelf.Find("Sun/StartButton").gameObject.GetComponent<Button>();
        btn_sun.onClick.AddListener(() => { LoadScene2("SolarSystem");
            Debug.Log("切换跳转到太阳系场景了");
        });

        btn_earth = TranSelf.Find("Earth/StartButton").gameObject.GetComponent<Button>();
        btn_earth.onClick.AddListener(() => { LoadScene2("EarthView"); });

        btn_Aircrash = TranSelf.Find("Aircrash/StartButton").gameObject.GetComponent<Button>();
        btn_Aircrash.onClick.AddListener(() => { LoadScene2(""); });
    }

    public void LoadScene2(string scene_name)
    {
        UISceceLoadAni.Self.Show();
        CurrSceneName = scene_name;
        Debug.Log("Scene2 loaded选择加载场景=" + scene_name);
        //SceneManager.LoadScene(scene_name);
      
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene_name);
        asyncLoad.completed += OnLoadCompleted;
    }
    private void OnLoadCompleted(AsyncOperation obj)
    {
        Debug.Log("加载场景成功的......:");
        //Hide();
        if (CurrSceneName == "EarthView")
        {
            EarthUIPanel.Self.Show();
            UISatelliteSubstellar.Self.Show();
        }
        UISceceLoadAni.Self.Hide();
    }


   

    public override void Hide()
    {
        if (!JudgeGameObjDestory())
        {
            base.Hide();
        }
        
    }
    /// <summary>
    /// 显示文本内容
    /// </summary>
    public override void Show()
    {
        base.Show();
    }
}
