using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainCtrl : MonoBehaviour
{
    /// <summary>
    /// ȫ��Canvas
    /// </summary>
    public Canvas GlobalSceneCanvas;
    private void Awake()
    {
        IocContainer_InstanceMgr.GetInstance().RegistInstance(this);
        GlobalSceneCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        //GameObject.DontDestroyOnLoad(GameObject.Find("Canvas"));
        ///�����������UI
        ///deb
        Debug.Log("MainUI="+ MainUI.self);
        MainUI.Self.Show();    

        #region ����ת��ʱ���
        //DateTime t = CalculateUTCTime(2433328.5);
        //Debug.Log(string.Format("{0}�� {1}�� {2}�� {3}ʱ {4}�� {5}��", t.Year,t.Month,t.Day,t.Hour,t.Minute,t.Second)) ;
        #endregion
    }


    public DateTime CalculateUTCTime(double julianDate)
    {
        double totalSeconds = (julianDate - 2440587.5) * 86400d;
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(totalSeconds);
        return dateTime;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
