using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseDataEarthRotate : MonoBehaviour
{
    [Tooltip("地球GameObject")]
    public GameObject EarthObject;

    [Tooltip("主相机的父物体")]
    public GameObject MainCameraParent;

    /// <summary>
    /// 当前点击透视的卫星
    /// </summary>
    public GameObject currClickPerspectiveSatellite;

    /// <summary>
    /// 是否进入卫星视角界面
    /// </summary>
    public bool IsEnterMinMapPanel = false;

    /// <summary>
    /// 卫星的Pro组件
    /// </summary>
    IProperties pro;

    Vector3 initPos;

    public Vector3 pointA; // A点的Transform
    public Transform pointB; // Earth点的Transform
    public Vector3 pointC; // C点的Transform
    public Vector3 offset; // A点到Earth点的向量
    public Quaternion initialRotation; // Earth点的初始旋转角度
    public Vector3 PB = new Vector3(0, 150, 0);
    // Start is called before the first frame update
    void Start()
    {
        // 计算A点到B点的向量
        offset = PB - pointA;
        // 记录B点的初始旋转角度
        initialRotation = transform.rotation;
        initPos = pointB.position;
    }

    private void Awake()
    {
        IocContainer_InstanceMgr.GetInstance().RegistInstance(this);
    }

    /// <summary>
    /// 切换卫星视角
    /// </summary>
    /// <param name="isSatelliteView">是卫星视角</param>
    public void SwitchSatelliteView(bool isSatelliteView)
    {
        IocContainer_InstanceMgr.GetInstance().GetInstance<UseDataEarthRotate>().IsEnterMinMapPanel = isSatelliteView;
        //EarthUIPanel.Self.ShowOrHideSlider(isSatelliteView);//显示或者隐藏地球ui界面的slider
        ProcedureSetting.IsSatelliteView = isSatelliteView;
        if (isSatelliteView)  //点击卫星后，处于卫星视角
        {

            IocContainer_InstanceMgr.GetInstance().GetInstance<LoadQuests>().CompareTime = 0.01f;
           
            if (Camera.main.gameObject.GetComponent<MainCameraOverallControl>()!=null)
            {
                Camera.main.gameObject.GetComponent<MainCameraOverallControl>().enabled = false;
            }
            if (Camera.main.gameObject.GetComponent<Rotate>() != null)
            {
                Camera.main.gameObject.GetComponent<Rotate>().enabled = true;
            }        

            Debug.Log("屏蔽当前的脚本");
            //IocContainer_InstanceMgr.GetInstance().GetInstance<WhenObjIsSpinningRotateAndScaleObjWithMouse>().enabled = false;

            //if (EarthObject.GetComponent<WhenObjIsSpinningRotateAndScaleObjWithMouse>() != null)
            //{
            //    EarthObject.GetComponent<WhenObjIsSpinningRotateAndScaleObjWithMouse>().enabled = false;
            //}
            EarthObject.transform.position = new Vector3(0, 105, 0);
            EarthObject.transform.localEulerAngles = new Vector3(74, 90, -90);
            EarthObject.transform.localScale = Vector3.one * 10;

            MainCameraParent.transform.position = new Vector3(39, 684.9f, -115.7f);
            MainCameraParent.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            MainCameraParent.transform.GetChild(0).transform.localPosition = Vector3.zero;
            MainCameraParent.transform.GetChild(0).transform.localEulerAngles = new Vector3(32.799f, 213.496f, 7.108f);

            ///设置卫星视角卫星的各种参数
            if (currClickPerspectiveSatellite != null)
            {
                pro = currClickPerspectiveSatellite.GetComponent<IProperties>();

                pro.GetProperty("Name").SetActive(false);
                currClickPerspectiveSatellite.transform.localScale = Vector3.one;
                currClickPerspectiveSatellite.transform.position = new Vector3(-11, 602f, -196.4f);
                currClickPerspectiveSatellite.transform.localEulerAngles = new Vector3(-108.39f, -8.190002f, -83f);
                if (currClickPerspectiveSatellite.GetComponent<ScaleObject>() != null)
                {
                    currClickPerspectiveSatellite.GetComponent<ScaleObject>().oldPos = new Vector3(-11f, 602, -196.4f);
                }
            }
            Debug.Log("点击卫星后，当前处于卫星视角");
        }
        else  //点击主页按钮后，处于主页地球视角
        {
            if (Camera.main.gameObject.GetComponent<MainCameraOverallControl>() != null)
            {
                Camera.main.gameObject.GetComponent<MainCameraOverallControl>().enabled = true;
            }
            if (Camera.main.gameObject.GetComponent<Rotate>() != null)
            {
                Camera.main.gameObject.GetComponent<Rotate>().enabled = false;
            }
            IocContainer_InstanceMgr.GetInstance().GetInstance<LoadQuests>().currentPoint = 0;
            IocContainer_InstanceMgr.GetInstance().GetInstance<LoadQuests>().count = 1;
            IocContainer_InstanceMgr.GetInstance().GetInstance<LoadQuests>().CompareTime = 0.01f;

            //IocContainer_InstanceMgr.GetInstance().GetInstance<WhenObjIsSpinningRotateAndScaleObjWithMouse>().enabled = true;

            EarthObject.transform.position = new Vector3(0, 0, 0);
            EarthObject.transform.localEulerAngles = new Vector3(0, 90, 0);
            EarthObject.transform.localScale = Vector3.one;

            MainCameraParent.transform.position = new Vector3(-16.91069f, 62.9f, -0.3f);
            MainCameraParent.transform.localEulerAngles = Vector3.zero;
            MainCameraParent.transform.localScale = Vector3.one;
            MainCameraParent.transform.GetChild(0).transform.localPosition = new Vector3(14.7403f, -58.17964f,-221.3122f);
            MainCameraParent.transform.GetChild(0).transform.localEulerAngles = new Vector3(0f,0f, 0f);
            MainCameraParent.transform.GetChild(0).transform.localScale = Vector3.one;
            if (MainCameraParent.transform.GetChild(0).GetComponent<Rotate>()!=null)
            {
                MainCameraParent.transform.GetChild(0).GetComponent<Rotate>().enabled = false;
            }
        }
    }

    /// <summary>
    /// 设置地球的旋转
    /// </summary>
    public void SetEarthRotate(Vector3 pos,int count)
    {
        Vector3 newOffset = (initPos - pos);
        // 计算B点需要旋转的角度
        Quaternion newRotation = Quaternion.FromToRotation(offset, newOffset) * new Quaternion(-1, -1, -1, -1);
        // 旋转B点
        transform.rotation = (initialRotation * newRotation);
        initPos = pos;
        //Debug.Log("计算B点需要旋转的角度111="+pos+ ",,,count="+ count);
        //Debug.Log("计算B点需要旋转的角度111:" + offset + ",,,角度:" + newOffset + ",,,,,newRotation:" + newRotation);
    }

}
