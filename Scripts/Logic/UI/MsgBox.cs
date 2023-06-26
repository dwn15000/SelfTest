using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MsgBox : UIBase
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
    static MsgBox self = null;

    Button btn_enterchild = null;


    Text txt_content = null;
    /// <summary>
    /// 获取单件对象
    /// </summary>
    public static MsgBox Self
    {
        get
        {
            if (self == null)
            {
                self = new MsgBox();
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

    GameObject currSelectObj = null;

    int timeLimit = 0;

    IProperties pro;
    //==================================================================================//

    protected override void LoadGame()
    {
        formName = "Prefab/Prefab_UI/xxx";
        base.LoadGame();
        //==================================================================================//
        pro = TranSelf.GetComponent<IProperties>();

        btn_enterchild = pro.GetProperty("btn_enterchild").GetComponent<Button>();
        btn_enterchild.onClick.AddListener(()=> { });
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
