using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ModelRotate : MonoBehaviour
{
    /// <summary>
    /// 旋转速度
    /// </summary>
    public float FlywheelRotationSpeed = 6000f;

    /// <summary>
    /// 转的方向
    /// </summary>
    public Vector3 Rotate = Vector3.forward;

    public Action<float> cmgRateAc = null;

    float time = 0.0f;

    float x = 0.0f;
    float y = 0.0f;

    private void Start()
    {
        cmgRateAc = SetFlywheeSpeed;
        x = this.transform.localEulerAngles.x;
        y = this.transform.localEulerAngles.y;
    }

    float increment = 0;

    void Update()
    {
        //time += Time.deltaTime;
        //if (time>=0.5f)
        //{
        //    time = 0.0f;
        //    transform.localEulerAngles = new Vector3(x,y, increment);
        //    increment++;
        //}
        // 将每秒的旋转速度转换为每帧的旋转角度，并围绕Y轴旋转
        transform.Rotate(Rotate * Time.deltaTime * FlywheelRotationSpeed);
    }


    /// <summary>
    /// 设置飞轮转速
    /// </summary>
    public void SetFlywheeSpeed(float speed)
    {
        FlywheelRotationSpeed = speed;
    }

    //void Update()
    //{
    //    // 模型组件自转
    //    transform.Rotate(Rotate,Time.deltaTime* FlywheelRotationSpeed / 60f);

    //}
}
