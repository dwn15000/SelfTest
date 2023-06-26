using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestLoadSatellite : MonoBehaviour
{
    /// <summary>
    /// ��ʾ��̫��ֱ��㾭γ��Image
    /// </summary>
    public GameObject showObjImage;

    public RawImage shadow;

    public LineRenderer lineRenderer;
    /// <summary>
    /// ��ʾ���������ľ�γ������
    /// </summary>
    public Text txt_lonLat;

    List<OrbitData> satellites = new List<OrbitData>();

    /// <summary>
    /// �Ƿ�������
    /// </summary>
    bool isLoadFinish = false;
    /// <summary>
    /// ��ǰ��ɵĸ���
    /// </summary>
    int count = 1;
    /// <summary>
    /// ��ʱ��ʱ��
    /// </summary>
    float timers = 0.0f;
    /// <summary>
    /// ��ȡ����ʱ������
    /// </summary>
    int[] dateTime;

    float ScreenWidth = 0.0f;

    /// <summary>
    /// ��γ��
    /// </summary>
    Vector3 lonLat;
    // Start is called before the first frame update
    void Start()
    {
        
        //lineRenderer.startWidth = 0.3f;
        //lineRenderer.endWidth = 0.3f;
        //lineRenderer.useWorldSpace = true;

        ScreenWidth = PlayerPrefs.GetFloat("CanvasWidth", 0.0f);

        screenWidthdifference = PlayerPrefs.GetFloat("CanvasWidth", 0.0f)/360;
        screenHeightdifference = PlayerPrefs.GetFloat("Canvasheight", 0.0f) / 180;
        Debug.Log("��Ļ��߱�-screenWidthdifference:" + screenWidthdifference + ",,,screenHeightdifference=" + screenHeightdifference);

        TextAsset questdata = Resources.Load<TextAsset>("data/SAT1_RVandQbi");
        string[] data = questdata.text.Split(new char[] { '\n' });
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
            satellites.Add(sate);
        }
        isLoadFinish = true;
    }

    int currentPoint = 0;
   
    // Update is called once per frame
    void Update()
    {
        if (isLoadFinish)
        {
            timers += Time.deltaTime;
            if (timers >= 0.05f)
            {
                timers = 0.0f;
                dateTime = DataTime(satellites[count].time);
                lonLat = IocContainer_InstanceMgr.GetInstance().GetInstance<SolarSystemEphemerisCtrl>().Test_JPL430eph(dateTime);
                txt_lonLat.text = "Lon:" + lonLat.x + " Lat:" + lonLat.y;
               
                showObjImage.transform.localPosition = GetShowLonLatData(lonLat);
                MoveEarthShadow(showObjImage.transform.localPosition.x);
                count +=20;
                //this.lineRenderer.positionCount = (currentPoint + 1);
                ////����:
                //this.lineRenderer.SetPosition(currentPoint, SatelliteTrans[i].position);
                //currentPoint++;
            }
            if (count>= satellites.Count)
            {
                isLoadFinish = false;
            }
        }
       
    }


    Vector3 lonLatPos;
    /// <summary>
    /// ��Ļ�����
    /// </summary>
    float screenWidthdifference = 0.0f;
    /// <summary>
    /// ��Ļ�߱���
    /// </summary>
    float screenHeightdifference = 0.0f;

    /// <summary>
    /// �õ���ʾ�ľ�γ������
    /// </summary>
    /// <returns></returns>
    Vector3 GetShowLonLatData(Vector3 pos)
    {
        
        lonLatPos.x = (pos.x - 180) * screenWidthdifference;
        lonLatPos.y = pos.y * screenHeightdifference;
        return lonLatPos;
    }

    float backPos = 0.0f;

    /// <summary>
    /// �ƶ���Ӱ��ͼ
    /// </summary>
    void MoveEarthShadow(float x)
    {

        float a = (ScreenWidth / 2) + x;
        a = a * (-0.00026f);//�������ڼ���
        shadow.uvRect = new Rect(a, 0, 1, 1);

        backPos = x;
    }



    int[] strs = new int[6];
    int[] DataTime(string data)
    {
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
                strs[i] = int.Parse(nyr[i]);
            }
            else
                strs[i] = int.Parse(sfm[i - 3]);

        }
        return strs;
    }


    private class OrbitData
    {
        public string time; // ʱ��
        public float x; // X������
        public float y; // Y������
        public float z; // Z������
        public float vx; // X���ٶ�
        public float vy; // Y���ٶ�
        public float vz; // Z���ٶ�
        public float q1;  //��Ԫ����x
        public float q2;  //��Ԫ����y
        public float q3;  //��Ԫ����z
        public float q4;  //��Ԫ����w
        public float[] CMGAng = new float[6];
        public float[] CMGRotate = new float[6];
        public float CMG1_ang; //CMG1���Ƕ���Ϣ
        public float CMG1_rate; //CMG1��ת��ת����Ϣ(תÿ��)
        public float CMG2_ang; //CMG2���Ƕ���Ϣ
        public float CMG2_rate; //CMG2��ת��ת����Ϣ(תÿ��)
        public float CMG3_ang; //CMG3���Ƕ���Ϣ
        public float CMG3_rate; //CMG3��ת��ת����Ϣ(תÿ��)
        public float CMG4_ang; //CMG4 ���Ƕ���Ϣ
        public float CMG4_rate; //CMG4 ��ת��ת����Ϣ(תÿ��)
        public float CMG5_ang; //CMG5 ���Ƕ���Ϣ
        public float CMG5_rate; //CMG5 ��ת��ת����Ϣ(תÿ��)
        public float CMG6_ang; //CMG6 ���Ƕ���Ϣ
        public float CMG6_rate; //CMG6 ��ת��ת����Ϣ(תÿ��)
    }
}

