using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraCon : MonoBehaviour
{

    public bool CanControl;
    public bool bl_UsingMouseWheelScalingCameraFieldOfView;
    public GameObject center;
    float sensitivityHor = 1.0f;
    float sensitivityVert = 1.5f;
    float rotationY = 0.0f;
    float _rotationX = 180.0f;
    float minimumVert = -13.0f;
    float maximumVert = 26f;
    float mouse_y = 0.0f;
    float mouse_x = 0.0f;

    public float rotationSpeed = 10;


    private void Start()
    {
        //×¢²áÊµÀý
        IocContainer_InstanceMgr.GetInstance().RegistInstance(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
