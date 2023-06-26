using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseDataEarthRotate : MonoBehaviour
{
    [Tooltip("����GameObject")]
    public GameObject EarthObject;

    [Tooltip("������ĸ�����")]
    public GameObject MainCameraParent;

    /// <summary>
    /// ��ǰ���͸�ӵ�����
    /// </summary>
    public GameObject currClickPerspectiveSatellite;

    /// <summary>
    /// �Ƿ���������ӽǽ���
    /// </summary>
    public bool IsEnterMinMapPanel = false;

    /// <summary>
    /// ���ǵ�Pro���
    /// </summary>
    IProperties pro;

    Vector3 initPos;

    public Vector3 pointA; // A���Transform
    public Transform pointB; // Earth���Transform
    public Vector3 pointC; // C���Transform
    public Vector3 offset; // A�㵽Earth�������
    public Quaternion initialRotation; // Earth��ĳ�ʼ��ת�Ƕ�
    public Vector3 PB = new Vector3(0, 150, 0);
    // Start is called before the first frame update
    void Start()
    {
        // ����A�㵽B�������
        offset = PB - pointA;
        // ��¼B��ĳ�ʼ��ת�Ƕ�
        initialRotation = transform.rotation;
        initPos = pointB.position;
    }

    private void Awake()
    {
        IocContainer_InstanceMgr.GetInstance().RegistInstance(this);
    }

    /// <summary>
    /// �л������ӽ�
    /// </summary>
    /// <param name="isSatelliteView">�������ӽ�</param>
    public void SwitchSatelliteView(bool isSatelliteView)
    {
        IocContainer_InstanceMgr.GetInstance().GetInstance<UseDataEarthRotate>().IsEnterMinMapPanel = isSatelliteView;
        //EarthUIPanel.Self.ShowOrHideSlider(isSatelliteView);//��ʾ�������ص���ui�����slider
        ProcedureSetting.IsSatelliteView = isSatelliteView;
        if (isSatelliteView)  //������Ǻ󣬴��������ӽ�
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

            Debug.Log("���ε�ǰ�Ľű�");
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

            ///���������ӽ����ǵĸ��ֲ���
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
            Debug.Log("������Ǻ󣬵�ǰ���������ӽ�");
        }
        else  //�����ҳ��ť�󣬴�����ҳ�����ӽ�
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
    /// ���õ������ת
    /// </summary>
    public void SetEarthRotate(Vector3 pos,int count)
    {
        Vector3 newOffset = (initPos - pos);
        // ����B����Ҫ��ת�ĽǶ�
        Quaternion newRotation = Quaternion.FromToRotation(offset, newOffset) * new Quaternion(-1, -1, -1, -1);
        // ��תB��
        transform.rotation = (initialRotation * newRotation);
        initPos = pos;
        //Debug.Log("����B����Ҫ��ת�ĽǶ�111="+pos+ ",,,count="+ count);
        //Debug.Log("����B����Ҫ��ת�ĽǶ�111:" + offset + ",,,�Ƕ�:" + newOffset + ",,,,,newRotation:" + newRotation);
    }

}
