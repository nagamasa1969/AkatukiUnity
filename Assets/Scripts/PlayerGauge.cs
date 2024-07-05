using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGauge : MonoBehaviour
{
    [SerializeField]
    private Image GreenGauge;
    [SerializeField]
    private Image RedGauge;

    private MoveCharacterAction player;
    private Tween redGaugeTween;

    /// <summary>
    /// プレイヤーライフ処理
    /// </summary>
    /// <param name="reducationValue">ライフ減少量</param>
    /// <param name="time">時間</param>
    public void GaugeReduction(float reducationValue, float time = 1f)
    {
        var valueFrom = player.life / player.maxLife;
        var valueTo = (player.life - reducationValue + 1) / player.maxLife;

        // 緑ゲージ減少
        GreenGauge.fillAmount = valueTo;

        if (redGaugeTween != null)
        {
            redGaugeTween.Kill();
        }

        // 赤ゲージ減少
        redGaugeTween = DOTween.To(
            () => valueFrom,
            x => {
                RedGauge.fillAmount = x;
            },
            valueTo,
            time
        );
    }

    /// <summary>
    /// プレイヤーセット
    /// </summary>
    /// <param name="player"></param>
    public void SetPlayer(MoveCharacterAction player)
    {
        this.player = player;
    }
}
