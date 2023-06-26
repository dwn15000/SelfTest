using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GlobalSliderCtrl
{
    private static GlobalSliderCtrl _instance = null;

    /// <summary>
    /// 得到单例
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
    /// 是否可以控制摄像机
    /// </summary>
    private bool isCanCtrlCamera = false;

    public bool GetIsCanCtrlCamera
    {
        get { return isCanCtrlCamera; }
        set { isCanCtrlCamera = value; }
    }
}
