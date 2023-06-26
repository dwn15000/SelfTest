using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelData
{
    private static ModelData _instance = null;

    /// <summary>
    /// 自己的用户Id
    /// </summary>
    public int userId = 16;
    public static ModelData GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ModelData();
            IocContainer_InstanceMgr.GetInstance().RegistInstance(_instance);
        }
        return _instance;
    }

    private Dictionary<string, ModeGameObjectParam> m_modeParamDic = new Dictionary<string, ModeGameObjectParam>();

    public void AddModeParamData(string name, ModeGameObjectParam model)
    {
        if (!m_modeParamDic.ContainsKey(name))
        {
            m_modeParamDic.Add(name, model);
        }
        else
            m_modeParamDic[name] =model;
    }

    public Dictionary<string, ModeGameObjectParam> GetModelParamDic
    {
        get { return m_modeParamDic; }
        set { m_modeParamDic = value; }
    }

    /// <summary>
    /// 所有的操作模型列表
    /// </summary>
    private List<GameObject> all_OperateGameObj = new List<GameObject>();

    public List<GameObject> GetAllOperateGameObj
    {
        get { return all_OperateGameObj; }
        set { all_OperateGameObj = value; }
    }


    public void AddOperateGameObj(GameObject obj)
    {
        if (!all_OperateGameObj.Contains(obj))
        {
            all_OperateGameObj.Add(obj);
        }
       
    }

    /// <summary>
    /// 移除操作的模型组件obj
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveOperateGameObj(GameObject obj)
    {
        if (all_OperateGameObj.Contains(obj))
        {
            all_OperateGameObj.Remove(obj);
        }
    }
}


/// <summary>
/// 模型相关属性信息
/// </summary>
public class ModeGameObjectParam
{
    public string name;
    public GameObject obj;
    public Vector3  old_pos;
    public Vector3 new_pos;
    public ModeGameObjectParam(string name ,GameObject obj,Vector3 old_pos,Vector3 new_pos)
    {
        this.name = name;
        this.obj = obj;
        this.old_pos = old_pos;
        this.new_pos = new_pos;
    }
}

public class SelectModelAttribute
{
    private static SelectModelAttribute _instance = null;
    public static SelectModelAttribute GetInstance()
    {
        if (_instance==null)
        {
            _instance = new SelectModelAttribute();
        }
        return _instance;
    }

    /// <summary>
    /// 当前拆分的级别
    /// </summary>
    private int child_Level = 0;

    
    private GameObject curr_obj;
    /// <summary>
    /// 是否为父级
    /// </summary>
    private bool is_Parent;

    /// <summary>
    /// 自己的父物体
    /// </summary>
    private GameObject parent_Obj;
    /// <summary>
    /// 当前操作的飞行器模型
    /// </summary>
    public GameObject GetCurrObj
    {

        set { curr_obj = value; }
        get { return curr_obj; }
    }
    /// <summary>
    /// 当前操作的飞行器模型
    /// </summary>
    public bool GetIsHaveParent
    {

        set { is_Parent = value; }
        get { return is_Parent; }
    }

    /// <summary>
    /// 自己的父物体
    /// </summary>
    public GameObject GetParentGameObj
    {

        set { parent_Obj = value; }
        get { return parent_Obj; }
    }
    /// <summary>
    /// 当前操作的级别
    /// </summary>
    public int GetOperateLevel
    {

        set { child_Level = value; }
        get { return child_Level; }
    }
}