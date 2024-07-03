using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DownGage : MonoBehaviour
{
    [SerializeField]
    private Image GreenGauge;
    private bool recFlg = true;

    private Tween greenGaugeTween;
    public UnityChanController time;
    float Dtime;
    float Mtime;

    void Update()
    {
        Dtime = time.DownTime;
        Mtime = time.DownMax;
    }
    public void GaugeDownImage(float reducationValue, float timeA = 1f)
    {
        if (Dtime <= 0)
        {
            return;
        }
        var valueFrom = (Dtime - 1) / Mtime;
        var valueTo = (Dtime - reducationValue - 1) / Mtime;

        // 緑ゲージ減少
        if (greenGaugeTween != null)
        {
            greenGaugeTween.Kill();
        }

        // 緑ゲージ減少
        greenGaugeTween = DOTween.To(
            () => valueFrom,
            x => {
                GreenGauge.fillAmount = x;
            },
            valueTo,
            timeA
        );
    }

    IEnumerator DownRecover(float second)
    {
        recFlg = false;
        yield return new WaitForSeconds(second);
        if (Mtime > Dtime)
        {
            Dtime++;
            time.DownTime = Dtime;
        }
        var valueFrom = Dtime / Mtime;
        var valueTo = (Dtime + second) / Mtime;
        greenGaugeTween = DOTween.To(
        () => valueFrom,
        x => {
            GreenGauge.fillAmount = x;
        },
        valueTo,
        1f
        );
        recFlg = true;
    }
}
