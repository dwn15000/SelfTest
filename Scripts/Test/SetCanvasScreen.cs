using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCanvasScreen : MonoBehaviour
{
    private void Awake()
    {
        var rt = this.gameObject.GetComponent<RectTransform>();
        PlayerPrefs.SetFloat("CanvasWidth", rt.sizeDelta.x);
        PlayerPrefs.SetFloat("Canvasheight", rt.sizeDelta.y);
        Debug.Log("¿í:"+ rt.sizeDelta.x+",,,,height:"+ rt.sizeDelta.y);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
