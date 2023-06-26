using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCliderTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("开始接触");
        Debug.Log(collider.name);
    }

    // 接触结束
    void OnTriggerExit2D(Collider2D collider)
    {
        Debug.Log("接触结束:" + collider.name);
    }


    // 接触持续中
    void OnTriggerStay2D(Collider2D collider)
    {
        Debug.Log("接触持续中");
        Debug.Log(collider.name);
    }
}
