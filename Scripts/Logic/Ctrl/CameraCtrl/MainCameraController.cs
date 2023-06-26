using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
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
    /// <summary>
    /// 转动速度
    /// </summary>
    [Tooltip("转动速度")]
    public float rotationSpeed = 10;

    private void Awake()
    {
        Debug.Log("主摄像机旋转了ff");
    }

    private void Start()
    {
        //注册实例
        IocContainer_InstanceMgr.GetInstance().RegistInstance<MainCameraController>(this);
    }

    void Update()
    {
        CameraRot();
    }

    void CameraRot()
    {
        if (CanControl)
        {
            //点击鼠标左键旋转摄像头：
            if (Input.GetMouseButton(0) && center != null)
            {
                #region 以前的
                /*
                mouse_y = Input.GetAxis("Mouse X") * sensitivityHor;
                mouse_x = Input.GetAxis("Mouse Y") * sensitivityVert;

                _rotationX -= mouse_x;
                _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);

                transform.RotateAround(center.transform.position, Vector3.up, mouse_y * rotationSpeed);
                //transform.localEulerAngles = new Vector3(_rotationX, transform.localEulerAngles.y, 0);
                transform.RotateAround(center.transform.position, Vector3.left, mouse_x * 5);
                */
                #endregion

                #region 优化后的，好用和正确的
                float horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed;
                float verticalRotation = Input.GetAxis("Mouse Y") * rotationSpeed;
                transform.RotateAround(center.transform.position, Vector3.up, horizontalRotation);
                transform.RotateAround(center.transform.position, transform.right, -verticalRotation);
                #endregion
            }

            //用鼠标滚轮调节摄像机的视野值：
            if (bl_UsingMouseWheelScalingCameraFieldOfView == true)
            {
                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    if (Camera.main.fieldOfView <= 170)
                        Camera.main.fieldOfView += 2;
                    if (Camera.main.orthographicSize <= 20)
                        Camera.main.orthographicSize += 0.5F;
                }

                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    if (Camera.main.fieldOfView > 2)
                        Camera.main.fieldOfView -= 2;
                    if (Camera.main.orthographicSize >= 1)
                        Camera.main.orthographicSize -= 0.5F;
                }
            }

        }
    }
}
