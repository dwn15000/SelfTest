using System.Collections;
using System.Collections.Generic;
using System.IO;
using TriLibCore;
using UnityEngine;
using UnityEngine.Networking;

public class WWWTest : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        IocContainer_InstanceMgr.GetInstance().RegistInstance<WWWTest>(this);
        //StartCoroutine(StartLoad());

    }


    public void DownStreamFile(string path,string name)
    {
        StartCoroutine(StartLoad(path,name));
    }
    IEnumerator StartLoad(string s,string names)
    {

        string parentPath = Path.GetDirectoryName(s); // ��ȡAssets�ļ����ϼ�Ŀ¼·��
        string path = parentPath;
        path = path.Replace("\\", "/");
        //����ģ�����ڵ��ļ��е��ļ���·��
        IocContainer_InstanceMgr.GetInstance().GetInstance<ImageLoader>().folderPath = path;
        IocContainer_InstanceMgr.GetInstance().GetInstance<ImageLoader>().LoadModelAllMap();


        var url = s;
        var www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();


        Debug.Log(www.error+",,,url="+url);
        Debug.Log("data="+ www.downloadHandler.data.Length);
        if (www.error!=string.Empty)
        {
            Stream stream = new MemoryStream(www.downloadHandler.data);

            IocContainer_InstanceMgr.GetInstance().GetInstance<AssetLoaderFilePicker>().ComeLocalDownStream(stream, names);
           
        }
        else
        {
            Debug.Log(www.responseCode); //״̬�� 200��ʾ����ɹ�
            Debug.Log(www.downloadHandler.text); //��������Ӧ��Ϣ
        }
    }

}
