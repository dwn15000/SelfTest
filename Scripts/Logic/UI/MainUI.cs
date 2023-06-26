using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class MainUI : UIBase
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
    public static MainUI self = null;

    /// <summary>
    /// ̫��ϵ
    /// </summary>
    Button btn_sun = null;

    /// <summary>
    /// ����
    /// </summary>
    Button btn_earth = null;

    /// <summary>
    /// ����װ��
    /// </summary>
    Button btn_Aircrash = null;

    Text txt_content = null;
    /// <summary>
    /// ��ȡ��������
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
    /// ��ǰ��Ҫ���صĳ�������
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
            Debug.Log("�л���ת��̫��ϵ������");
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
        Debug.Log("Scene2 loadedѡ����س���=" + scene_name);
        //SceneManager.LoadScene(scene_name);
      
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene_name);
        asyncLoad.completed += OnLoadCompleted;
    }
    private void OnLoadCompleted(AsyncOperation obj)
    {
        Debug.Log("���س����ɹ���......:");
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
    /// ��ʾ�ı�����
    /// </summary>
    public override void Show()
    {
        base.Show();
    }
}
