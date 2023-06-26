using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// ����������������
/// </summary>
public class ScaleObject : MonoBehaviour
{
    // ���ŵ��ٶ�
    [Tooltip("���ŵ��ٶ�")]
    public float scaleSpeed = 1f;

    // ��С����ֵ
    [Tooltip("��С����ֵ")]
    public float minScale = 0.5f;

    // �������ֵ
    [Tooltip("�������ֵ")]
    public float maxScale = 16f;

    // �Ƿ�������ʱ��������ĳ����
    [Tooltip("�Ƿ�������ʱ��������ĳ����")]
    public bool maintainAspectRatio = true;

    // ��ǰ����ֵ
    private float currentScale;

    // ��ʼ����ֵ
    private Vector3 initialScale;

    /// <summary>
    /// ԭʼY��λ��
    /// </summary>
    public float oldAxisY = 0.0f;
    /// <summary>
    /// ԭʼλ��
    /// </summary>
    public Vector3 oldPos;

    /// <summary>
    /// ����͸�ӵ�����
    /// </summary>
    List<GameObject> CanperspectiveList = new List<GameObject>();
    // ��ʼ��
    void Start()
    {
        initialScale = transform.localScale;
        currentScale = 1f;
        Traverse(this.gameObject,false);
    }

    int cmgAngIndex = 0;
    int cmgRateIndex = 0;
    string[] ModelData;
    void Traverse(GameObject obj, bool is_show)
    {
        foreach (var item in IocContainer_InstanceMgr.GetInstance().GetInstance<LoadFileConfig>().allModelRotateName)
        {
            ModelData = item.Split(',');
            if (obj.name==ModelData[0])
            {
                
                if (int.Parse(ModelData[1]) == 10000)
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

    /// <summary>
    /// ��ʾ�������ص�͸������
    /// </summary>
    void HideOrShowPerspectiveObj(bool is_show)
    {
        foreach (var item in CanperspectiveList)
        {
            item.GetComponent<MeshRenderer>().enabled=is_show;  
        }
    }


    float scaleFactor = 0.0f;
    // ����
    void Update()
    {
        // ��ȡ��������ֵ
         scaleFactor = Input.GetAxis("Mouse ScrollWheel");
        
        // ������벻Ϊ0
        if (scaleFactor != 0)
        {
            // ��������ֵ
            currentScale += scaleFactor * scaleSpeed;

            // �������ŷ�Χ
            currentScale = Mathf.Clamp(currentScale, minScale, maxScale);

            // ��������
            //Vector3 newScale = initialScale * currentScale;
            Vector3 newScale = Vector3.one* currentScale;
            //Debug.Log("����ֵinitialScale:" + initialScale + ",,currentScale="+ currentScale+",,newscale="+newScale);
            if (maintainAspectRatio)
            {
                newScale.y = newScale.x;
            }
            transform.position = new Vector3(oldPos.x, oldPos.y+ newScale.y, oldPos.z);
            transform.localScale = newScale;
            if (newScale.x >= maxScale*0.6)
            {
                HideOrShowPerspectiveObj(false);
            }
            else if (newScale.x<=5f)
            {
                HideOrShowPerspectiveObj(true);
            }
               
        }
    }
}

