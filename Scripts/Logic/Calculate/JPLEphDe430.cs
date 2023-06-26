using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
/*
 * JPL_Eph_DE430������̫���������;ſ���Ҫ���ǵĳ��

%ʹ��JPL������λ��

%

%���룺

%Mjd_TDB�޸�TDB����������

%

%�����

%r_����̫��ϵ���ģ�SSB������r_ˮ�ǡ�r_���ǡ�r_���ǣ�

%r_ľ�ǣ�r_���ǣ�r_�����ǣ�r_�����ǣ�r_ڤ���ǣ�r_������

%r_Sun�����ĳ��λ�ã�[m]��ָ

%��������ο�ϵ

%

%ע�⣺�ѽ�����ʱ�俼������

%

%�ϴ��޸�ʱ�䣺2023/06/14 
*/

/// <summary>
/// JPL-DE430������������ʵ�֣�ʹ��C#ʵ�֣���NASA JPL Development Ephemerides (DE430)
/// </summary>
public class JPLEphDe430
{
    private static JPLEphDe430 _instance = null;

    public static JPLEphDe430 GetInstance()
    {
        if (_instance == null)
        {
            _instance = new JPLEphDe430();
        }
        return _instance;
    }

    /// <summary>
    /// �洢���ڶ�ȡDE340Coeff���ݱ�
    /// </summary>
    public List<List<double>> PCList = new List<List<double>>();

    /// <summary>
    /// �к�����
    /// </summary>
    int RowIndex = 0;
    /// <summary>
    /// ȡ�����յ��к�
    /// </summary>
    int ResultRow = -1;

    /// <summary>
    /// ��������ʸ������ r_Mercury,r_Venus,r_Earth,r_Mars,r_Jupiter,r_Saturn,r_Uranus,r_Neptune,r_Pluto,r_Moon,r_Sun
    /// </summary>
    /// <param name="Mjd_TDB"></param>
    /// <param name="planetVec"></param>
    public void CalculatePlanetVector(double Mjd_TDB,ref UnityEngine.Vector3[] planetVec)
    {
        double JD = Mjd_TDB + 2400000.5f;
        int row = FindPCRow(JD);
        if (row<0)
        {
            return;
        }
        List<double> PCtemps = PCList[row];

        double[] PCtemp = PCtemps.ToArray();

        double t1 = PCtemp[0] - 2400000.5;
        double dt = Mjd_TDB - t1;

        int[] temp = new int[4];

        #region  1.�������ģ��
        int start = 231;
        int end = 270;
        int step = 13;
        EqualDifferenceIncrement(ref temp,start,end,step);

        #region ��������XYZʸ��
        ///�����Xʸ��
        double[] CX_Earth = PCtempFunc(temp,PCtemp,0,1);

        ///�����Yʸ��
        double[] CY_Earth = PCtempFunc(temp, PCtemp, 1, 2);

        ///�����Zʸ��
        double[] CZ_Earth = PCtempFunc(temp, PCtemp, 2, 3);
        #endregion

        ///��temp�е�ÿ��ֵ������39
        TempAdd(ref temp,39);

        #region ���� X Y Z
        ///Xʸ��
        double[] Cx = PCtempFunc(temp, PCtemp, 0, 1);

        ///Yʸ��
        double[] Cy = PCtempFunc(temp, PCtemp, 1, 2);

        ///Zʸ��
        double[] Cz = PCtempFunc(temp, PCtemp, 2, 3);
        #endregion

        double[] New_Cx_Earth = CX_Earth.Concat(Cx).ToArray();
        double[] New_Cy_Earth = CY_Earth.Concat(Cy).ToArray();
        double[] New_Cz_Earth = CZ_Earth.Concat(Cx).ToArray();
        CX_Earth = New_Cx_Earth;
        CY_Earth = New_Cy_Earth;
        CZ_Earth = New_Cz_Earth;
        int j = -1;
        double Mjd0 = 0.0f;
        if (0 <= dt && dt <= 16)
        {
            j = 0;
            Mjd0 = t1;
        }
        else if (16 < dt && dt <= 32)
        {
            j = 1;
            Mjd0 = t1 + 16 * j;
        }

        //��Ҫ�����õ������ݸ���,��1�α���13��,����
        int x_erathCount = (13 * j + 13) - (13 * j + 1);

        New_Cx_Earth = DataFetch((13 * j + 1), (13 * j + 13),CX_Earth);
 
        New_Cy_Earth = DataFetch((13 * j + 1), (13 * j + 13), CY_Earth);
       
        New_Cz_Earth = DataFetch((13 * j + 1), (13 * j + 13), CZ_Earth);

        planetVec[2] = PlanetDataCalculate.GetInstance().CalculateCheb3D(Mjd_TDB, 13, Mjd0, Mjd0 + 16, New_Cx_Earth, New_Cy_Earth, New_Cz_Earth);

        #endregion

        #region  2.��������ģ��
        start = 441;
        end = 480;
        step = 13;
        EqualDifferenceIncrement(ref temp, start, end, step);

        //���������д�ģ�
        double[] Cx_Moon = PCtempFunc(temp, PCtemp, 0, 1);

        double[] Cy_Moon = PCtempFunc(temp, PCtemp, 1, 2);

        double[] Cz_Moon = PCtempFunc(temp, PCtemp, 2, 3);

        for (int i = 0; i < 7; i++)
        {
            ///��temp�е�ÿ��ֵ������39
            TempAdd(ref temp, 39);

            Cx = PCtempFunc(temp, PCtemp, 0, 1);

            Cy = PCtempFunc(temp, PCtemp, 1, 2);

            Cz = PCtempFunc(temp, PCtemp, 2, 3);

            Cx_Moon = Cx_Moon.Concat(Cx).ToArray();
            Cy_Moon = Cy_Moon.Concat(Cy).ToArray();
            Cz_Moon = Cz_Moon.Concat(Cz).ToArray();
        }

        if (0 <= dt && dt <= 4)
        {
            j = 0;
            Mjd0 = t1;
        }
        else if (4 < dt && dt <= 8)
        {
            j = 1;
            Mjd0 = t1 + 4 * j;
        }
        else if (8 < dt && dt <= 12)
        {
            j = 2;
            Mjd0 = t1 + 4 * j;
        }
        else if (12 < dt && dt <= 16)
        {
            j = 3;
            Mjd0 = t1 + 4 * j;
        }
        else if (16 < dt && dt <= 20)
        {
            j = 4;
            Mjd0 = t1 + 4 * j;
        }
        else if (20 < dt && dt <= 24)
        {
            j = 5;
            Mjd0 = t1 + 4 * j;
        }
        else if (24 < dt && dt <= 28)
        {
            j = 6;
            Mjd0 = t1 + 4 * j;
        }
        else if (28 < dt && dt <= 32)
        {
            j = 7;
            Mjd0 = t1 + 4 * j;
        }

        //��Ҫ�����õ������ݸ�������2�α���13��������
        double[] New_Cx_Moon = DataFetch((13 * j + 1), (13 * j + 13), Cx_Moon);

        double[] New_Cy_Moon = DataFetch((13 * j + 1), (13 * j + 13), Cy_Moon);
       
        double[] New_Cz_Moon = DataFetch((13 * j + 1), (13 * j + 13), Cz_Moon);

        planetVec[9] = PlanetDataCalculate.GetInstance().CalculateCheb3D(Mjd_TDB, 13, Mjd0, Mjd0 + 16, New_Cx_Moon, New_Cy_Moon, New_Cz_Moon);

        #endregion

        #region 3.����̫��ģ��
        start = 753;
        end = 786;
        step = 11;
        EqualDifferenceIncrement(ref temp, start, end, step);

        double[] Cx_Sun = PCtempFunc(temp, PCtemp, 0, 1);

        double[] Cy_Sun = PCtempFunc(temp, PCtemp, 1, 2);

        double[] Cz_Sun = PCtempFunc(temp, PCtemp, 2, 3);

        ///��temp�е�ÿ��ֵ������33
        TempAdd(ref temp, 33);

         Cx = PCtempFunc(temp, PCtemp, 0, 1);

         Cy = PCtempFunc(temp, PCtemp, 1, 2);

         Cz = PCtempFunc(temp, PCtemp, 2, 3);

        Cx_Sun = Cx_Sun.Concat(Cx).ToArray();
        Cy_Sun = Cy_Sun.Concat(Cy).ToArray();
        Cz_Sun = Cz_Sun.Concat(Cz).ToArray();

        if (0 <= dt && dt <= 16)
        {
            j = 0;
            Mjd0 = t1;
        }
        else if (16 < dt && dt <= 32)
        {
            j = 1;
            Mjd0 = t1 + 16 * j;
        }

        //��Ҫ�����õ������ݸ�������3�α���11����̫��
        double[] New_Cx_Sun = DataFetch((11 * j + 1), (11 * j + 11), Cx_Sun);

        double[] New_Cy_Sun = DataFetch((11 * j + 1), (11 * j + 11), Cy_Sun);

        double[] New_Cz_Sun = DataFetch((11 * j + 1), (11 * j + 11), Cz_Sun);

        planetVec[10] = PlanetDataCalculate.GetInstance().CalculateCheb3D(Mjd_TDB, 11, Mjd0, Mjd0 + 16, New_Cx_Sun, New_Cy_Sun, New_Cz_Sun);

        #endregion

        #region 4.����ˮ��ģ��
        start = 3;
        end = 45;
        step = 14;
        EqualDifferenceIncrement(ref temp, start, end, step);

        double[] Cx_Mercury = PCtempFunc(temp, PCtemp, 0, 1);

        double[] Cy_Mercury = PCtempFunc(temp, PCtemp, 1, 2);

        double[] Cz_Mercury = PCtempFunc(temp, PCtemp, 2, 3);

        for (int i = 0; i < 3; i++)
        {
            ///��temp�е�ÿ��ֵ������42
            TempAdd(ref temp, 42);

            Cx = PCtempFunc(temp, PCtemp, 0, 1);

            Cy = PCtempFunc(temp, PCtemp, 1, 2);

            Cz = PCtempFunc(temp, PCtemp, 2, 3);

            Cx_Mercury = Cx_Mercury.Concat(Cx).ToArray();
            Cy_Mercury = Cy_Mercury.Concat(Cy).ToArray();
            Cz_Mercury = Cz_Mercury.Concat(Cz).ToArray();
        }

        if (0 <= dt && dt <= 8)
        {
            j = 0;
            Mjd0 = t1;
        }
        else if (8 < dt && dt <= 16)
        {
            j = 1;
            Mjd0 = t1 + 8 * j;
        }
        else if (16 < dt && dt <= 24)
        {
            j = 2;
            Mjd0 = t1 + 8 * j;
        }
        else if (24 < dt && dt <= 32)
        {

            j = 3;
            Mjd0 = t1 + 8 * j;
        }

        //��Ҫ�����õ������ݸ�������4�α���14��,ˮ��
        double[] New_Cx_Mercury = DataFetch((14 * j + 1), (14 * j + 14), Cx_Mercury);

        double[] New_Cy_Mercury = DataFetch((14 * j + 1), (14 * j + 14), Cy_Mercury);

        double[] New_Cz_Mercury = DataFetch((14 * j + 1), (14 * j + 14), Cz_Mercury);

        planetVec[0] = PlanetDataCalculate.GetInstance().CalculateCheb3D(Mjd_TDB, 14, Mjd0, Mjd0 + 8, New_Cx_Mercury, New_Cy_Mercury, New_Cz_Mercury);

        #endregion

        #region 5.�������ģ��
        start = 171;
        end = 201;
        step = 10;
        EqualDifferenceIncrement(ref temp, start, end, step);

        double[] Cx_Venus = PCtempFunc(temp, PCtemp, 0, 1);

        double[] Cy_Venus = PCtempFunc(temp, PCtemp, 1, 2);

        double[] Cz_Venus = PCtempFunc(temp, PCtemp, 2, 3);

        ///��temp�е�ÿ��ֵ������30
        TempAdd(ref temp, 30);

        Cx = PCtempFunc(temp, PCtemp, 0, 1);

        Cy = PCtempFunc(temp, PCtemp, 1, 2);

        Cz = PCtempFunc(temp, PCtemp, 2, 3);

        Cx_Venus = Cx_Venus.Concat(Cx).ToArray();
        Cy_Venus = Cy_Venus.Concat(Cy).ToArray();
        Cz_Venus = Cz_Venus.Concat(Cz).ToArray();

        if (0 <= dt && dt <= 16)
        {
            j = 0;
            Mjd0 = t1;
        }
        else if (16 < dt && dt <= 32)
        {
            j = 1;
            Mjd0 = t1 + 16 * j;
        }

        //��Ҫ�����õ������ݸ�������5�α���10��������
        double[] New_Cx_Venus = DataFetch((10 * j + 1), (10 * j + 10), Cx_Venus);

        double[] New_Cy_Venus = DataFetch((10 * j + 1), (10 * j + 10), Cy_Venus);

        double[] New_Cz_Venus = DataFetch((10 * j + 1), (10 * j + 10), Cz_Venus);

        planetVec[1] = PlanetDataCalculate.GetInstance().CalculateCheb3D(Mjd_TDB, 10, Mjd0, Mjd0 + 16, New_Cx_Venus, New_Cy_Venus, New_Cz_Venus);

        #endregion

        #region 6.�������ģ��
        start = 309;
        end = 342;
        step = 11;
        EqualDifferenceIncrement(ref temp, start, end, step);

        double[] Cx_Mars = PCtempFunc(temp, PCtemp, 0, 1);

        double[] Cy_Mars = PCtempFunc(temp, PCtemp, 1, 2);

        double[] Cz_Mars = PCtempFunc(temp, PCtemp, 2, 3);

        j = 0;
        Mjd0 = t1;

        //��Ҫ�����õ������ݸ�������6�α���11��������
        double[] New_Cx_Mars = DataFetch((11 * j + 1), (11 * j + 11), Cx_Mars);

        double[] New_Cy_Mars = DataFetch((11 * j + 1), (11 * j + 11), Cy_Mars);

        double[] New_Cz_Mars = DataFetch((11 * j + 1), (11 * j + 11), Cz_Mars);

        planetVec[3]  = PlanetDataCalculate.GetInstance().CalculateCheb3D(Mjd_TDB, 11, Mjd0, Mjd0 + 32, New_Cx_Mars, New_Cy_Mars, New_Cz_Mars);

        #endregion

        #region 7.����ľ��ģ��
        start = 342;
        end = 366;
        step = 8;
        EqualDifferenceIncrement(ref temp, start, end, step);

        double[] Cx_Jupiter = PCtempFunc(temp, PCtemp, 0, 1);

        double[] Cy_Jupiter = PCtempFunc(temp, PCtemp, 1, 2);

        double[] Cz_Jupiter = PCtempFunc(temp, PCtemp, 2, 3);

        j = 0;
        Mjd0 = t1;

        //��Ҫ�����õ������ݸ�������7�α���8����ľ��
        double[] New_Cx_Jupiter = DataFetch((8 * j + 1), (8 * j + 8), Cx_Jupiter);

        double[] New_Cy_Jupiter = DataFetch((8 * j + 1), (8 * j + 8), Cy_Jupiter);

        double[] New_Cz_Jupiter = DataFetch((8 * j + 1), (8 * j + 8), Cz_Jupiter);

        planetVec[4]  = PlanetDataCalculate.GetInstance().CalculateCheb3D(Mjd_TDB, 8, Mjd0, Mjd0 + 32, New_Cx_Jupiter, New_Cy_Jupiter, New_Cz_Jupiter);

        #endregion

        #region 8.��������ģ��
        start = 366;
        end = 387;
        step = 7;
        EqualDifferenceIncrement(ref temp, start, end, step);

        double[] Cx_Saturn = PCtempFunc(temp, PCtemp, 0, 1);

        double[] Cy_Saturn = PCtempFunc(temp, PCtemp, 1, 2);

        double[] Cz_Saturn = PCtempFunc(temp, PCtemp, 2, 3);

        j = 0;
        Mjd0 = t1;

        //��Ҫ�����õ������ݸ�������8�α���7��������
        double[] New_Cx_Saturn = DataFetch((7 * j + 1), (7 * j + 7), Cx_Saturn);

        double[] New_Cy_Saturn = DataFetch((7 * j + 1), (7 * j + 7), Cy_Saturn);

        double[] New_Cz_Saturn = DataFetch((7 * j + 1), (7 * j + 7), Cz_Saturn);

        planetVec[5]  = PlanetDataCalculate.GetInstance().CalculateCheb3D(Mjd_TDB, 7, Mjd0, Mjd0 + 32, New_Cx_Saturn, New_Cy_Saturn, New_Cz_Saturn);

        #endregion

        #region 9.����������ģ��
        start = 387;
        end = 405;
        step = 6;
        EqualDifferenceIncrement(ref temp, start, end, step);

        double[] Cx_Uranus = PCtempFunc(temp, PCtemp, 0, 1);

        double[] Cy_Uranus = PCtempFunc(temp, PCtemp, 1, 2);

        double[] Cz_Uranus = PCtempFunc(temp, PCtemp, 2, 3);

        j = 0;
        Mjd0 = t1;

        //��Ҫ�����õ������ݸ�������9�α���6����������
        double[] New_Cx_Uranus = DataFetch((6 * j + 1), (6 * j + 6), Cx_Uranus);

        double[] New_Cy_Uranus = DataFetch((6 * j + 1), (6 * j + 6), Cy_Uranus);

        double[] New_Cz_Uranus = DataFetch((6 * j + 1), (6 * j + 6), Cz_Uranus);

        planetVec[6]  = PlanetDataCalculate.GetInstance().CalculateCheb3D(Mjd_TDB, 6, Mjd0, Mjd0 + 32, New_Cx_Uranus, New_Cy_Uranus, New_Cz_Uranus);

        #endregion

        #region 10.���㺣����ģ��
        start = 405;
        end = 423;
        step = 6;
        EqualDifferenceIncrement(ref temp, start, end, step);

        double[] Cx_Neptune = PCtempFunc(temp, PCtemp, 0, 1);

        double[] Cy_Neptune = PCtempFunc(temp, PCtemp, 1, 2);

        double[] Cz_Neptune = PCtempFunc(temp, PCtemp, 2, 3);

        j = 0;
        Mjd0 = t1;

        //��Ҫ�����õ������ݸ�������10�α���6����������
        double[] New_Cx_Neptune = DataFetch((6 * j + 1), (6 * j + 6), Cx_Neptune);

        double[] New_Cy_Neptune = DataFetch((6 * j + 1), (6 * j + 6), Cy_Neptune);

        double[] New_Cz_Neptune = DataFetch((6 * j + 1), (6 * j + 6), Cz_Neptune);

        planetVec[7]  = PlanetDataCalculate.GetInstance().CalculateCheb3D(Mjd_TDB, 6, Mjd0, Mjd0 + 32, New_Cx_Neptune, New_Cy_Neptune, New_Cz_Neptune);
   
        #endregion

        #region 11.����ڤ����ģ��
        start = 423;
        end = 441;
        step = 6;
        EqualDifferenceIncrement(ref temp, start, end, step);

        double[] Cx_Pluto = PCtempFunc(temp, PCtemp, 0, 1);

        double[] Cy_Pluto = PCtempFunc(temp, PCtemp, 1, 2);

        double[] Cz_Pluto = PCtempFunc(temp, PCtemp, 2, 3);

        j = 0;
        Mjd0 = t1;

        //��Ҫ�����õ������ݸ�������11�α���6����ڤ����
        double[] New_Cx_Pluto = DataFetch((6 * j + 1), (6 * j + 6), Cx_Pluto);

        double[] New_Cy_Pluto = DataFetch((6 * j + 1), (6 * j + 6), Cy_Pluto);

        double[] New_Cz_Pluto = DataFetch((6 * j + 1), (6 * j + 6), Cz_Pluto);

        planetVec[8]  = PlanetDataCalculate.GetInstance().CalculateCheb3D(Mjd_TDB, 6, Mjd0, Mjd0 + 32, New_Cx_Pluto, New_Cy_Pluto, New_Cz_Pluto);

        #endregion

        #region 12.���������¶�ģ��
        start = 819;
        end = 839;
        step = 10;
        EqualDifferenceIncrement(ref temp, start, end, step);

        double[] Cx_Nutations = PCtempFunc(temp, PCtemp, 0, 1);

        double[] Cy_Nutations = PCtempFunc(temp, PCtemp, 1, 2);

        for (int i = 0; i < 3; i++)
        {
            ///��temp�е�ÿ��ֵ������20
            TempAdd(ref temp, 20);

            Cx = PCtempFunc(temp, PCtemp, 0, 1);

            Cy = PCtempFunc(temp, PCtemp, 1, 2);

            Cx_Nutations = Cx_Nutations.Concat(Cx).ToArray();
            Cy_Nutations = Cy_Nutations.Concat(Cy).ToArray();
        }

        if (0 <= dt && dt <= 8)
        {
            j = 0;
            Mjd0 = t1;
        }
        else if (8 < dt && dt <= 16)
        {
            j = 1;
            Mjd0 = t1 + 8 * j;
        }
        else if (16 < dt && dt <= 24)
        {
            j = 2;
            Mjd0 = t1 + 8 * j;
        }
        else if (24 < dt && dt <= 32)
        {
            j = 3;
            Mjd0 = t1 + 8 * j;
        }

        //��Ҫ�����õ������ݸ�������12�α���10���������¶�
        double[] New_Cx_Nutations = DataFetch((10 * j + 1), (10 * j + 10), Cx_Nutations);

        double[] New_Cy_Nutations = DataFetch((10 * j + 1), (10 * j + 10), Cy_Nutations);

        double[] zeros = new double[10];

        UnityEngine.Vector3 Nutations = PlanetDataCalculate.GetInstance().CalculateCheb3D(Mjd_TDB, 10, Mjd0, Mjd0 + 8, New_Cx_Nutations, New_Cy_Nutations, zeros);
        #endregion

        #region 13.����ƽ��ģ��
        start = 899;
        end = 929;
        step = 10;
        EqualDifferenceIncrement(ref temp, start, end, step);

        double[] Cx_Librations = PCtempFunc(temp, PCtemp, 0, 1);

        double[] Cy_Librations = PCtempFunc(temp, PCtemp, 1, 2);

        double[] Cz_Librations = PCtempFunc(temp, PCtemp, 2, 3);

        for (int i = 0; i < 3; i++)
        {
            ///��temp�е�ÿ��ֵ������30
            TempAdd(ref temp, 30);

            Cx = PCtempFunc(temp, PCtemp, 0, 1);

            Cy = PCtempFunc(temp, PCtemp, 1, 2);

            Cz = PCtempFunc(temp, PCtemp, 2, 3);

            Cx_Librations = Cx_Librations.Concat(Cx).ToArray();

            Cy_Librations = Cy_Librations.Concat(Cy).ToArray();

            Cz_Librations = Cz_Librations.Concat(Cz).ToArray();
        }

        if (0 <= dt && dt <= 8)
        {
            j = 0;
            Mjd0 = t1;
        }
        else if (8 < dt && dt <= 16)
        {
            j = 1;
            Mjd0 = t1 + 8 * j;
        }
        else if (16 < dt && dt <= 24)
        {
            j = 2;
            Mjd0 = t1 + 8 * j;
        }
        else if (24 < dt && dt <= 32)
        {
            j = 3;
            Mjd0 = t1 + 8 * j;
        }

        //��Ҫ�����õ������ݸ�������13�α���10����ƽ��
        double[] New_Cx_Librations = DataFetch((10 * j + 1), (10 * j + 10), Cx_Librations);

        double[] New_Cy_Librations = DataFetch((10 * j + 1), (10 * j + 10), Cy_Librations);

        double[] New_Cz_Librations = DataFetch((10 * j + 1), (10 * j + 10), Cz_Librations);

        UnityEngine.Vector3 Librations = PlanetDataCalculate.GetInstance().CalculateCheb3D(Mjd_TDB, 10, Mjd0, Mjd0 + 8, New_Cx_Librations, New_Cy_Librations, New_Cz_Librations);
        #endregion

        float EMRAT = 81.30056907419062f; // % DE430
        float EMRAT1 = 1 / (1 + EMRAT);
        planetVec[2] = planetVec[2] - (EMRAT1 * planetVec[9]);  //ԭ����

        planetVec[0] = -planetVec[2] + planetVec[0];
        planetVec[1] = -planetVec[2] + planetVec[1];
        planetVec[3] = -planetVec[2] + planetVec[3];
        planetVec[4] = -planetVec[2] + planetVec[4];
        planetVec[5] = -planetVec[2] + planetVec[5];
        planetVec[6] = -planetVec[2] + planetVec[6];
        planetVec[7] = -planetVec[2] + planetVec[7];
        planetVec[8] = -planetVec[2] + planetVec[8];
        planetVec[10] = -planetVec[2] + planetVec[10];
    }

    /// <summary>
    /// ��ӦMatlab�е�PCtemp(temp(1):temp(2)-1)���������޸�
    /// </summary>
    /// <param name="temp"></param>
    /// <param name="PCtemp"></param>
    /// <param name="startIndex"></param>
    /// <param name="endIndex"></param>
    /// <returns></returns>
    double[] PCtempFunc(int[] temp, double[] PCtemp,int startIndex,int endIndex)
    {
        int Estart = temp[startIndex] - 1;
        int count = temp[endIndex] - temp[startIndex];
        var segment = new ArraySegment<double>(PCtemp, Estart, count);
        double[] resultD = segment.ToArray();
        return resultD;
    }

    /// <summary>
    ///��ӦMatlab�е�temp = temp+39;�������޸�
    /// </summary>
    /// <param name="temp"></param>
    void TempAdd(ref int[] temp,int arg_Value)
    {
        for (int i = 0; i < temp.Length; i++)
        {
            temp[i] = temp[i] + arg_Value;
        }
    }

    /// <summary>
    /// ������ȡ���ݲ�����һ���µ����飬��ӦMatlab�е�Cy_Earth(13*j+1:13*j+13)���������޸�
    /// </summary>
    /// <returns></returns>
    double[] DataFetch(int start,int end,double[] currArr)
    {

        int x_erathCount = end - start+1;
        //Debug.Log("������ȡ���ݲ�����һ���µ�����:"+start+",,end:"+end+ ",,x_erathCount:"+ x_erathCount+",,,,�����ܸ�����"+ currArr.Length);
       double[] New_Arr = new double[x_erathCount];
        for (int i = start; i < (end ); i++)
        {
            New_Arr[i - start] = currArr[i];
        }
        return New_Arr;
    }

    /// <summary>
    /// ��ӦMatlab�е�temp = (753:11:786);�������޸�
    /// </summary>
    /// <param name="temp"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="step"></param>
    void EqualDifferenceIncrement(ref int[] temp,int start, int end, int step)
    {
        var range = Enumerable.Range(0, (end - start) / step + 1)
                              .Select(i => start + i * step);
         temp = range.ToArray();
    }

    /// <summary>
    /// �ҵ���һ������JD����PC����1����JDС��PC����2����������
    /// </summary>
    /// <returns></returns>
    public int FindPCRow(double JD)
    {
        //List<double> list = PCList.Find(list => JD > list[0] && JD < list[1]);
        int indexs = PCList.FindIndex(list => JD > list[0] && JD < list[1]);
        if (indexs != -1)
        {
            return indexs;
        }
        return -1;
    }
}
