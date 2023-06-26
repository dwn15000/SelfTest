using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Draw2DUILine : MonoBehaviour {

    public Color colorLine1 = Color.white;
    public List<Vector2> listPoints = new List<Vector2>();

    public Color32 bgColor = Color.white;
    public Color32 zeroColor = Color.black;

    [SerializeField]
    private RawImage bgImage;

    [SerializeField]
    private float height = 0.34f;
    [SerializeField]
    private float width=0.35f;

    private Texture2D bgTexture;
    private int widthPixels;
    private int heightPixels;

    private Color32[] pixelsBg;
    private Color32[] pixelsDrawLine;

    public float semiMajorAxis = 10000f;  // 半长轴
    public float eccentricity = 0.1f;     // 偏心率
    public float inclination = 45f;       // 轨道倾角
    public float rightAscension = 0f;     // 升交点赤经
    public float argumentOfPeriapsis = 0f; // 近地点幅角
    public float period = 2f * Mathf.PI;   // 轨道周期
    private float trueAnomaly;    // 真近点角
    private float meanAnomaly;    // 平近点角
    private float eccentricAnomaly;  // 离心近点角
    private float x, y, z;        // 卫星在空间坐标系中的位置
    private float latitude, longitude, altitude;  // 卫星在地球表面上的经纬度和高度
    private float earthRadius = 6371f;  // 地球半径
    // 星下点轨迹的线段
    private LineRenderer lineRenderer;
    // 星下点轨迹的点
    private Vector3[] subPoints = new Vector3[100];
    // 星下点轨迹的点数
    private int subPointCount = 0;
    // 地球图底图
    public Image earthImage;

    List<Vector3> allPos = new List<Vector3>();


    Vector3[] points;


    private void Awake()
    {
        IocContainer_InstanceMgr.GetInstance().RegistInstance(this);
    }


    // Use this for initialization
    void Start () {
        CalculData(10678.138253, 3.192319, 5.207114, "2023/2/24 4:00:01");

        TimeSpan timeSpan = (new DateTime(2023, 2, 24, 04, 0, 0) - new DateTime(1970, 1, 1, 0, 0, 0));
        int timeStamp = ((int)timeSpan.TotalSeconds);
        Debug.Log("转换后的日期:" + timeStamp);
        string ss = new DateTime(1970, 01, 01, 00, 00, 00).AddSeconds(timeStamp).ToString("yyyy/MM/dd HH:mm:ss");

        Debug.Log("转换回去的日期:" + ss);




        //int result1 = 1694668801; // 假设已经得到了以秒为单位的整型数
        //long ticks1 = result1 * TimeSpan.TicksPerSecond; // 将以秒为单位的整型数转换为以Ticks为单位的长整型数
        //DateTime dt1 = new DateTime(ticks1); // 将长整型数转换为DateTime类型
        //string str1 = dt1.ToString("yyyy/M/d HH:mm:ss"); // 将DateTime类型转换为字符串



       // points = GizmodoTest();
       // //创建背景贴图
       // widthPixels = (int)(Screen.width * width);
       // heightPixels = (int)(Screen.height * height);
       //bgTexture = new Texture2D(widthPixels, heightPixels);
     
       // bgImage.texture = bgTexture;
       // bgImage.SetNativeSize();


       // bgImage.rectTransform.anchorMin = Vector2.zero;
       // bgImage.rectTransform.anchorMax = Vector2.zero;
       // bgImage.rectTransform.pivot = Vector2.zero;
       // bgImage.rectTransform.sizeDelta = new Vector2(widthPixels, heightPixels);


       // pixelsDrawLine = new Color32[widthPixels * heightPixels];
       // pixelsBg = new Color32[widthPixels * heightPixels];

       // for (int i = 0; i < pixelsBg.Length; ++i)
       // {
       //     pixelsBg[i] = bgColor;
       // }
    }

    Vector3 pos;

    Vector3 backPos = Vector3.zero;
    bool isCall = false;
	// Update is called once per frame
	void Update () {
        // Clear.
        //Array.Copy(pixelsBg, pixelsDrawLine, pixelsBg.Length);

        //// 基准线
        ////DrawLine(new Vector2(0f, heightPixels * 0.5f), new Vector2(widthPixels, heightPixels * 0.5f), zeroColor);
        ////for (int i = 0; i < listPoints.Count - 1; i++)
        ////{
        ////    Vector2 from = listPoints[i];
        ////    Vector2 to = listPoints[i + 1];
        ////    DrawLine(from, to, colorLine1);
        ////}

        //if (isCall)
        //{
        //    return;
        //}
        //isCall = true;
        //Debug.LogError("点的集合长度:"+points.Length);
        //for (int i = 0; i < points.Length - 1; i++)
        //{
        //    //if (i>20)
        //    //{
        //    //    break;
        //    //}
        //    Vector2 from = points[i];
        //    Vector2 to = points[i + 1];
        //    DrawLine(from, to, colorLine1);
        //}

        ////pos  = UpdateTest();
        ////allPos.Add(pos);
        ////for (int i = 0; i < allPos.Count - 1; i++)
        ////{
        ////    Vector2 from = allPos[i];
        ////    Vector2 to = allPos[i + 1];
        ////    DrawLine(from, to, colorLine1);
        ////}
        ////DrawLine(backPos, pos, colorLine1);

        //bgTexture.SetPixels32(pixelsDrawLine);
        //bgTexture.Apply();


    }



    Vector3 currPos = Vector3.zero;
    Vector3 UpdateTest()
    {
        float time = Time.timeSinceLevelLoad;
        // 计算平近点角
        meanAnomaly = (time / period) * 2f * Mathf.PI;
        // 使用牛顿迭代法计算离心近点角
        float epsilon = 0.001f;
        float f, fp, delta;
        eccentricAnomaly = meanAnomaly;
        do
        {
            f = eccentricAnomaly - eccentricity * Mathf.Sin(eccentricAnomaly) - meanAnomaly;
            fp = 1f - eccentricity * Mathf.Cos(eccentricAnomaly);
            delta = f / fp;
            eccentricAnomaly -= delta;
        }
        while (Mathf.Abs(delta) > epsilon);
        // 计算真近点角
        trueAnomaly = 2f * Mathf.Atan(Mathf.Sqrt((1f + eccentricity) / (1f - eccentricity)) * Mathf.Tan(0.5f * eccentricAnomaly));
        // 计算卫星在轨道平面上的位置
        float r = semiMajorAxis * (1f - eccentricity * Mathf.Cos(eccentricAnomaly));
        float x0 = r * Mathf.Cos(trueAnomaly);
        float y0 = r * Mathf.Sin(trueAnomaly);
        // 计算卫星在空间坐标系中的位置
        float cosRA = Mathf.Cos(rightAscension);
        float sinRA = Mathf.Sin(rightAscension);
        float cosI = Mathf.Cos(inclination);
        float sinI = Mathf.Sin(inclination);
        x = x0 * cosRA - y0 * sinRA * cosI;
        y = x0 * sinRA + y0 * cosRA * cosI;
        z = y0 * sinI;
        // 计算卫星在地球表面上的经纬度和高度
        longitude = Mathf.Atan2(y, x);
        latitude = Mathf.Atan2(z, Mathf.Sqrt(x * x + y * y));
        altitude = Mathf.Sqrt(x * x + y * y + z * z) - earthRadius;

        currPos.x = longitude;
        currPos.y = latitude;
        // 计算星下点位置
        //Vector3 subPoint = new Vector3(
        //    earthRadius * Mathf.Cos(latitude) * Mathf.Cos(longitude),
        //    earthRadius * Mathf.Cos(latitude) * Mathf.Sin(longitude),
        //    earthRadius * Mathf.Sin(latitude));
        // 更新RectTransform对象的位置
        //RectTransform rectTransform = GetComponent<RectTransform>();
        //Vector2 position = new Vector2(
        //    (subPoint.x / earthRadius + 1f) * 0.5f * earthImage.rectTransform.rect.width,
        //    (subPoint.y / earthRadius + 1f) * 0.5f * earthImage.rectTransform.rect.height);
        //rectTransform.anchoredPosition = position;
        // 更新星下点轨迹
        //if (subPointCount < subPoints.Length)
        //{
        //    subPoints[subPointCount] = subPoint;
        //    subPointCount++;
        //}
        return currPos;
        //lineRenderer.positionCount = subPointCount;
        //lineRenderer.SetPositions(subPoints);
    }



    void DrawLine(Vector2 from, Vector2 to, Color32 color)
    {
        int i;
        int j;

        from.y = from.y * 100;
        from.x = from.x * 100;

        to.y = to.y * 100;
        to.x = to.x * 100;

        Debug.Log("开始的位置:" + from + ",,结束的位置:" + to);
        if (Mathf.Abs(to.x - from.x) > Mathf.Abs(to.y - from.y))
        {
            // Horizontal line.
            i = 0;
            j = 1;
        }
        else
        {
            // Vertical line.
            i = 1;
            j = 0;
        }

        int x = (int)from[i];
        int delta = (int)Mathf.Sign(to[i] - from[i]);
        while (x != (int)to[i])
        {
            int y = (int)Mathf.Round(from[j] + (x - from[i]) * (to[j] - from[j]) / (to[i] - from[i]));

            int index;
            if (i == 0)
                index = y * widthPixels + x;
            else
                index = x * widthPixels + y;

            index = Mathf.Clamp(index, 0, pixelsDrawLine.Length - 1);
            pixelsDrawLine[index] = color;

            x += delta;
        }
    }

    public float amplitude = 1f; // 振幅
    public float frequency = 1f; // 频率
    public float phase = 0f; // 相位
    public float xOffset = 0f; // x轴偏移量
    public float yOffset = 0f; // y轴偏移量
    public int resolution = 100; // 曲线的分辨率

    Vector3[] GizmodoTest()
    {
        Vector3[] points = new Vector3[resolution];
        for (int i = 0; i < resolution; i++)
        {
            float t = (float)i / (float)(resolution - 1);
            float x = t * 10f + xOffset;
            float y = amplitude * Mathf.Sin(2f * Mathf.PI * frequency * t + phase) + yOffset;
            points[i] = new Vector3(x, y, 0f);
        }
        return points;
    }

    void CalculData(double x, double y, double z,string dataTime)
    {
        //输入年月日时分秒
        string[] data = DataTime(dataTime);

        int YR = int.Parse(data[0]), MO = int.Parse(data[1]), DY = int.Parse(data[2]), Hr = int.Parse(data[3]), Mi = int.Parse(data[4]), Se = int.Parse(data[5]);

        Debug.Log(string.Format("分割完的时间-YR:{0},MO:{1},DY:{2},Hr:{3},Mi:{4},Se:{5}",YR,MO,DY,Hr,Mi,Se));
       double MJD = DY - 32075 + (int)MathF.Floor(1461 * (YR + 4800 + (int)MathF.Floor((MO - 14) / 12)) / 4) + (int)MathF.Floor(367 * (MO - 2 - (int)MathF.Floor((MO - 14) / 12) * 12) / 12) - (int)MathF.Floor(3 * (int)MathF.Floor((YR + 4900 + (int)MathF.Floor((MO - 14) / 12)) / 100) / 4) - 0.5;
        MJD = MJD + Hr / 24 + Mi / 60 / 24 + Se / 60 / 60 / 24;

        double Sga_T1 = (100.4606184 * Math.PI / 180);
        double DSga_T1 = (36000.77004 * Math.PI / 180);
        double DSga_day1 = (360.98564724 * Math.PI / 180);

        double T1 = MJD/365.25;
 

        double Sga_m2 = Sga_T1 + DSga_T1 * T1 + Mod(MJD - 0.5, 1) * DSga_day1;
        double Sg0 = Mod(Sga_m2, 2 * Math.PI) * 180 / Math.PI;

        // lon 经度（-180到180）    lat 纬度(-90到90)
        float temp = Mathf.Atan2((float)y, (float)z);
        if (y<0)
        {
            temp = temp + (float)(2 * Math.PI);
        }
        double lon = Sg0 + temp;
        lon = Mod(lon,2*Mathf.PI);
        if (lon>2*Mathf.PI)
        {
            lon = lon - 2 * Mathf.PI;
        }
        
        lon = (180/Mathf.PI)*lon;
        double lat = (180/Mathf.PI)* Mathf.Atan2((float)z, Mathf.Sqrt((float)(x*x+y*y)));
        Debug.Log("得到的经纬度:"+lon+",,lat="+lat);
    }


    string[] DataTime(string data)
    {
        string[] strs = new string[6];
        string[] splits = data.Split(' ');
        string[] nyr = splits[0].Split('/');
        string[] sfm = splits[1].Split(':');
        string[] second = sfm[2].Split('.');
        //Debug.Log("时间选项的:" + data+",,,,,second:"+ second.Length);
        if (second.Length > 1)
        {
            sfm[2] = second[0];
        }
        for (int i = 0; i < strs.Length; i++)
        {
            if (i<3)
            {
                strs[i] = nyr[i];
            }
           else
                strs[i] = sfm[i-3];

        }
        return strs;
    }


    /// <summary>
    /// 求余函数
    /// </summary>
    /// <returns></returns>
    double Mod(double a,double b)
    {
        return a % b;
    }
}
