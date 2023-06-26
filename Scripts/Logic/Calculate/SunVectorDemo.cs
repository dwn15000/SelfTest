using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SunVectorDemo : MonoBehaviour
{
    public float sunDistance = 149.6f; // ̫��������ľ��루��λ�������
    public float daysSinceJ2000 = 0; // ��ǰʱ����J2000ʱ��֮�������
    void Start()
    {
        daysSinceJ2000 = (float)DaysSinceJ2000s(DateTime.UtcNow);
        // ����̫������ʸ��
        Vector3 sunDirection = CalculateSunVector();
        Debug.Log("Sun Direction: " + sunDirection);
        QuZheng();
        int start = 231;
        int end = 270;
        int step = 13;
        var range = Enumerable.Range(0, (end - start) / step + 1)
                              .Select(i => start + i * step);
        int[] result = range.ToArray();
        for (int i = 0; i < result.Length; i++)
        {
            Debug.Log("��ӡ�����ֵ: " + result[i]);
        }
    }

    void QuZheng()
    {
        float a = 3.8f;
        Debug.Log(Mathf.CeilToInt(a)); // ����ȡ��
        a = 3.8f;
        Debug.Log(Mathf.FloorToInt(a)); // ����ȡ��
        a = 3.8f;
        Debug.Log(Mathf.RoundToInt(a)); // ���㷽��ȡ��
    }
    Vector3 CalculateSunVector()
    {
        // ����̫���ĻƵ�����
        float meanLongitude = (280.460f + 0.9856474f * daysSinceJ2000) % 360;
        float anomaly = 357.528f + 0.9856003f * daysSinceJ2000;
        float eclipticLongitude = meanLongitude + 1.915f * Mathf.Sin(anomaly * Mathf.Deg2Rad) + 0.02f * Mathf.Sin(2 * anomaly * Mathf.Deg2Rad);
        float eclipticLatitude = 0;
        // ת��Ϊ�ѿ�������ϵ�е�λ��ʸ��
        float x = Mathf.Cos(eclipticLongitude * Mathf.Deg2Rad);
        float y = 0;
        float z = Mathf.Sin(eclipticLongitude * Mathf.Deg2Rad);
        Vector3 sunPosition = new Vector3(x, y, z) * sunDistance;
        // ת��Ϊ��������ϵ
        Vector3 up = transform.up;
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Matrix4x4 matrix = new Matrix4x4();
        matrix.SetColumn(0, right);
        matrix.SetColumn(1, up);
        matrix.SetColumn(2, forward);
        Vector3 sunPositionSatellite = matrix.inverse.MultiplyPoint(sunPosition);
        // ����̫������ʸ��
        Vector3 satellitePosition = transform.position;
        Vector3 sunDirection = (sunPositionSatellite - satellitePosition).normalized;
        return sunDirection;
    }

    private   DateTime J2000 = new DateTime(2000, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    public  double DaysSinceJ2000s(DateTime dateTime)
    {
        TimeSpan span = dateTime.ToUniversalTime() - J2000;
        return span.TotalDays;
    }

}
