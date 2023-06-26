using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 行星数据计算
/// </summary>
public class PlanetDataCalculate
{
    private static PlanetDataCalculate _instance = null;

    public static PlanetDataCalculate GetInstance()
    {
        if (_instance == null)
        {
            _instance = new PlanetDataCalculate();
        }
        return _instance;
    }

    /// <summary>
    /// 经纬度数组
    /// </summary>
    float[] LonAndLat = new float[2];

    /// <summary>
    /// 计算SgaJs
    /// </summary>
    /// <param name="MJD0"></param>
    /// <returns></returns>
    public double CalculateSqajs(double MJD0)
    {
        double T0 = Floor((float)MJD0) / 36525;
        double Sga_day0 = 2 * Mathf.PI * 0.7790572732640;
        double DSga_day = 2 * Mathf.PI * 1.00273781191135448;
        double Sga_T0 = 0.014506 / 3600 / 180 * Mathf.PI;
        double DSga_T0 = 4612.15739966 / 3600 / 180 * Mathf.PI;
        double DDSga_T0 = 1.39667721 / 3600 / 180 * Mathf.PI;
        double Sga_m1 = Sga_day0 + DSga_day * MJD0 + Sga_T0 + DSga_T0 * T0 + DDSga_T0 * (T0 * T0);
        Sga_m1 = Mod(Sga_m1, 2 * Mathf.PI);
        return Sga_m1;
    }


    //计算MjDay
    public double CalculateMjday(int year, int month, int day, int hour, int min, int sec, int totalcount)
    {
        double Mjd = 0.0f;
        if (totalcount < 4)
        {
            hour = 0;
            min = 0;
            sec = 0;
        }
        int y = year;
        int m = month;
        float c = 0;
        float a = 0.0f;
        float b = 0.0f;

        if (m <= 2)
        {
            y = y - 1;
            m = m + 12;
        }
        if (y < 0)
        {
            c = -0.75f;
        }
        if (year < 1582)
        {

        }
        else if (year > 1582)
        {
            a = Fix(y / 100);
            b = 2 - a + Floor(a / 4);
        }
        else if (month < 10)
        {

        }
        else if (month > 10)
        {
            a = Fix(y / 100);
            b = 2 - a + Floor(a / 4);
        }
        else if (day <= 4)
        {

        }
        else if (day > 14)
        {
            a = Fix(y / 100);
            b = 2 - a + Floor(a / 4);
        }
        else
        {
            return 0.0f;
        }
        float jd = Fix((float)365.25 * y + c) + Fix((float)30.6001 * (m + 1));
        jd = jd + day + b + 1720994.5f;
        jd = jd + (hour + min / 60 + sec / 3600) / 24;
        Mjd = jd - 2400000.5;
        return Mjd;
    }


    /// <summary>
    /// 计算YMD到MJD的转换
    /// </summary>
    /// <returns></returns>
    public double CalculateYMD2MJD(string[] YMD)
    {
        double MJD = 0.0f;
        float YR = float.Parse(YMD[0]);
        float MO = float.Parse(YMD[1]);
        float DY = float.Parse(YMD[2]);
        float Hr = float.Parse(YMD[3]);
        float Mi = float.Parse(YMD[4]);
        float Se = float.Parse(YMD[5]);

        MJD = DY - 32075 + Fix(1461 * (YR + 4800 + Fix((MO - 14) / 12)) / 4) +
            Fix(367 * (MO - 2 - Fix((MO - 14) / 12) * 12) / 12) -
            Fix(3 * Fix((YR + 4900 + Fix((MO - 14) / 12)) / 100) / 4) - 0.5;
        MJD = MJD + Hr / 24 + Mi / 60 / 24 + Se / 60 / 60 / 24;
        return MJD;
    }

    /// <summary>
    /// 计算卫星质心和地心的连线与地球表面相交的点的经纬度
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="Sg0"></param>
    /// <returns></returns>
    public float[] CalculateLonAndLat(float x, float y, float z, double Sg0)
    {
        float Temp = Mathf.Atan2(y, x);
        if (y < 0)
        {
            Temp = Temp + 2 * Mathf.PI;
        }

        LonAndLat[0] = (float)Sg0 + Temp;
        LonAndLat[0] = (float)Mod(LonAndLat[0], 2 * Mathf.PI);
        if (LonAndLat[0] > 2 * Mathf.PI)
        {
            LonAndLat[0] = LonAndLat[0] - 2 * Mathf.PI;
        }

        LonAndLat[0] = (180 / Mathf.PI) * LonAndLat[0];
        LonAndLat[1] = (180 / Mathf.PI) * Mathf.Atan2(z, Mathf.Sqrt(x * x + y * y));
        return LonAndLat;
    }

    public Vector3 CalculateCheb3D(double t,int N,double Ta,double Tb,double[] Cx,double[] Cy,double[] Cz)
    {
        Vector3 r_vec;

        if (t<Ta || Tb<t)
        {
            Debug.Log("*&&&&&&&&&&&CalculateCheb3D-Error-t:"+t+ ",,Ta:"+ Ta+ ",,,Tb:"+ Tb+",,N:"+N);
        }
       
        float tau = (float)(2 * t - Ta - Tb) / (float)(Tb - Ta);

        Vector3 f1 = new Vector3(0,0,0);
        Vector3 f2 = new Vector3(0, 0, 0);

        Vector3 old_f1 = new Vector3(0, 0, 0);

        for (int i = N; i >= 2; i--)
        {
            old_f1 = f1;
            f1 = (2 * tau * f1) - f2 + new Vector3((float)Cx[i - 1], (float)Cy[i - 1], (float)Cz[i - 1]);
            f2 = old_f1;
        }
        r_vec = (tau* f1)-f2 + new Vector3((float)Cx[1], (float)Cy[1], (float)Cz[1]);
        return r_vec;
    }

    /// <summary>
    /// 计算给定时间对应的 UTC 时间
    /// </summary>
    /// <param name="julianDate"></param>
    /// <returns></returns>
    public DateTime CalculateUTCTime(double julianDate)
    {
        double totalSeconds = (julianDate - 2440587.5) * 86400d;
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(totalSeconds);
        return dateTime;
    }


    /// <summary>
    /// 分割成需要用到的时间
    /// </summary>
    /// <returns></returns>
    public string[] SpliTime(string data)
    {
        string[] strTimes = new string[6];
        string[] splits = data.Split(' ');
        string[] nyr = splits[0].Split('/');
        string[] sfm = splits[1].Split(':');
        string[] second = sfm[2].Split('.');
        if (second.Length > 1)
        {
            sfm[2] = second[0];
        }
        for (int i = 0; i < strTimes.Length; i++)
        {
            if (i < 3)
            {
                strTimes[i] = nyr[i];
            }
            else
                strTimes[i] = sfm[i - 3];

        }
        return strTimes;
    }

    /// <summary>
    /// 向下取整
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    int Floor(float a)
    {
        return Mathf.FloorToInt(a);
    }


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
}
