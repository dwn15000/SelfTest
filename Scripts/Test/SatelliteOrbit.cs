using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SatelliteOrbit : MonoBehaviour
{
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
    void Start()
    {
        // 创建LineRenderer组件，用于绘制轨迹
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();// gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.positionCount = 0;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        // 初始化星下点轨迹的点
        for (int i = 0; i < subPoints.Length; i++)
        {
            subPoints[i] = Vector3.zero;
        }
    }

    Vector3 pos;
    void Update()
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
        // 计算星下点位置
        Vector3 subPoint = new Vector3(
            earthRadius * Mathf.Cos(latitude) * Mathf.Cos(longitude),
            earthRadius * Mathf.Cos(latitude) * Mathf.Sin(longitude),
            earthRadius * Mathf.Sin(latitude));
        // 更新RectTransform对象的位置
        
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 position = new Vector2(
            (subPoint.x / earthRadius + 1f) * 0.5f * earthImage.rectTransform.rect.width,
            (subPoint.y / earthRadius + 1f) * 0.5f * earthImage.rectTransform.rect.height);
        rectTransform.anchoredPosition = position;
        // 更新星下点轨迹
        //if (subPointCount < subPoints.Length)
        //{
        //    subPoints[subPointCount] = subPoint;
        //    subPointCount++;
        //}
        
        lineRenderer.positionCount = subPointCount+1;
        pos = new Vector3(longitude, latitude, 0);
        Debug.Log("更新RectTransform对象的位置:" + pos+ ",,,,subPointCount="+ subPointCount);
        //lineRenderer.SetPositions(subPoints);
        lineRenderer.SetPosition(subPointCount, pos);

        subPointCount++;
    }
}
