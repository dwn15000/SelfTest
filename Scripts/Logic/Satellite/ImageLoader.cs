using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ImageLoader : MonoBehaviour
{
    public string folderPath; // �ļ���·��
    public Texture2D[] textures; // �洢����ͼƬ������
    Dictionary<string, Texture2D> textureDic = new Dictionary<string, Texture2D>();
    /// <summary>
    /// �õ��ķ�����ͼ
    /// </summary>
    public Texture2D NormalMap;
    void Start()
    {
        
    }

    private void Awake()
    {
        IocContainer_InstanceMgr.GetInstance().RegistInstance(this);
    }


    /// <summary>
    /// ����ģ�����е���ͼ
    /// </summary>
    public void LoadModelAllMap()
    {
        Debug.Log("ͼƬ��ַ:"+folderPath);
        folderPath += "/Map";
        DirectoryInfo dir = new DirectoryInfo(folderPath);
        if (!dir.Exists)
        {
            Debug.Log("û�е�ַ��");
            return;
        }
        FileInfo[] info = dir.GetFiles("*.*");
        textures = new Texture2D[info.Length];
        int index = 0;
        foreach (FileInfo f in info)
        {
            if (f.Extension.ToLower() == ".png" || f.Extension.ToLower() == ".jpg" || f.Extension.ToLower() == ".bmp")
            {
                byte[] fileData = File.ReadAllBytes(f.FullName);
                Texture2D tex = new Texture2D(256, 256);           
                tex.LoadImage(fileData);
                if (f.Name.Split('.')[0] == "Nor")
                {
                    textures[index] = NormalMap;
                    textureDic.Add(f.Name.Split('.')[0], NormalMap);
                }
                else
                {
                    textures[index] = tex;
                    textureDic.Add(f.Name.Split('.')[0], tex);
                }              
                index++;
            }
        }
    }


    /// <summary>
    /// ������ͼ
    /// </summary>
    public void SetMap(Material m,string name,bool isNormal)
    {
        string ChangName = name.Substring(1);
        string type = name.Substring(0,1);
        if (isNormal)
        {
            if (textureDic.ContainsKey(ChangName))
            {
                m.SetTexture("_BumpMap", textureDic[ChangName]);
                m.EnableKeyword("_NORMALMAP");
            }
            
        }
        else
        {
            if (textureDic.ContainsKey(ChangName))
            {
                m.SetTexture(GetAttribute(type, ChangName), textureDic[ChangName]);
                m.SetTexture("_BumpMap", null);
            }
        }
       
    }


    /// <summary>
    /// ��ȡshader����
    /// </summary>
    /// <param name="name"></param>
    /// <param name="ChangName"></param>
    /// <returns></returns>
    string GetAttribute(string name,string ChangName)
    {
        switch (name)
        {
            case "M":               
                return "_MetallicGlossMap";
            case "B":
                return "_BaseMap";
            default:
                return "";             
        }
    }
}
