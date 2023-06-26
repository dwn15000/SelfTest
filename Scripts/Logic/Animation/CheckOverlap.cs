using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CheckOverlap : MonoBehaviour
{
    public Image irregularImage;
    private Texture2D texture;
    private bool[,] pixels;
    private Rect pixelRect;
    void Start()
    {
        // 获取不规则Image的可显示像素范围
        pixelRect = RectTransformUtility.PixelAdjustRect(irregularImage.rectTransform, irregularImage.canvas);
        // 获取不规则Image的贴图
        texture = irregularImage.sprite.texture;
        // 获取不规则Image的透明度信息
        pixels = new bool[texture.width, texture.height];
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                pixels[x, y] = texture.GetPixel(x, y).a > 0;
            }
        }
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {

            // 鼠标点击位置
            Vector2 mousePosition = Input.mousePosition;
            // 将鼠标点击位置转换为UI坐标系下的位置
            RectTransform rectTransform = irregularImage.rectTransform;
            Vector2 point;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePosition, null, out point);
            // 获取不规则Image组件的可显示像素区域
            Rect pixelRect = RectTransformUtility.PixelAdjustRect(rectTransform, irregularImage.canvas);

            Vector2Int pixelPosition = new Vector2Int((int)mousePosition.x, (int)mousePosition.y);

            // 判断该点是否在可显示像素区域内
            if (pixelRect.Contains(point))
            {
                // 点在范围内
                Debug.Log("Point is in the range="+ pixels.Length);
                if (pixels[pixelPosition.x, pixelPosition.y])
                {
                    Debug.Log("Point is in the range.iiiiiii");
                }
            }


            Debug.Log("鼠标点击位置");
            // 鼠标点击位置
            //Vector2 mousePosition = Input.mousePosition;
            //// 将鼠标点击位置转换为像素坐标
            //Vector2Int pixelPosition = new Vector2Int((int)mousePosition.x, (int)mousePosition.y);
            //// 如果该像素位置在不规则Image的可显示像素范围内
            //if (pixelRect.Contains(mousePosition))
            //{
            //    // 点在范围内
            //    Debug.Log("Point is in the range.");
            //    if (pixels[pixelPosition.x, pixelPosition.y])
            //    {
            //        Debug.Log("Point is in the range.iiiiiii");
            //    }
            //}
        }
        
    }
}