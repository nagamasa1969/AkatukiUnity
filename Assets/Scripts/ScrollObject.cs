using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollObject : MonoBehaviour
{
    public GameController gameControll;
    GameController gameScript;
    public float speed;
    public float startPosition;
    public float endPosition;
    float speedTime = 5.0f;
    public float MaxSpeed;
    public bool gameover = true;


    /// <summary>
    /// スピード
    /// </summary>
    /// <returns></returns>
    public float IsSpeed()
    {
        return speed;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Start()
    {
        gameScript = gameControll.GetComponent<GameController>();
        gameover = true;
    }

    // Update is called once per frame
    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        if (gameover)
        {
            // 毎フレームxポジションを少しずつ移動させる
            if (speedTime >= Time.deltaTime)
            {
                transform.Translate(-1 * speed * Time.deltaTime, 0, 0);
                speedTime -= Time.deltaTime;
            }
            else
            {
                if (speed <= MaxSpeed)
                {
                    speed += 1.0f;
                }
                speedTime = 5.0f;
            }
            if(gameScript.life <= 0)
            {
                GameStop();
            }

            // スクロールが目標ポイントまで到達したかチェック
            if (transform.position.x <= endPosition) ScrollEnd();
        }
 
    }

    /// <summary>
    /// スクロールエンド処理
    /// </summary>
    void ScrollEnd()
    {
        // 通り過ぎた分を加味してポジションを再設定
        float diff = transform.position.x - endPosition;
        Vector3 restartPosition = transform.position;
        restartPosition.x = startPosition + diff;

        transform.position = restartPosition;

        // 同じゲームオブジェクトにアタッチされているコンポーネントにメッセージを送る
        SendMessage("OnScrollEnd", SendMessageOptions.DontRequireReceiver);
    }

    void GameStop()
    {
        gameover = false;
    }
}
