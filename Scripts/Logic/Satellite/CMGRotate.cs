using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CMGRotate : MonoBehaviour
{
    /// <summary>
    /// 转的方向
    /// </summary>
    public Vector3 Rotate = Vector3.up;
    /// <summary>
    /// 方向索引值
    /// </summary>
    public int DirIndex = -1;

    public Action<float> cmgAngAc = null;

    float x = 0.0f;
    float y = 0.0f;
    float z = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        cmgAngAc = SetCMGOuterFrame;
        x = this.transform.localEulerAngles.x;
        y = this.transform.localEulerAngles.y;
        z = this.transform.localEulerAngles.z;
    }

    /// <summary>
    /// 设置CMG外壳角度信息
    /// </summary>
    public void SetCMGOuterFrame(float ang)
    {
        switch (DirIndex)
        {
            case 1:
                transform.localEulerAngles = new Vector3(ang, y, z);
                break;
            case 2:
                transform.localEulerAngles = new Vector3(x, ang, z);
                break;
            case 3:
                transform.localEulerAngles = new Vector3(x, y, ang);
                break;
        }       
    }

    float localTime = 0.0f;
    float currAng = 0;
    // Update is called once per frame
    void Update()
    {
        //localTime += Time.deltaTime;
        //if (localTime>=0.5f)
        //{
        //    localTime = 0.0f;
        //    SetCMGOuterFrame(currAng);
        //    currAng++;
        //}
    }
}
