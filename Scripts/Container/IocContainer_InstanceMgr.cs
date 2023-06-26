using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// IOC容器之单例管理器
/// </summary>
public class IocContainer_InstanceMgr
{
    public static IocContainer_InstanceMgr _instance = null;
    public Dictionary<Type, object> m_instance = new Dictionary<Type, object>();

    /// <summary>
    /// 得到单例
    /// </summary>
    /// <returns>单例</returns>
    public static IocContainer_InstanceMgr GetInstance()
    {
        if (_instance == null)
        {
            _instance = new IocContainer_InstanceMgr();
        }
        return _instance;
    }

    /// <summary>
    /// 移除单例
    /// </summary>
    /// <typeparam name="T">类型T</typeparam>
    public void RemoveInstance<T>()
    {
        var type = typeof(T);
        if (m_instance.ContainsKey(type))
        {
            m_instance.Remove(type);
        }
    }

    /// <summary>
    /// 注册单例
    /// </summary>
    /// <typeparam name="T">类型T</typeparam>
    /// <param name="instance">类型T的单例</param>
    public void RegistInstance<T>(T instance)
    {
        var type = typeof(T);
        if (m_instance.ContainsKey(type))
        {
            m_instance[type] = instance;
        }
        else
        {
            m_instance.Add(type, instance);
        }
    }

    /// <summary>
    /// 获得单例
    /// </summary>
    /// <typeparam name="T">类型T</typeparam>
    /// <returns>类型T</returns>
    public T GetInstance<T>() where T : class
    {
        var type = typeof(T);
        object obj = null;
        if (m_instance.TryGetValue(type, out obj))
        {
            return obj as T;
        }
        return null;
    }
}
