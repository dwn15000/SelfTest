using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase
{
    /// <summary>
    /// ui面板的名称
    /// </summary>
    public string formName = "";
    /// <summary>
    /// 父节点
    /// </summary>
    private static Transform uiRoot = null;

    public static Transform UiRoot
    {
        get
        {
            if (uiRoot == null)
            {
                uiRoot = GameObject.Find("UI Root/Camera").transform;
            }
            return UIBase.uiRoot;

        }
    }
    /// <summary>
    /// ui面板的gameobject
    /// </summary>
    public GameObject ObjSelf = null;
    /// <summary>
    /// ui面板的transform
    /// </summary>
    public Transform TranSelf = null;
  

    /// <summary>
    /// 所有UI对象的列表
    /// </summary>
    private static List<UIBase> allUIList = new List<UIBase>();

    /// <summary>
    /// 加载ui面板
    /// </summary>
    /// <param name="obj"></param>
    protected virtual void LoadGame()
    {
        ObjSelf = ResHelper.GetInstance().LoadUI(formName);
        TranSelf = ObjSelf.transform;
        InitPanelList();
        allUIList.Add(this);
    }


    public virtual bool JudgeGameObjDestory()
    {
        if (ObjSelf == null || !ObjSelf)
        {
            Debug.Log("Target object has been destroyed!");
            return true;
        }
        return false;
    }
    /// <summary>
    /// 初始化panel 列表
    /// </summary>
    protected void InitPanelList()
    {

    }

   
    /// <summary>
    /// 所有的ui的层次
    /// </summary>
    protected void SetDepth()
    {

    }

    public static void HideAllUI()
    {
        allUIList.ForEach((ui) =>
        {
            if (ui != null && ui.ObjSelf != null && ui.ObjSelf.activeSelf)
            {
                ui.Hide();
            }
        });
    }

    /// <summary>
    /// 可见性
    /// </summary>
    public bool Visible
    {
        get
        {
            //Debug.Log("uibase");
            if (ObjSelf != null)
                return ObjSelf.activeSelf;
            return false;
        }
    }


    public virtual bool IsVisible(GameObject obj, string str)
    {
        if (ObjSelf != null)
            return ObjSelf.activeSelf;
        return false;   
    }
    /// <summary>
    /// 显示
    /// </summary>
    public virtual void Show()
    {
        if (!JudgeGameObjDestory())
        {
            ObjSelf.SetActive(true);
        }
        else
        {
            LoadGame();
        }
    }

    /// <summary>
    /// 隐藏
    /// </summary>
    public virtual void Hide()
    {
        if (!JudgeGameObjDestory())
        {
            ObjSelf.SetActive(false);
        }
        
    }

}
