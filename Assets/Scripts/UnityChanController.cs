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

    void Start()
    {
        downCk = true;
        gameScript = gameControll.GetComponent<GameController>();
    }

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
    void LifeCheck()
    {
        gameScript.Damage(1f);
    }

    void DownCheck()
    {
        if (downCk && DownTime != 0)
        {
            StartCoroutine(DownTimeDown());
        }
    }

    IEnumerator DownTimeDown()
    {
        downCk = false;
        DownTime -= 1;
        gameScript.GageDown(1f);
        yield return new WaitForSeconds(1.0f);
        downCk = true;
    }
}
