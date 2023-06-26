using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 用鼠标滚轮缩放物体
/// </summary>
public class ScaleObject : MonoBehaviour
{
    // 缩放的速度
    [Tooltip("缩放的速度")]
    public float scaleSpeed = 1f;

    // 最小缩放值
    [Tooltip("最小缩放值")]
    public float minScale = 0.5f;

    // 最大缩放值
    [Tooltip("最大缩放值")]
    public float maxScale = 16f;

    // 是否在缩放时保持物体的长宽比
    [Tooltip("是否在缩放时保持物体的长宽比")]
    public bool maintainAspectRatio = true;

    // 当前缩放值
    private float currentScale;

    // 初始缩放值
    private Vector3 initialScale;

    /// <summary>
    /// 原始Y轴位置
    /// </summary>
    public float oldAxisY = 0.0f;
    /// <summary>
    /// 原始位置
    /// </summary>
    public Vector3 oldPos;

    /// <summary>
    /// 可以透视的物体
    /// </summary>
    List<GameObject> CanperspectiveList = new List<GameObject>();
    // 初始化
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
    /// 显示或者隐藏的透视物体
    /// </summary>
    void HideOrShowPerspectiveObj(bool is_show)
    {
        foreach (var item in CanperspectiveList)
        {
            item.GetComponent<MeshRenderer>().enabled=is_show;  
        }
    }


    float scaleFactor = 0.0f;
    // 更新
    void Update()
    {
        // 获取缩放输入值
         scaleFactor = Input.GetAxis("Mouse ScrollWheel");
        
        // 如果输入不为0
        if (scaleFactor != 0)
        {
            // 计算缩放值
            currentScale += scaleFactor * scaleSpeed;

            // 限制缩放范围
            currentScale = Mathf.Clamp(currentScale, minScale, maxScale);

            // 设置缩放
            //Vector3 newScale = initialScale * currentScale;
            Vector3 newScale = Vector3.one* currentScale;
            //Debug.Log("缩放值initialScale:" + initialScale + ",,currentScale="+ currentScale+",,newscale="+newScale);
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

