using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GlobalSliderCtrl
{
    private static GlobalSliderCtrl _instance = null;

    /// <summary>
    /// �õ�����
    /// </summary>
    /// <returns></returns>
    public static GlobalSliderCtrl GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GlobalSliderCtrl();
        }

        return _instance;
    }

    /// <summary>
    /// �Ƿ���Կ��������
    /// </summary>
    private bool isCanCtrlCamera = false;

    public bool GetIsCanCtrlCamera
    {
        get { return isCanCtrlCamera; }
        set { isCanCtrlCamera = value; }
    }
}
