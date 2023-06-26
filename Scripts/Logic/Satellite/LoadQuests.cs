using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.IO;
using System.Linq;

public class LoadQuests : MonoBehaviour
{
    float totalTime;
    /// <summary>
    /// 卫星模型Transform组件
    /// </summary>
    public Transform[] SatelliteTrans;
    public Transform Earth;
    public Transform MainMinMapCamer;

    List<List<OrbitData>> sateAllList = new List<List<OrbitData>>();

    List<OrbitData> satellites = new List<OrbitData>();

    List<OrbitData> tempSatellite = new List<OrbitData>();

    /// <summary>
    /// 卫星组件的LineRender
    /// </summary>
    public LineRenderer[] lineRenderer;

    /// <summary>
    /// 星下点线
    /// </summary>
    public LineRenderer subStarPointLineRender;
    /// <summary>
    /// 比较的时间
    /// </summary>
    public float CompareTime = 0.01f;

    /// <summary>
    /// 数据表是否加载完成
    /// </summary>
    [HideInInspector]
    public bool isLoadFinsh = false;
    /// <summary>
    /// 当前画线的个数
    /// </summary>
    public int currentPoint = 0;

    private Vector3[] j2000Positions;
    // 地球自转的速度
    public float rotationSpeed = 15.0f;

    [HideInInspector]
    public int count = 1;

    /// <summary>
    /// 当前点击的卫星编号
    /// </summary>
    [HideInInspector]
    public int CurrCliskSatelliteIndex = 1;

    /// <summary>
    /// 快进速度
    /// </summary>
    public int FastSpeed = 20;
    /// <summary>
    /// 目标位置
    /// </summary>
    private List<Vector3> targetPosition = new List<Vector3>();

    /// <summary>
    /// Int类型时间数组
    /// </summary>
    private int[] DateTimeInt;

    /// <summary>
    /// 计算出来的太阳直射点经纬度
    /// </summary>
    private Vector3 lonLat;

    string[] baseParam = new string[4];
    private void Awake()
    {
        IocContainer_InstanceMgr.GetInstance().RegistInstance(this);
    }

    int angIndex = 0;
    void Start()
    {

        //LineRender相关参数
        for (int j = 0; j < SatelliteTrans.Length; j++)
        {  
            lineRenderer[j] = SatelliteTrans[j].GetComponent<LineRenderer>();
            lineRenderer[j].startWidth = 1f;
            lineRenderer[j].endWidth = 1f;
            lineRenderer[j].useWorldSpace = true;
            targetPosition.Add(SatelliteTrans[j].position);


            List<OrbitData> OItem = new List<OrbitData>();

            TextAsset questdata = Resources.Load<TextAsset>("data/SAT"+(j + 1) +"_RVandQbi");
            string[] data = questdata.text.Split(new char[] { '\n' });
            j2000Positions = new Vector3[data.Length];
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
                OItem.Add(sate);
                j2000Positions[i - 1] = new Vector3(sate.x, sate.y, sate.z);
            }
            if (j==0)
            {
                tempSatellite = OItem;
            }
            sateAllList.Add(OItem);
        }

        isLoadFinsh = true;
 
    }

    private Vector3 middlePos = new Vector3(0,0,0);
    int index = 0;
    private float timer = 0f; // 已经经过的时间
    float timers = 0;

    
   

    /// <summary>
    /// 经纬度
    /// </summary>
    Vector3 LonLat;
    void Update()
    {

        if (isLoadFinsh)
        {
            // 更新计时器     
            timers += Time.deltaTime;
            timers = 0.0f;

            #region 暂时屏蔽的
            //if (IocContainer_InstanceMgr.GetInstance().GetInstance<UseDataEarthRotate>().IsEnterMinMapPanel)
            //{

            //    #region  原本的卫星视角界面
            //    //middlePos.x = satellites[count].x;
            //    //middlePos.y = satellites[count].y;
            //    //middlePos.z = satellites[count].z;
            //    //IocContainer_InstanceMgr.GetInstance().GetInstance<UseDataEarthRotate>().SetEarthRotate(middlePos);

            //    //for (int i = 0; i < 6; i++)
            //    //{
            //    //    IocContainer_InstanceMgr.GetInstance().GetInstance<MainUIController>().CMGAngArr[i].SetCMGOuterFrame(satellites[count].CMGAng[i]);
            //    //    IocContainer_InstanceMgr.GetInstance().GetInstance<MainUIController>().CMGRotateArr[i].SetFlywheeSpeed(satellites[count].CMGRotate[i]);
            //    //}
            //    #endregion
            //    //NewCalcluLonLat(satellites[count].x, satellites[count].y, satellites[count].z, satellites[count].time);         
            //}
            //else
            //{
            //    //targetPosition = new Vector3(satellites[count].x / 100, satellites[count].y / 100, satellites[count].z / 100);

            //    //trans.position = targetPosition;

            //    //Quaternion q = Quaternion.Euler(satellites[count].q1, satellites[count].q2, satellites[count].q3);
            //    ////Vector3 eulerAngles = q.ToEulerAngles();
            //    //this.lineRenderer.positionCount = (currentPoint + 1);
            //    ////画线:
            //    //this.lineRenderer.SetPosition(currentPoint, trans.position);
            //    //currentPoint++;
            //}
            #endregion

            if (IocContainer_InstanceMgr.GetInstance().GetInstance<UseDataEarthRotate>().IsEnterMinMapPanel)
            {
                satellites = sateAllList[CurrCliskSatelliteIndex]; 
                middlePos.x = satellites[count].x;
                middlePos.y = satellites[count].y;
                middlePos.z = satellites[count].z;
                IocContainer_InstanceMgr.GetInstance().GetInstance<UseDataEarthRotate>().SetEarthRotate(middlePos,count);

                for (int i = 0; i < 6; i++)
                {
                    GlobalSatelliteCtrl.GetInstance().GetAllSatelliteName[CurrCliskSatelliteIndex].CMGAngArr[i].SetCMGOuterFrame(satellites[count].CMGAng[i]);
                    GlobalSatelliteCtrl.GetInstance().GetAllSatelliteName[CurrCliskSatelliteIndex].CMGRotateArr[i].SetFlywheeSpeed(satellites[count].CMGRotate[i]);
                }
            }
            else
            {
                for (int i = 0; i < SatelliteTrans.Length; i++)
                {
                    satellites = sateAllList[i];
                    targetPosition[i] = new Vector3(satellites[count].x / 100, satellites[count].y / 100, satellites[count].z / 100);

                    SatelliteTrans[i].position = targetPosition[i];

                    Quaternion q = Quaternion.Euler(satellites[count].q1, satellites[count].q2, satellites[count].q3);
                    //Vector3 eulerAngles = q.ToEulerAngles();
                    this.lineRenderer[i].positionCount = (currentPoint + 1);
                    //画线:
                    this.lineRenderer[i].SetPosition(currentPoint, SatelliteTrans[i].position);

                }

                LonLat = NewCalcluLonLat(tempSatellite[count].x, tempSatellite[count].y, tempSatellite[count].z, tempSatellite[count].time);

                baseParam[0] = "X:" + tempSatellite[count].vx + " Y:" + tempSatellite[count].vy + " Z:" + tempSatellite[count].vz;
                baseParam[1] = "" + tempSatellite[count].y;
                baseParam[2] = "" + tempSatellite[count].x + " " + tempSatellite[count].y + " " + tempSatellite[count].z;
                baseParam[3] = "经度:" + LonLat.x + "  纬度:" + LonLat.y;

                UISatelliteSubstellar.Self.SetPostionSpeedParamInfo(baseParam);

                lonLat = IocContainer_InstanceMgr.GetInstance().GetInstance<SolarSystemEphemerisCtrl>().Test_JPL430eph(DateTimeInt);
                UISatelliteSubstellar.Self.CallShowShadow(lonLat);

                #region 地球视角地球自转
                // 获取当前时间
                float currentTime = Time.time;
                // 计算当前时间对应的位置在J2000数据中的索引
                int currentIndex = (int)(currentTime % j2000Positions.Length);
                // 计算前后两个位置之间的插值比例
                float t = currentTime % 1.0f;
                // 计算当前时间对应的J2000位置
                Vector3 j2000Position = Vector3.Lerp(j2000Positions[currentIndex], j2000Positions[(currentIndex + 1) % j2000Positions.Length], t);
                // 将J2000坐标系中的地球位置转换为地球坐标系中的旋转角度
                Vector3 eulerAngles = j2000Position.normalized * 180.0f / Mathf.PI;
                // 将旋转角度应用到地球模型中
                Earth.rotation = Quaternion.Euler(-eulerAngles.y, eulerAngles.x, 0.0f);
                // 调整旋转速度
                Earth.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
                #endregion
            }



            currentPoint++;

            #region 如果切换到单个卫星下面就会显示具体的卫星数据
            
            #endregion

            count+= FastSpeed;

            if (count >= satellites.Count)
            {
                count = 1;
                isLoadFinsh = false;
            }

          
        }
    }


    /// <summary>
    /// 显示单个卫星
    /// </summary>
    public void ShowSingleSatellite(int index,bool isShow)
    {
        CurrCliskSatelliteIndex = index-1;
        Debug.Log("显示单个卫星=" + index);
        for (int i = 0; i < SatelliteTrans.Length; i++)
        {
            if (i == (index - 1))
            {
                SatelliteTrans[i].gameObject.SetActive(true);
            }
            else
            {
                SatelliteTrans[i].gameObject.SetActive(isShow);
            }
        }
    }



    #region 计算星下点的

    private int subStarPointCount = 0;


    /// <summary>
    /// 取出时间字符串里的 年月日 时分秒
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    string[] DataTime(string data)
    {
        
        string[] strs = new string[6];
        string[] splits = data.Split(' ');
        string[] nyr = splits[0].Split('/');
        string[] sfm = splits[1].Split(':');
        string[] second = sfm[2].Split('.');
        if (second.Length>1)
        {
            sfm[2] = second[0];
        }
        for (int i = 0; i < strs.Length; i++)
        {
            if (i < 3)
            {
                strs[i] = nyr[i];
            }
            else
                strs[i] = sfm[i - 3];

        }
        return strs;
    }


    /// <summary>
    /// 计算星下点
    /// </summary>
    Vector3 NewCalcluLonLat(float x, float y, float z, string dataTime)
    {
        string[] data = DataTime(dataTime);
        DateTimeInt = data.Select(int.Parse).ToArray();
        return YMD2MJD(x, y, z, data);
    }


    Vector3 YMD2MJD(float x, float y, float z, string[] YMD)
    {
        float YR = float.Parse(YMD[0]);
        float MO = float.Parse(YMD[1]);
        float DY = float.Parse(YMD[2]);
        float Hr = float.Parse(YMD[3]);
        float Mi = float.Parse(YMD[4]);
        float Se = float.Parse(YMD[5]);

        double MJD = DY - 32075 + Fix(1461 * (YR + 4800 + Fix((MO - 14) / 12)) / 4) +
            Fix(367 * (MO - 2 - Fix((MO - 14) / 12) * 12) / 12) -
            Fix(3 * Fix((YR + 4900 + Fix((MO - 14) / 12)) / 100) / 4) - 0.5;
        MJD = MJD + Hr / 24 + Mi / 60 / 24 + Se / 60 / 60 / 24;
        MJD = MJD - 2451545;

        double sg0 = Sqajs(MJD);
        return LonAndLat(x, y, z, sg0);
    }

    double Sqajs(double MJD0)
    {
        double T0 = Fix((float)MJD0) / 36525;
        double Sga_day0 = 2 * Mathf.PI * 0.7790572732640;
        double DSga_day = 2 * Mathf.PI * 1.00273781191135448;
        double Sga_T0 = 0.014506 / 3600 / 180 * Mathf.PI;
        double DSga_T0 = 4612.15739966 / 3600 / 180 * Mathf.PI;
        double DDSga_T0 = 1.39667721 / 3600 / 180 * Mathf.PI;
        double Sga_m1 = Sga_day0 + DSga_day * MJD0 + Sga_T0 + DSga_T0 * T0 + DDSga_T0 * (T0 * T0);
        Sga_m1 = Mod(Sga_m1, 2 * Mathf.PI);
        return Sga_m1;
    }

    /// <summary>
    /// 计算最终的经纬度并显示星下点轨迹 经度是0~360，纬度是-90~90
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="Sg0"></param>
    Vector3 LonAndLat(float x, float y, float z, double Sg0)
    {
        float Temp = Mathf.Atan2(y, x);
        if (y < 0)
        {
            Temp = Temp + 2 * Mathf.PI;
        }

        float Lon = (float)Sg0 + Temp;
        Lon = (float)Mod(Lon, 2 * Mathf.PI);
        if (Lon > 2 * Mathf.PI)
        {
            Lon = Lon - 2 * Mathf.PI;
        }

        Lon = (180 / Mathf.PI) * Lon;
        float Lat = (180 / Mathf.PI) * Mathf.Atan2(z, Mathf.Sqrt(x * x + y * y));

        subStarPointLineRender.positionCount = (subStarPointCount + 1);
        subStarPointLineRender.SetPosition(subStarPointCount, new Vector3((float)Lon, (float)Lat * 2, 0));
        subStarPointCount++;
        return new Vector3(Lon, Lat, 0);
    }

    /// <summary>
    /// 向零方向取整
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    int Fix(float val)
    {
        return UnityEngine.Mathf.RoundToInt(val);
    }


    /// <summary>
    /// 求余函数
    /// </summary>
    /// <returns></returns>
    double Mod(double a, double b)
    {
        return a % b;
    }
    #endregion

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
