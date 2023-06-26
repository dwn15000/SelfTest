using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// IOC����֮����������
/// </summary>
public class IocContainer_InstanceMgr
{
    public static IocContainer_InstanceMgr _instance = null;
    public Dictionary<Type, object> m_instance = new Dictionary<Type, object>();

    /// <summary>
    /// �õ�����
    /// </summary>
    /// <returns>����</returns>
    public static IocContainer_InstanceMgr GetInstance()
    {
        if (_instance == null)
        {
            _instance = new IocContainer_InstanceMgr();
        }
        return _instance;
    }

    /// <summary>
    /// �Ƴ�����
    /// </summary>
    /// <typeparam name="T">����T</typeparam>
    public void RemoveInstance<T>()
    {
        var type = typeof(T);
        if (m_instance.ContainsKey(type))
        {
            m_instance.Remove(type);
        }
    }

    /// <summary>
    /// ע�ᵥ��
    /// </summary>
    /// <typeparam name="T">����T</typeparam>
    /// <param name="instance">����T�ĵ���</param>
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
    /// ��õ���
    /// </summary>
    /// <typeparam name="T">����T</typeparam>
    /// <returns>����T</returns>
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
