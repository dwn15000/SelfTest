using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class WaveText : MonoBehaviour
{
    public float duration = 1;      // ��������ʱ��
    public float angle = 360;       // ת���Ƕ�
    public Ease easeType = Ease.Linear;  // ��������
    void Start()
    {
        // ʹ��DORotate����ģ��Բ��ͼƬתȦ����
        transform.DORotate(new Vector3(0, 0, angle), duration, RotateMode.FastBeyond360)
            .SetEase(easeType)
            .SetLoops(-1, LoopType.Restart);
    }
}
