using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class WaveText : MonoBehaviour
{
    public float duration = 1;      // 动画持续时间
    public float angle = 360;       // 转动角度
    public Ease easeType = Ease.Linear;  // 缓动类型
    void Start()
    {
        // 使用DORotate方法模拟圆形图片转圈动画
        transform.DORotate(new Vector3(0, 0, angle), duration, RotateMode.FastBeyond360)
            .SetEase(easeType)
            .SetLoops(-1, LoopType.Restart);
    }
}
