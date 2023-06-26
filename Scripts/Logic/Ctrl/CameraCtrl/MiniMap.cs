using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public Camera miniMapCamera; // 小地图相机
    public RenderTexture miniMapTexture; // 小地图纹理
    public LayerMask miniMapLayerMask = -1; // 小地图渲染层级
    private RenderTargetIdentifier miniMapRenderTarget; // 小地图渲染目标

    /// <summary>
    /// 是否进入小地图界面
    /// </summary>
    public bool IsEnterMinMapPanel = true;

    /// <summary>
    /// 返回
    /// </summary>
    public Button btn_back;
    /// <summary>
    /// 进入爆炸视图界面
    /// </summary>
    public Button btn_enterExplo;

    public GameObject gameobjBack;

    public RawImage miniMapImage; // 小地图Raw Image
    private void Awake()
    {
        IocContainer_InstanceMgr.GetInstance().RegistInstance(this);
    }


    private void Start()
    {
        // 设置小地图渲染目标
        miniMapRenderTarget = new RenderTargetIdentifier(miniMapTexture);
        SetMinMapRenderTexture(true);
        ///返回到正常界面
        btn_back.onClick.AddListener(()=> {
            //SetMinMapRenderTexture(true);
            //IsEnterMinMapPanel = true;
        });
        ///进入星下点轨迹图界面
        btn_enterExplo.onClick.AddListener(() => {
            //SetMinMapRenderTexture(false);
            //IsEnterMinMapPanel = false;
            Debug.LogError("进入星下点轨迹图界面");

            UISatelliteSubstellar.Self.substellarGame.gameObject.SetActive(true);
            IocContainer_InstanceMgr.GetInstance().GetInstance<MainCameraOverallControl>().CanControl = false;
            Camera.main.transform.localPosition = new Vector3(14.7403f, -58.17964f, -221.3122f);
            Camera.main.transform.localEulerAngles = Vector3.zero;
            Camera.main.transform.parent.transform.localPosition = new Vector3(188f, 61f, 32f);

            //隐藏地球
            IocContainer_InstanceMgr.GetInstance().GetInstance<UseDataEarthRotate>().EarthObject.gameObject.SetActive(false);

            if (UISatelliteSubstellar.Self.subStarPointLine != null)
            {
                UISatelliteSubstellar.Self.subStarPointLine.layer = 5;
                //UISatelliteSubstellar.Self.subStarPointLine.SetActive(true);
            }
           EarthUIPanel.Self.SetCanvasRenderMode(true);
            //隐藏卫星（后面卫星多了就是卫星列表）
            for (int i = 0; i < IocContainer_InstanceMgr.GetInstance().GetInstance<LoadQuests>().SatelliteTrans.Length; i++)
            {
                IocContainer_InstanceMgr.GetInstance().GetInstance<LoadQuests>().SatelliteTrans[i].gameObject.SetActive(false);
            }

        });
    }
    private void LateUpdate()
    {
        // 更新小地图相机的位置和旋转
        // miniMapCamera.transform.position = transform.position + Vector3.up * miniMapCamera.transform.position.y;
        //miniMapCamera.transform.rotation = Quaternion.Euler(90, transform.eulerAngles.y, 0);
        // 将小地图相机的culling mask设置为指定的LayerMask
        miniMapCamera.cullingMask = miniMapLayerMask;
        // 开始渲染小地图
        var cmd = new CommandBuffer();
        cmd.SetRenderTarget(miniMapRenderTarget);
        cmd.ClearRenderTarget(true, true, Color.black);
        miniMapCamera.AddCommandBuffer(CameraEvent.BeforeForwardAlpha, cmd);
        miniMapImage.texture = miniMapTexture;
    }


    /// <summary>
    /// 设置RenderTexture
    /// </summary>
    public void SetMinMapRenderTexture(bool isShow)
    {
        if (isShow)
        {
            IsEnterMinMapPanel = isShow;
            miniMapCamera.targetTexture = miniMapTexture;
            //显示小地图的
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
