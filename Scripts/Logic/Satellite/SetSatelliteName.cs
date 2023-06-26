using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SetSatelliteName : MonoBehaviour
{

    /// <summary>
    /// �������ֵ�TextMeshPro
    /// </summary>
    public TextMeshPro text_Name;

    /// <summary>
    /// �и����ǵ�����
    /// </summary>
    string[] splitSatelliteName;

    /// <summary>
    /// ���Ǳ��
    /// </summary>
    private int SatelliteNumber = -1;
    /// <summary>
    /// ��������
    /// </summary>
    [HideInInspector]
    public string satelliteName = "";
    /// <summary>
    /// �Ƿ�����������ʾģʽ
    /// </summary>
    [HideInInspector]
    public bool isClickZoom = false;

    /// <summary>
    /// ��ת���ٶ����
    /// </summary>
    public ModelRotate[] CMGRotateArr = new ModelRotate[6];

    /// <summary>
    /// ���ת�����
    /// </summary>
    public CMGRotate[] CMGAngArr = new CMGRotate[6];

    /// <summary>
    /// ����͸�ӵ�����
    /// </summary>
    List<GameObject> CanperspectiveList = new List<GameObject>();


    /// <summary>
    ///���ǷŴ���С�����
    /// </summary>
    private ScaleObject sateScale = null;
    /// <summary>
    /// ��ʾ�������ص�͸������
    /// </summary>
    public void HideOrShowPerspectiveObj(bool is_show)
    {
        foreach (var item in CanperspectiveList)
        {
            item.GetComponent<MeshRenderer>().enabled = is_show;
        }
    }

    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        GlobalSatelliteCtrl.GetInstance().AddSatelliteName(this);
        text_Name = this.gameObject.GetComponent<TextMeshPro>();
        IocContainer_InstanceMgr.GetInstance().RegistInstance(this);
        satelliteName = text_Name.text;
        Traverse(this.transform.parent.gameObject,false);
        EarthUIPanel.Self.AddSatelliteDropOptions(satelliteName);
    }


    int cmgAngIndex = 0;
    int cmgRateIndex = 0;
    string[] ModelData;

    /// <summary>
    /// ��Ѱ��ģ���������
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="is_show"></param>
    void Traverse(GameObject obj, bool is_show)
    {
        foreach (var item in IocContainer_InstanceMgr.GetInstance().GetInstance<LoadFileConfig>().allModelRotateName)
        {
            ModelData = item.Split(',');
            if (obj.name == ModelData[0])
            {
                if (int.Parse(ModelData[1]) == 2)
                {
                    if (int.Parse(ModelData[2]) > 100)
                    {
                        ModelRotate mr = obj.AddComponent<ModelRotate>();
                        mr.Rotate = Vector3.right;
                       CMGRotateArr[cmgRateIndex] = mr;
                        cmgRateIndex++;//��ת���ٶ�
                    }
                    else
                    {
                        CMGRotate cr = obj.AddComponent<CMGRotate>();
                        cr.DirIndex = int.Parse(ModelData[2]);
                        CMGAngArr[cmgAngIndex] = cr;
                        cmgAngIndex++;//���
                    }
                }
                else if (int.Parse(ModelData[1]) == 10000)
                {
                    CanperspectiveList.Add(obj);
                }
                continue;
            }

        }


        foreach (Transform child in obj.transform)
        {
            Traverse(child.gameObject, is_show);
        }
    }



    private void LateUpdate()
    {
        this.gameObject.transform.rotation = Camera.main.transform.rotation;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                Debug.Log("Hit cube-is=" + hitInfo.collider);
                if (hitInfo.collider.CompareTag("Satellite"))
                {
                    if (!isClickZoom)
                    {
                        isClickZoom = true;
                        splitSatelliteName = hitInfo.collider.name.Split('_');
                        IocContainer_InstanceMgr.GetInstance().GetInstance<LoadQuests>().ShowSingleSatellite(int.Parse(splitSatelliteName[1]), false);
                        text_Name.text = satelliteName+"(click to zoom)";
                    }
                    else
                    {
                        IocContainer_InstanceMgr.GetInstance().GetInstance<LoadQuests>().isLoadFinsh = true;
                        IocContainer_InstanceMgr.GetInstance().GetInstance<UseDataEarthRotate>().currClickPerspectiveSatellite = hitInfo.collider.transform.parent.gameObject;
                        if (Camera.main.gameObject.GetComponent<Rotate>() != null)
                        {
                            Camera.main.gameObject.GetComponent<Rotate>().center = hitInfo.collider.transform.parent.gameObject;
                        }

                        //��������ӷŴ���С�Ĺ��ܽű����
                        sateScale = hitInfo.collider.transform.parent.gameObject.GetComponent<ScaleObject>();
                        if (sateScale != null)
                        {
                            sateScale.enabled = true;
                        }
                        else
                            hitInfo.collider.transform.parent.gameObject.AddComponent<ScaleObject>();
                        IocContainer_InstanceMgr.GetInstance().GetInstance<UseDataEarthRotate>().SwitchSatelliteView(true);
                        Debug.Log("���������ӽ�");
                    }
                }
            }
        }
    }


    /// <summary>
    /// ������������
    /// </summary>
    /// <param name="Sname"></param>
    public void SetName(string Sname)
    {
        text_Name.text = Sname;
    }

    /// <summary>
    /// ������뵽������
    /// </summary>
    private void OnMouseEnter()
    {
        Debug.Log("�����������:"+ satelliteName);
        IocContainer_InstanceMgr.GetInstance().GetInstance<LoadQuests>().isLoadFinsh = false;
    }


    /// <summary>
    /// ����Ƴ�����
    /// </summary>
    private void OnMouseExit()
    {
        IocContainer_InstanceMgr.GetInstance().GetInstance<LoadQuests>().isLoadFinsh = true;
        Debug.Log("����Ƴ�����");
    }

}
