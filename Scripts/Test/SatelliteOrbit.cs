using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SatelliteOrbit : MonoBehaviour
{
    public float semiMajorAxis = 10000f;  // �볤��
    public float eccentricity = 0.1f;     // ƫ����
    public float inclination = 45f;       // ������
    public float rightAscension = 0f;     // ������ྭ
    public float argumentOfPeriapsis = 0f; // ���ص����
    public float period = 2f * Mathf.PI;   // �������
    private float trueAnomaly;    // ������
    private float meanAnomaly;    // ƽ�����
    private float eccentricAnomaly;  // ���Ľ����
    private float x, y, z;        // �����ڿռ�����ϵ�е�λ��
    private float latitude, longitude, altitude;  // �����ڵ�������ϵľ�γ�Ⱥ͸߶�
    private float earthRadius = 6371f;  // ����뾶
    // ���µ�켣���߶�
    private LineRenderer lineRenderer;
    // ���µ�켣�ĵ�
    private Vector3[] subPoints = new Vector3[100];
    // ���µ�켣�ĵ���
    private int subPointCount = 0;
    // ����ͼ��ͼ
    public Image earthImage;
    void Start()
    {
        // ����LineRenderer��������ڻ��ƹ켣
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();// gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.positionCount = 0;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        // ��ʼ�����µ�켣�ĵ�
        for (int i = 0; i < subPoints.Length; i++)
        {
            subPoints[i] = Vector3.zero;
        }
    }

    Vector3 pos;
    void Update()
    {
        float time = Time.timeSinceLevelLoad;
        // ����ƽ�����
        meanAnomaly = (time / period) * 2f * Mathf.PI;
        // ʹ��ţ�ٵ������������Ľ����
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
        // ����������
        trueAnomaly = 2f * Mathf.Atan(Mathf.Sqrt((1f + eccentricity) / (1f - eccentricity)) * Mathf.Tan(0.5f * eccentricAnomaly));
        // ���������ڹ��ƽ���ϵ�λ��
        float r = semiMajorAxis * (1f - eccentricity * Mathf.Cos(eccentricAnomaly));
        float x0 = r * Mathf.Cos(trueAnomaly);
        float y0 = r * Mathf.Sin(trueAnomaly);
        // ���������ڿռ�����ϵ�е�λ��
        float cosRA = Mathf.Cos(rightAscension);
        float sinRA = Mathf.Sin(rightAscension);
        float cosI = Mathf.Cos(inclination);
        float sinI = Mathf.Sin(inclination);
        x = x0 * cosRA - y0 * sinRA * cosI;
        y = x0 * sinRA + y0 * cosRA * cosI;
        z = y0 * sinI;
        // ���������ڵ�������ϵľ�γ�Ⱥ͸߶�
        longitude = Mathf.Atan2(y, x);
        latitude = Mathf.Atan2(z, Mathf.Sqrt(x * x + y * y));
        altitude = Mathf.Sqrt(x * x + y * y + z * z) - earthRadius;
        // �������µ�λ��
        Vector3 subPoint = new Vector3(
            earthRadius * Mathf.Cos(latitude) * Mathf.Cos(longitude),
            earthRadius * Mathf.Cos(latitude) * Mathf.Sin(longitude),
            earthRadius * Mathf.Sin(latitude));
        // ����RectTransform�����λ��
        
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 position = new Vector2(
            (subPoint.x / earthRadius + 1f) * 0.5f * earthImage.rectTransform.rect.width,
            (subPoint.y / earthRadius + 1f) * 0.5f * earthImage.rectTransform.rect.height);
        rectTransform.anchoredPosition = position;
        // �������µ�켣
        //if (subPointCount < subPoints.Length)
        //{
        //    subPoints[subPointCount] = subPoint;
        //    subPointCount++;
        //}
        
        lineRenderer.positionCount = subPointCount+1;
        pos = new Vector3(longitude, latitude, 0);
        Debug.Log("����RectTransform�����λ��:" + pos+ ",,,,subPointCount="+ subPointCount);
        //lineRenderer.SetPositions(subPoints);
        lineRenderer.SetPosition(subPointCount, pos);

        subPointCount++;
    }
}
