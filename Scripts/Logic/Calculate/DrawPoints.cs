using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DrawPoints : MonoBehaviour
{
    public Image arrow; //Image 素材
    public RectTransform pa; //A点
    public RectTransform pb; //B点

    public TMP_Dropdown drop;

    private void Start()
    {
        drop.AddOptions(new List<string> { "风云一号", "风云二号", "风云三号" });
        //var rP = pa.anchoredPosition;
        //var tp = pa.position;
        //Debug.Log($"A点  Transform P:({tp.x},{tp.y}) RectTransform P :({rP.x},{rP.y})");
    }

    // Update is called once per frame
    //void Update()
    //{
    //    arrow.transform.position = pa.position;
    //    arrow.transform.localRotation = Quaternion.AngleAxis(-GetAngle(), Vector3.forward);

    //    var distance = Vector2.Distance(pb.anchoredPosition, pa.anchoredPosition);
    //    arrow.rectTransform.sizeDelta = new Vector2(10, Math.Max(1, distance - 30));
    //}


    //public float GetAngle()
    //{
    //    var dir = pb.position - pa.position;
    //    var dirV2 = new Vector2(dir.x, dir.y);
    //    var angle = Vector2.SignedAngle(dirV2, Vector2.down);


    //    return angle;
    //}
}
