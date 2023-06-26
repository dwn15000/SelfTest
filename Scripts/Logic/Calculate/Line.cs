using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class Line: MonoBehaviour
{
    public LineRenderer lineRenderer;
    private void Start()
    {
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.SetPositions(new Vector3[] { new Vector3(100, 100, 0), new Vector3(200, 100, 0) });
    }


}
