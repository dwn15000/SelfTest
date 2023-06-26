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

        string parentPath = Path.GetDirectoryName(s); // 获取Assets文件夹上级目录路径
        string path = parentPath;
        path = path.Replace("\\", "/");
        //设置模型所在的文件夹的文件夹路径
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
            Debug.Log(www.responseCode); //状态码 200表示请求成功
            Debug.Log(www.downloadHandler.text); //服务器响应信息
        }
    }

}
