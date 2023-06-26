using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateSubStarPoint
{

    private static CalculateSubStarPoint _instance = null;

    public static CalculateSubStarPoint GetInstance()
    {
        if (_instance == null)
        {
            _instance = new CalculateSubStarPoint();
        }
        return _instance;
    }

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
        if (second.Length > 1)
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
    /// 计算星下点经纬度
    /// </summary>
    public Vector3 NewCalcluLonLat(float x, float y, float z, int[] dataTime)
    {
        //string[] data = DataTime(dataTime);
        return YMD2MJD(x, y, z, dataTime);
    }


    Vector3 YMD2MJD(float x, float y, float z, int[] YMD)
    {
        float YR = YMD[0];
        float MO = YMD[1];
        float DY = YMD[2];
        float Hr = YMD[3];
        float Mi = YMD[4];
        float Se = YMD[5];

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
    /// 计算最终的经纬度并显示太阳矢量星下点轨迹 经度是0~360，纬度是-90~90
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

        return new Vector3((float)Lon, (float)Lat, 0);
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
}
