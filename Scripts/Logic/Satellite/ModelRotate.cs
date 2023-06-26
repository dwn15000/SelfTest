using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ModelRotate : MonoBehaviour
{
    /// <summary>
    /// ��ת�ٶ�
    /// </summary>
    public float FlywheelRotationSpeed = 6000f;

    /// <summary>
    /// ת�ķ���
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
        // ��ÿ�����ת�ٶ�ת��Ϊÿ֡����ת�Ƕȣ���Χ��Y����ת
        transform.Rotate(Rotate * Time.deltaTime * FlywheelRotationSpeed);
    }


    /// <summary>
    /// ���÷���ת��
    /// </summary>
    public void SetFlywheeSpeed(float speed)
    {
        FlywheelRotationSpeed = speed;
    }

    //void Update()
    //{
    //    // ģ�������ת
    //    transform.Rotate(Rotate,Time.deltaTime* FlywheelRotationSpeed / 60f);

    //}
}
