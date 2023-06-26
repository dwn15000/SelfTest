using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestLoadSatellite : MonoBehaviour
{
    /// <summary>
    /// 显示的太阳直射点经纬度Image
    /// </summary>
    public GameObject showObjImage;

    public RawImage shadow;

    public LineRenderer lineRenderer;
    /// <summary>
    /// 显示真正解析的经纬度数据
    /// </summary>
    public Text txt_lonLat;

    List<OrbitData> satellites = new List<OrbitData>();

    /// <summary>
    /// 是否加载完成
    /// </summary>
    bool isLoadFinish = false;
    /// <summary>
    /// 当前完成的个数
    /// </summary>
    int count = 1;
    /// <summary>
    /// 计时器时间
    /// </summary>
    float timers = 0.0f;
    /// <summary>
    /// 获取到的时间数组
    /// </summary>
    int[] dateTime;

    float ScreenWidth = 0.0f;

    /// <summary>
    /// 经纬度
    /// </summary>
    Vector3 lonLat;
    // Start is called before the first frame update
    void Start()
    {
        
        //lineRenderer.startWidth = 0.3f;
        //lineRenderer.endWidth = 0.3f;
        //lineRenderer.useWorldSpace = true;

        ScreenWidth = PlayerPrefs.GetFloat("CanvasWidth", 0.0f);

        screenWidthdifference = PlayerPrefs.GetFloat("CanvasWidth", 0.0f)/360;
        screenHeightdifference = PlayerPrefs.GetFloat("Canvasheight", 0.0f) / 180;
        Debug.Log("屏幕宽高比-screenWidthdifference:" + screenWidthdifference + ",,,screenHeightdifference=" + screenHeightdifference);

        TextAsset questdata = Resources.Load<TextAsset>("data/SAT1_RVandQbi");
        string[] data = questdata.text.Split(new char[] { '\n' });
        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(',');

            if (row.Length < 10)
            {
                continue;
            }
            OrbitData sate = new OrbitData();
            sate.time = row[0];
            sate.x = float.Parse(row[1]);
            sate.y = float.Parse(row[2]);
            sate.z = float.Parse(row[3]);

            sate.vx = float.Parse(row[4]);
            sate.vy = float.Parse(row[5]);
            sate.vz = float.Parse(row[6]);

            float.Parse(row[3]);
            sate.q1 = float.Parse(row[7]);
            sate.q2 = float.Parse(row[8]);
            sate.q3 = float.Parse(row[9]);
            satellites.Add(sate);
        }
        isLoadFinish = true;
    }

    int currentPoint = 0;
   
    // Update is called once per frame
    void Update()
    {
        if (isLoadFinish)
        {
            timers += Time.deltaTime;
            if (timers >= 0.05f)
            {
                timers = 0.0f;
                dateTime = DataTime(satellites[count].time);
                lonLat = IocContainer_InstanceMgr.GetInstance().GetInstance<SolarSystemEphemerisCtrl>().Test_JPL430eph(dateTime);
                txt_lonLat.text = "Lon:" + lonLat.x + " Lat:" + lonLat.y;
               
                showObjImage.transform.localPosition = GetShowLonLatData(lonLat);
                MoveEarthShadow(showObjImage.transform.localPosition.x);
                count +=20;
                //this.lineRenderer.positionCount = (currentPoint + 1);
                ////画线:
                //this.lineRenderer.SetPosition(currentPoint, SatelliteTrans[i].position);
                //currentPoint++;
            }
            if (count>= satellites.Count)
            {
                isLoadFinish = false;
            }
        }
       
    }


    Vector3 lonLatPos;
    /// <summary>
    /// 屏幕宽比列
    /// </summary>
    float screenWidthdifference = 0.0f;
    /// <summary>
    /// 屏幕高比列
    /// </summary>
    float screenHeightdifference = 0.0f;

    /// <summary>
    /// 得到显示的经纬度数据
    /// </summary>
    /// <returns></returns>
    Vector3 GetShowLonLatData(Vector3 pos)
    {
        
        lonLatPos.x = (pos.x - 180) * screenWidthdifference;
        lonLatPos.y = pos.y * screenHeightdifference;
        return lonLatPos;
    }

    float backPos = 0.0f;

    /// <summary>
    /// 移动阴影区图
    /// </summary>
    void MoveEarthShadow(float x)
    {

        float a = (ScreenWidth / 2) + x;
        a = a * (-0.00026f);//最终用于计算
        shadow.uvRect = new Rect(a, 0, 1, 1);

        backPos = x;
    }



    int[] strs = new int[6];
    int[] DataTime(string data)
    {
        string[] splits = data.Split(' ');
        string[] nyr = splits[0].Split('/');
        string[] sfm = splits[1].Split(':');
        string[] second = sfm[2].Split('.');
        if (second.Length > 1)
        {
            sfm[2] = second[0];
        }
        for (int i = 0; i < strs.Length; i++)
        {
            if (i < 3)
            {
                strs[i] = int.Parse(nyr[i]);
            }
            else
                strs[i] = int.Parse(sfm[i - 3]);

        }
        return strs;
    }


    private class OrbitData
    {
        public string time; // 时间
        public float x; // X轴坐标
        public float y; // Y轴坐标
        public float z; // Z轴坐标
        public float vx; // X轴速度
        public float vy; // Y轴速度
        public float vz; // Z轴速度
        public float q1;  //四元数的x
        public float q2;  //四元数的y
        public float q3;  //四元数的z
        public float q4;  //四元数的w
        public float[] CMGAng = new float[6];
        public float[] CMGRotate = new float[6];
        public float CMG1_ang; //CMG1外框角度信息
        public float CMG1_rate; //CMG1内转子转速信息(转每分)
        public float CMG2_ang; //CMG2外框角度信息
        public float CMG2_rate; //CMG2内转子转速信息(转每分)
        public float CMG3_ang; //CMG3外框角度信息
        public float CMG3_rate; //CMG3内转子转速信息(转每分)
        public float CMG4_ang; //CMG4 外框角度信息
        public float CMG4_rate; //CMG4 内转子转速信息(转每分)
        public float CMG5_ang; //CMG5 外框角度信息
        public float CMG5_rate; //CMG5 内转子转速信息(转每分)
        public float CMG6_ang; //CMG6 外框角度信息
        public float CMG6_rate; //CMG6 内转子转速信息(转每分)
    }
}

