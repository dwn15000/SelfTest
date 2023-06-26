using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public Camera miniMapCamera; // С��ͼ���
    public RenderTexture miniMapTexture; // С��ͼ����
    public LayerMask miniMapLayerMask = -1; // С��ͼ��Ⱦ�㼶
    private RenderTargetIdentifier miniMapRenderTarget; // С��ͼ��ȾĿ��

    /// <summary>
    /// �Ƿ����С��ͼ����
    /// </summary>
    public bool IsEnterMinMapPanel = true;

    /// <summary>
    /// ����
    /// </summary>
    public Button btn_back;
    /// <summary>
    /// ���뱬ը��ͼ����
    /// </summary>
    public Button btn_enterExplo;

    public GameObject gameobjBack;

    public RawImage miniMapImage; // С��ͼRaw Image
    private void Awake()
    {
        IocContainer_InstanceMgr.GetInstance().RegistInstance(this);
    }


    private void Start()
    {
        // ����С��ͼ��ȾĿ��
        miniMapRenderTarget = new RenderTargetIdentifier(miniMapTexture);
        SetMinMapRenderTexture(true);
        ///���ص���������
        btn_back.onClick.AddListener(()=> {
            //SetMinMapRenderTexture(true);
            //IsEnterMinMapPanel = true;
        });
        ///�������µ�켣ͼ����
        btn_enterExplo.onClick.AddListener(() => {
            //SetMinMapRenderTexture(false);
            //IsEnterMinMapPanel = false;
            Debug.LogError("�������µ�켣ͼ����");

            UISatelliteSubstellar.Self.substellarGame.gameObject.SetActive(true);
            IocContainer_InstanceMgr.GetInstance().GetInstance<MainCameraOverallControl>().CanControl = false;
            Camera.main.transform.localPosition = new Vector3(14.7403f, -58.17964f, -221.3122f);
            Camera.main.transform.localEulerAngles = Vector3.zero;
            Camera.main.transform.parent.transform.localPosition = new Vector3(188f, 61f, 32f);

            //���ص���
            IocContainer_InstanceMgr.GetInstance().GetInstance<UseDataEarthRotate>().EarthObject.gameObject.SetActive(false);

            if (UISatelliteSubstellar.Self.subStarPointLine != null)
            {
                UISatelliteSubstellar.Self.subStarPointLine.layer = 5;
                //UISatelliteSubstellar.Self.subStarPointLine.SetActive(true);
            }
           EarthUIPanel.Self.SetCanvasRenderMode(true);
            //�������ǣ��������Ƕ��˾��������б�
            for (int i = 0; i < IocContainer_InstanceMgr.GetInstance().GetInstance<LoadQuests>().SatelliteTrans.Length; i++)
            {
                IocContainer_InstanceMgr.GetInstance().GetInstance<LoadQuests>().SatelliteTrans[i].gameObject.SetActive(false);
            }

        });
    }
    private void LateUpdate()
    {
        // ����С��ͼ�����λ�ú���ת
        // miniMapCamera.transform.position = transform.position + Vector3.up * miniMapCamera.transform.position.y;
        //miniMapCamera.transform.rotation = Quaternion.Euler(90, transform.eulerAngles.y, 0);
        // ��С��ͼ�����culling mask����Ϊָ����LayerMask
        miniMapCamera.cullingMask = miniMapLayerMask;
        // ��ʼ��ȾС��ͼ
        var cmd = new CommandBuffer();
        cmd.SetRenderTarget(miniMapRenderTarget);
        cmd.ClearRenderTarget(true, true, Color.black);
        miniMapCamera.AddCommandBuffer(CameraEvent.BeforeForwardAlpha, cmd);
        miniMapImage.texture = miniMapTexture;
    }


    /// <summary>
    /// ����RenderTexture
    /// </summary>
    public void SetMinMapRenderTexture(bool isShow)
    {
        if (isShow)
        {
            IsEnterMinMapPanel = isShow;
            miniMapCamera.targetTexture = miniMapTexture;
            //��ʾС��ͼ��
        }
        else
        {
            
            miniMapCamera.targetTexture = null;            
        }
        //IocContainer_InstanceMgr.GetInstance().GetInstance<MainController>().LineAxisGameObj.SetActive(!isShow);
        //IocContainer_InstanceMgr.GetInstance().GetInstance<MainController>().ShowHierarchyUI.SetActive(!isShow);
        miniMapImage.gameObject.SetActive(isShow);
        gameobjBack.gameObject.SetActive(!isShow);
    }
}
