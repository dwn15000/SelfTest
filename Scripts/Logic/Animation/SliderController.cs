using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderController : MonoBehaviour
{
    /*
     *用于Slider滑动控制的判断
     */
    public Slider slider =null;
    public Scrollbar scrBar = null;
    private Coroutine coroutine; // 协程对象

    /// <summary>
    /// 滑动结束是否还原初始位置
    /// </summary>
    public bool EndIsReduction = false;

    /// <summary>
    /// 初始位置值
    /// </summary>
    public float InitPosVal = 0.5f;
    private void Start()
    {
        //IocContainer_InstanceMgr.GetInstance().RegistInstance(this);
        if (slider!=null)
        {
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }
        if (scrBar != null)
        {
            scrBar.onValueChanged.AddListener(OnSliderValueChanged);
        }
        
    }
    private void OnSliderValueChanged(float value)
    {
        //Debug.Log("OnSliderValueChanged:"+ value);
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        GlobalSliderCtrl.GetInstance().GetIsCanCtrlCamera = true;
        if (slider != null)
        {
            coroutine = StartCoroutine(CheckSliderStopped());
        }
        if (scrBar != null)
        {
            coroutine = StartCoroutine(CheckScoBarStopped());
        }

        
    }
    private IEnumerator CheckSliderStopped()
    {
        float currentValue = slider.value;
       
        yield return new WaitForSeconds(0.1f); // 等待0.1秒
        while (currentValue != slider.value)
        {
            currentValue = slider.value;
            yield return new WaitForSeconds(0.1f); // 等待0.1秒
        }
        GlobalSliderCtrl.GetInstance().GetIsCanCtrlCamera = false;
        Debug.LogError("Slider stopped");
    }

    private IEnumerator CheckScoBarStopped()
    {
        float currentValue = scrBar.value;
        yield return new WaitForSeconds(0.1f); // 等待0.1秒
        while (currentValue != scrBar.value)
        {
            currentValue = scrBar.value;
            yield return new WaitForSeconds(0.1f); // 等待0.1秒
        }
        GlobalSliderCtrl.GetInstance().GetIsCanCtrlCamera = false;
        if (EndIsReduction)
        {
            scrBar.value = InitPosVal;
        }
        Debug.LogError("ScrBar stopped");
    }


}
