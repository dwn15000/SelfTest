using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public bool CanControl;

    public bool is_useOld =false;

    public GameObject center;

    public bool IsMainCamer = true;

    public Camera minCamera = null;

    /// <summary>
    /// �������ת�ٶ�
    /// </summary>
    public float speed = 1.0f;

    /// <summary>
    /// �Ƿ�����������ת�����
    /// </summary>
    public bool IsRotateSatellite = false;

    public bool ModelLibray = false;
    private void Start()
    {
        //ע��ʵ��
        IocContainer_InstanceMgr.GetInstance().RegistInstance<Rotate>(this);
        if (minCamera!=null)
        {
            minCamera.orthographicSize = 10;
        }
        IocContainer_InstanceMgr.GetInstance().GetInstance<Rotate>().enabled = false;
    }


    void Update()
    {
       
        CameraRot();

    }
    private void Awake()
    {
        Debug.Log("��ת=");

    }

    public float zoomSpeed = 1.0f;
    /// <summary>
    /// ����������ľ���
    /// </summary>
    public float minDistance = 3.0f;
    /// <summary>
    /// ��������Զ�ľ���
    /// </summary>
    public float maxDistance = 6.0f;

    float sensitivityHor = 1.0f;

    float sensitivityVert = 1.5f;

    float rotationY = 0.0f;

    float _rotationX = 180.0f;

    float minimumVert = 15f;

    float maximumVert = 46f;

   

    float mouse_y = 0.0f;

    float mouse_x = 0.0f;

    float mouseSatellite_x = 0.0f;
    float _SatelliteRotationX = 180.0f;
    /// <summary>
    /// ת���ٶ�
    /// </summary>
    float rotationSpeed = 10;
    /// <summary>
    /// �����Ƕ����������״̬
    /// </summary>
    bool ViewSatatus = false;
    void CameraRot()
    {
        if (CanControl)
        {
            //�����������ת����ͷ
            if (Input.GetMouseButton(0) && center != null)
            {

                #region ��ǰ��
                //mouse_y = Input.GetAxis("Mouse X") * sensitivityHor;
                //mouse_x = Input.GetAxis("Mouse Y") * sensitivityVert;
                //_rotationX -= mouse_x;
                //_rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);
                //transform.RotateAround(center.transform.position, Vector3.up, mouse_y * rotationSpeed);
                //if (!is_useOld)
                //{
                //    transform.localEulerAngles = new Vector3(_rotationX, transform.localEulerAngles.y, 0);
                //}
                //else
                //    transform.RotateAround(center.transform.position, Vector3.left, mouse_x * 5);
                #endregion

                #region �Ż����
                float horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed;
                float verticalRotation = Input.GetAxis("Mouse Y") * rotationSpeed;
                transform.RotateAround(center.transform.position, Vector3.up, horizontalRotation);
                transform.RotateAround(center.transform.position, transform.right, -verticalRotation);
                #endregion
            }
            if (Input.GetMouseButton(1) && IsRotateSatellite)
            {
                #region �ɰ����������ת
                //float deltaX = Input.GetAxis("Mouse X");
                ////float deltaY = Input.GetAxis("Mouse Y");
                //Vector3 rotation = center.transform.rotation.eulerAngles;
                //rotation.x -= deltaX*2;
                //rotation.y += deltaY*2;
                //Quaternion targetRotation = Quaternion.Euler(rotation);
                //center.transform.rotation = targetRotation;
                //Debug.LogError("deltaY:" + deltaY);
                //ʹ���������������
                //if (deltaY < 0.0f) //������������
                //{
                //    center.transform.localEulerAngles = new Vector3(193.285f, 1.45f, -0.389f);
                //}
                //else if (deltaY > 0.0f)  //�����Ǻ�����
                //{
                //    center.transform.localEulerAngles = new Vector3(68.623f, 0, -48.611f);
                //}
                #endregion

                #region �°��Ż����
                float horizontalRotation = Input.GetAxis("Mouse X") * speed;
                float verlRotation = Input.GetAxis("Mouse Y") * speed;
                transform.Rotate(-verlRotation, horizontalRotation, 0, Space.Self);
                #endregion
            }
            if (IsMainCamer)
            {
                #region �ɰ��
                if (ModelLibray)
                {
                    if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    {
                        Debug.Log("������С��0"); //������Զ
                        if (Camera.main.fieldOfView <= 100)
                            Camera.main.fieldOfView += 2;
                        if (Camera.main.orthographicSize <= 20)
                            Camera.main.orthographicSize += 0.5F;
                    }
                    //Zoom in
                    if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        Debug.Log("�����Ĵ���0"); //�������
                        if (Camera.main.fieldOfView > 2)
                            Camera.main.fieldOfView -= 2;
                        if (Camera.main.orthographicSize >= 1)
                            Camera.main.orthographicSize -= 0.5F;
                    }
                }

                #endregion
                #region �°��Ż����
                //if (Input.GetAxis("Mouse ScrollWheel") < 0)
                //{

                //    float distance = Vector3.Distance(transform.position, center.transform.position);
                //    float scroll = Input.GetAxis("Mouse ScrollWheel");
                //    distance -= scroll * zoomSpeed;
                //    distance = Mathf.Clamp(distance, minDistance, maxDistance);
                //    Debug.Log("������С��0������Զ:" + (-distance));
                //    transform.position = transform.position + new Vector3(0, 0, -distance);
                //}
                ////Zoom in
                //if (Input.GetAxis("Mouse ScrollWheel") > 0)
                //{                   
                //    float distance = Vector3.Distance(transform.position, center.transform.position);
                //    float scroll = Input.GetAxis("Mouse ScrollWheel");
                //    distance -= scroll * zoomSpeed;
                //    distance = Mathf.Clamp(distance, minDistance, maxDistance);
                //    Debug.Log("�����Ĵ���0�������:"+(-distance));
                //    transform.position = transform.position + new Vector3(0, 0, -distance);
                //}
                #endregion

            }
            else
            {
                ///С��ͼ�����
                if (Input.GetAxis("Mouse ScrollWheel") < 0 && minCamera!=null)
                {
                    if (minCamera.orthographicSize <= 100)
                        minCamera.orthographicSize += 2;
                    if (minCamera.orthographicSize <= 20)
                        minCamera.orthographicSize += 0.5F;
                }
                //Zoom in
                if (Input.GetAxis("Mouse ScrollWheel") > 0 && minCamera != null)
                {
                    if (minCamera.orthographicSize > 2)
                        minCamera.orthographicSize -= 2;
                    if (minCamera.orthographicSize >= 1)
                        minCamera.orthographicSize -= 0.5F;
                }
            }
            
        }
    }

}
