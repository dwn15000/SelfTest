using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using OfficeOpenXml;  //引入EPPlus
using UnityEngine;

/// <summary>
/// 太阳系星历控制,在Unity中用EPPlus读取DE430Coeff.xlsx星历系数文件,赋值星历系数给全局变量,计算给定时间对应的UTC时间,
/// 计算输出ICRF参考系下的各天体的位置（单位：米）,计算太阳中心地球黄道坐标系下的各天体矢量，以及地球自转轴和对应时间的地球本初子午线
/// </summary>
public class SolarSystemEphemerisCtrl : MonoBehaviour  //DE430Calculator
{
    // 定义全局变量
    private double[,] coeffArray = new double[2285, 1020];
    double[] earthRotationAxis = new double[3];
    double greenwichMeanSiderealTime;
    public ExcelWorksheet worksheet;

    /// <summary>
    /// 挂载显示主界面的obj
    /// </summary>
    public GameObject MainObj;

    /// <summary>
    /// 资源加载提示提示obj
    /// </summary>
    public GameObject TipObj;

    /// <summary>
    /// 总行数
    /// </summary>
    int totalRow = 2285;

    /// <summary>
    /// 当前行数
    /// </summary>
    int currRow = 0;
    private void Awake()
    {
        IocContainer_InstanceMgr.GetInstance().RegistInstance(this);
    }


    void Start()
    {
        // 定义文件路径
        //string filePath = Application.dataPath + "/DE430Coeff.xlsx";

        string exePath = Path.GetDirectoryName(Application.dataPath);
        string filePath = Path.Combine(exePath, "config/DE430Coeff.xlsx");


        // 读取Excel文件
        FileInfo file = new FileInfo(filePath);
        ExcelPackage excel = new ExcelPackage(file);
         worksheet = excel.Workbook.Worksheets[1];

        // 循环读取Excel中的星历系数并赋值给全局变量
        for (int i = 1; i <= 2285; i++)
        {
            List<double> list = new List<double>();
            for (int j = 1; j <= 1020; j++)
            {
                list.Add(worksheet.Cells[i, j].GetValue<double>());
                JPLEphDe430.GetInstance().PCList.Add(list);
                //coeffArray[i - 1, j - 1] = worksheet.Cells[i, j].GetValue<double>();
            }
        }
        MainObj.SetActive(true);
        TipObj.SetActive(false);
        //Test_JPL430eph();
    }

   
    /// <summary>
    /// 测试计算各行星天体相对太阳矢量(在黄道坐标系）(单位：米))
    /// </summary>
    public Vector3 Test_JPL430eph(int[] Time)
    {
        //{ 2023, 4, 15, 4, 0, 0 };
        //int[] Time = new int[6] { dateTime[0], m, d, h, min, second };
        double Mjd_UTC = PlanetDataCalculate.GetInstance().CalculateMjday(Time[0], Time[1], Time[2], Time[3], Time[4], Time[5], 6);

        //planetVec数组依次对应： r_Mercury, r_Venus, r_Earth, r_Mars, r_Jupiter, r_Saturn, r_Uranus, r_Neptune, r_Pluto, r_Moon, r_Sun
        Vector3[] planetVec = new Vector3[11];
        JPLEphDe430.GetInstance().CalculatePlanetVector(Mjd_UTC, ref planetVec);

        string[] startTime = System.Array.ConvertAll<int, string>(Time, x => x.ToString());
        string[] ToTime = new string[6] { "2000", "1", "1", "12", "0", "0" };

        double MJD = PlanetDataCalculate.GetInstance().CalculateYMD2MJD(startTime) - PlanetDataCalculate.GetInstance().CalculateYMD2MJD(ToTime);
        double T1 = (1 / 36525) * (MJD);
        double angle_HCJJ = -(23.439302222 - 0.0130041667 * T1) * Mathf.PI / 180;
        Matrix4x4 C = DefineMatrix((float)angle_HCJJ);
        Vector3 r_Mercury_N = MatrixGetNewVector3(C, planetVec, 0);
        Vector3 r_Venus_N = MatrixGetNewVector3(C, planetVec, 1);
        Vector3 r_Earth_N = C * (planetVec[2]);
        Vector3 r_Mars_N = MatrixGetNewVector3(C, planetVec, 3);
        Vector3 r_Jupiter_N = MatrixGetNewVector3(C, planetVec, 4);
        Vector3 r_Saturn_N = MatrixGetNewVector3(C, planetVec, 5);
        Vector3 r_Uranus_N = MatrixGetNewVector3(C, planetVec, 6);
        Vector3 r_Neptune_N = MatrixGetNewVector3(C, planetVec, 7);
        Vector3 r_Pluto_N = MatrixGetNewVector3(C, planetVec, 8);

        Vector3 EZ = new Vector3(0, Mathf.Sign((float)-angle_HCJJ), Mathf.Cos((float)-angle_HCJJ));
        double Sga_m1 = PlanetDataCalculate.GetInstance().CalculateSqajs(MJD);

        Vector3 newRun = -r_Earth_N;
        Vector3 pos =  CalculateSubStarPoint.GetInstance().NewCalcluLonLat(newRun.x, newRun.y, newRun.z, Time);
        //Debug.Log("太阳光照信息-pos:" + pos+",,oldPos:"+newRun);
        return pos;
    }

    /// <summary>
    /// 矩阵乘以向量得到一个新的Vector3
    /// </summary>
    /// <returns></returns>
    Vector3 MatrixGetNewVector3(Matrix4x4 C, Vector3[] planetVec, int idx)
    {
        return C * (planetVec[idx] + planetVec[2] + (planetVec[10] + planetVec[2]));
    }


    /// <summary>
    /// 得到一个转置矩阵
    /// </summary>
    /// <param name="angle_HCJJ"></param>
    /// <returns></returns>
    Matrix4x4 DefineMatrix(float angle_HCJJ)
    {
        Matrix4x4 matrix = new Matrix4x4();
        matrix.m00 = 1; matrix.m01 = 0; matrix.m02 = 0;
        matrix.m10 = 0; matrix.m11 = Mathf.Cos(-angle_HCJJ); matrix.m12 = Mathf.Sign(-angle_HCJJ);
        matrix.m20 = 0; matrix.m21 = -Mathf.Sign(-angle_HCJJ); matrix.m22 = Mathf.Cos(-angle_HCJJ);
        return matrix;
    }
}