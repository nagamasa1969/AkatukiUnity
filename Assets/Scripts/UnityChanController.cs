using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChanController : MonoBehaviour
{
    public GameController gameControll;
    GameController gameScript;
    public MoveCharacterAction unityMove;
    public float DownMax = 10;
    public float DownTime = 10;
    public bool downCk;

    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        downCk = true;
        gameScript = gameControll.GetComponent<GameController>();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        if(DownTime == 0)
        {
            unityMove.downCk = false;
        }
        else
        {
            unityMove.downCk = true;
        }
    }

    /// <summary>
    /// ライフチェック
    /// </summary>
    void LifeCheck()
    {
        gameScript.Damage(1f);
    }

    /// <summary>
    /// ダウンフラグ処理
    /// </summary>
    void DownCheck()
    {
        if (downCk && DownTime != 0)
        {
            StartCoroutine(DownTimeDown());
        }
    }

    /// <summary>
    /// ダウン時間処理
    /// </summary>
    /// <returns>ダウンフラグ</returns>
    IEnumerator DownTimeDown()
    {
        downCk = false;
        DownTime -= 1;
        gameScript.GageDown(1f);
        yield return new WaitForSeconds(1.0f);
        downCk = true;
    }
}
