using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ImageOverlap : MonoBehaviour
{
    public Image image1;
    public Image image2;
    private void Update()
    {
        bool isIntersecting = IsIntersecting(image1.rectTransform, image2.rectTransform);
        if (isIntersecting)
        {
            Debug.Log("两个Image有交接");
        }
        else
        {
            Debug.Log("两个Image没有交接");
        }
    }
    private bool IsIntersecting(RectTransform rect1, RectTransform rect2)
    {
        if (rect1 == null || rect2 == null)
        {
            return false;
        }
        Rect rect1Rect = new Rect(rect1.position.x, rect1.position.y, rect1.rect.width, rect1.rect.height);
        Rect rect2Rect = new Rect(rect2.position.x, rect2.position.y, rect2.rect.width, rect2.rect.height);
        return rect1Rect.Overlaps(rect2Rect) && !rect1Rect.Contains(rect2Rect.min) && !rect1Rect.Contains(rect2Rect.max);
    }
}
