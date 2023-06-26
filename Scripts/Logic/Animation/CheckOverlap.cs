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
        // ��ȡ������Image�Ŀ���ʾ���ط�Χ
        pixelRect = RectTransformUtility.PixelAdjustRect(irregularImage.rectTransform, irregularImage.canvas);
        // ��ȡ������Image����ͼ
        texture = irregularImage.sprite.texture;
        // ��ȡ������Image��͸������Ϣ
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

            // �����λ��
            Vector2 mousePosition = Input.mousePosition;
            // �������λ��ת��ΪUI����ϵ�µ�λ��
            RectTransform rectTransform = irregularImage.rectTransform;
            Vector2 point;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePosition, null, out point);
            // ��ȡ������Image����Ŀ���ʾ��������
            Rect pixelRect = RectTransformUtility.PixelAdjustRect(rectTransform, irregularImage.canvas);

            Vector2Int pixelPosition = new Vector2Int((int)mousePosition.x, (int)mousePosition.y);

            // �жϸõ��Ƿ��ڿ���ʾ����������
            if (pixelRect.Contains(point))
            {
                // ���ڷ�Χ��
                Debug.Log("Point is in the range="+ pixels.Length);
                if (pixels[pixelPosition.x, pixelPosition.y])
                {
                    Debug.Log("Point is in the range.iiiiiii");
                }
            }


            Debug.Log("�����λ��");
            // �����λ��
            //Vector2 mousePosition = Input.mousePosition;
            //// �������λ��ת��Ϊ��������
            //Vector2Int pixelPosition = new Vector2Int((int)mousePosition.x, (int)mousePosition.y);
            //// ���������λ���ڲ�����Image�Ŀ���ʾ���ط�Χ��
            //if (pixelRect.Contains(mousePosition))
            //{
            //    // ���ڷ�Χ��
            //    Debug.Log("Point is in the range.");
            //    if (pixels[pixelPosition.x, pixelPosition.y])
            //    {
            //        Debug.Log("Point is in the range.iiiiiii");
            //    }
            //}
        }
        
    }
}