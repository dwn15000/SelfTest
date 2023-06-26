using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class ResHelper
{
    private static ResHelper _instance = null;

    public static ResHelper GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ResHelper();
           
        }
        return _instance;
    }

    public GameObject LoadUI(string UIName)
    {
        GameObject prefab = Resources.Load<GameObject>(UIName);
        if (prefab)
        {

            GameObject obj = GameObject.Instantiate(prefab) as GameObject;
            if (obj)
            {
                
                string[] s_name = UIName.Split('/');
                obj.name = s_name[s_name.Length - 1];
               
                obj.transform.parent = GameObject.Find("Canvas").transform;
                RectTransform rect = obj.GetComponent<RectTransform>();
                if (rect != null)
                {
                    rect.offsetMax = Vector2.zero;
                    rect.offsetMin = Vector2.zero;
                    rect.localPosition = new Vector3(0,0,-3);
                }
                obj.transform.localScale = Vector2.one;
                obj.SetActive(true);
                return obj;
            }
        }
        return prefab;
    }
}
