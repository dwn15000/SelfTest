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
        Debug.Log("��ʼ�Ӵ�");
        Debug.Log(collider.name);
    }

    // �Ӵ�����
    void OnTriggerExit2D(Collider2D collider)
    {
        Debug.Log("�Ӵ�����:" + collider.name);
    }


    // �Ӵ�������
    void OnTriggerStay2D(Collider2D collider)
    {
        Debug.Log("�Ӵ�������");
        Debug.Log(collider.name);
    }
}
