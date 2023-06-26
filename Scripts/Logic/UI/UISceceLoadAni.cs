using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UISceceLoadAni : UIBase
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
    static UISceceLoadAni self = null;

   
    /// <summary>
    /// 获取单件对象
    /// </summary>
    public static UISceceLoadAni Self
    {
        get
        {
            if (self == null)
            {
                self = new UISceceLoadAni();
                self.LoadGame();
            }
            return self;
        }
        set { self = value; }
    }
    //==================================================================================//
    Transform tran = null;


    int timeLimit = 0;

    //==================================================================================//

    protected override void LoadGame()
    {
        formName = "Prefab/Prefab_UI/SceceLoadAni";
        base.LoadGame();
        //==================================================================================//                 
    }

    public override void Hide()
    {
        base.Hide();
    }

    public override void Show()
    {
        base.Show();
    }
}
