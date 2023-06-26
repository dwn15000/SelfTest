using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallJPL430eph : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    /// <summary>
    /// 加载DE430Coeff文件(这个方法周说他实现了)
    /// </summary>
    void LoadDE430CoeffFile()
    {
        
    }

    /// <summary>
    /// 测试计算各行星天体相对太阳矢量(在黄道坐标系）(单位：米))
    /// </summary>
    void Test_JPL430eph()
    {
        int[] Time = new int[6] {2049,8,29,4,0,0};
        double Mjd_UTC = PlanetDataCalculate.GetInstance().CalculateMjday(Time[0], Time[1], Time[2], Time[3], Time[4], Time[5],6);

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
        Vector3 r_Saturn_N = MatrixGetNewVector3(C,planetVec,5);
        Vector3 r_Uranus_N = MatrixGetNewVector3(C, planetVec, 6);
        Vector3 r_Neptune_N = MatrixGetNewVector3(C, planetVec, 7);
        Vector3 r_Pluto_N = MatrixGetNewVector3(C, planetVec, 8);

        Vector3 EZ = new Vector3(0, Mathf.Sign((float)-angle_HCJJ), Mathf.Cos((float)-angle_HCJJ));
        double Sga_m1 = PlanetDataCalculate.GetInstance().CalculateSqajs(MJD);
    }

    /// <summary>
    /// 矩阵乘以向量得到一个新的Vector3
    /// </summary>
    /// <returns></returns>
    Vector3 MatrixGetNewVector3(Matrix4x4 C,Vector3[] planetVec,int idx)
    {
       return  C * (planetVec[idx] + planetVec[2] + (planetVec[10] + planetVec[2]));
    }

    /// <summary>
    /// 得到一个转置矩阵
    /// </summary>
    /// <param name="angle_HCJJ"></param>
    /// <returns></returns>
    Matrix4x4 DefineMatrix(float angle_HCJJ)
    {
        Matrix4x4 matrix = new Matrix4x4();
        matrix.m00 = 1; matrix.m01 = 0;                         matrix.m02 = 0;                       
        matrix.m10 = 0; matrix.m11 = Mathf.Cos(-angle_HCJJ);    matrix.m12 = Mathf.Sign(-angle_HCJJ); 
        matrix.m20 = 0; matrix.m21 = -Mathf.Sign(-angle_HCJJ);  matrix.m22 = Mathf.Cos(-angle_HCJJ);                    
        return matrix;
    }
}
