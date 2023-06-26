using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase
{
    /// <summary>
    /// ui��������
    /// </summary>
    public string formName = "";
    /// <summary>
    /// ���ڵ�
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
    /// ui����gameobject
    /// </summary>
    public GameObject ObjSelf = null;
    /// <summary>
    /// ui����transform
    /// </summary>
    public Transform TranSelf = null;
  

    /// <summary>
    /// ����UI������б�
    /// </summary>
    private static List<UIBase> allUIList = new List<UIBase>();

    /// <summary>
    /// ����ui���
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
    /// ��ʼ��panel �б�
    /// </summary>
    protected void InitPanelList()
    {

    }

   
    /// <summary>
    /// ���е�ui�Ĳ��
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
    /// �ɼ���
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
    /// ��ʾ
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
    /// ����
    /// </summary>
    public virtual void Hide()
    {
        if (!JudgeGameObjDestory())
        {
            ObjSelf.SetActive(false);
        }
        
    }

}
