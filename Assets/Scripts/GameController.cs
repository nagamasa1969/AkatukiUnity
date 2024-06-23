using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NCMB;


public class GameController : MonoBehaviour
{
    Rigidbody2D rb2d;
    public ScrollObject speed;
    Animator animator;
    public GameObject unityChan;
    public float jumpSize;
    float angle;
    public bool ground;
    public ScrollObject[] beems;
    int beemsCount;
    List<int> beemsArray;
    public float life;
    public TextMeshProUGUI lifetext;
    public TextMeshProUGUI scoretext;
    public int score;
    public bool scFlg = true;
    public AudioClip sound1;
    AudioSource audioSource;
    public AudioSource adBgm;
    public GameObject titleBtn;

    public MoveCharacterAction unity;

    protected PlayerGauge playerGage;
    protected DownGage downGage;

    enum CharaState
    {
        Walk,
        Run,
        Stop
    }

    CharaState UnityState;

    void Awake()
    {
        beemsArray = new List<int>();
        rb2d = unityChan.GetComponent<Rigidbody2D>();
        animator = unityChan.GetComponent<Animator>();
        UnityState = CharaState.Stop;
        beemsCount = beems.Length - 1;
        for(int i = 0; i <= beemsCount; i++)
        {
            beems[i].enabled = false;
            beemsArray.Add(i);
        }
        score = 0;
        life = unity.maxLife;
        lifetext.text = "Life : " + life;
        scoretext.text = "Score : " + score;
        playerGage = GameObject.FindObjectOfType<PlayerGauge>();
        playerGage.SetPlayer(unity);
        downGage = GameObject.FindObjectOfType<DownGage>();
        //downGage.SetPlayer(unity);
        audioSource = GetComponent<AudioSource>();
        titleBtn.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (life > 0)
        {
            if (speed.IsSpeed() >= 8)
            {
                UnityState = CharaState.Run;
            }
            else if (speed.IsSpeed() <= 8 && speed.IsSpeed() != 0)
            {
                UnityState = CharaState.Walk;
            }
            else
            {
                UnityState = CharaState.Stop;
            }

            StartCoroutine(BeemsStart());
            if (scFlg)
            {
                StartCoroutine(ScoreUp());
            }
            beems[0].enabled = true;
            //ApplyAngle();
            scoretext.text = "Score : " + score;
        }
        else
        {
            speed.gameover = false;
        }

        switch (UnityState)
        {
            case CharaState.Walk:
                animator.SetBool("WalkFlg", true);
                animator.SetBool("RunFlg", false);
                break;
            case CharaState.Run:
                animator.SetBool("RunFlg", true);
                break;
            case CharaState.Stop:
                animator.SetBool("WalkFlg", false);
                animator.SetBool("RunFlg", false);
                break;
        }
    }

    void LateUpdate()
    {
        life = unity.life;
    }

    void ApplyAngle()
    {
        float targetAngle = Mathf.Atan2(rb2d.velocity.y, speed.speed) * Mathf.Rad2Deg;
        
    }

    IEnumerator BeemsStart()
    {
        float second = Random.Range(1, 20);
        yield return new WaitForSeconds(second);
        int beemEnable = Random.Range(0, beemsArray.Count);
        beems[beemEnable].enabled = true;
    }

    IEnumerator ScoreUp()
    {
        scFlg = false;
        float second = 1;
        yield return new WaitForSeconds(second);
        score += 10;
        scFlg = true;
    }

    void SpeedDown()
    {
        speed.speed -= 5;
        if(speed.speed <= 0)
        {
            speed.speed = 1;
        }
    }

    public void Damage(float power)
    {
        playerGage.GaugeReduction(power);
        life -= power;
        lifetext.text = "Life : " + life;
        audioSource.PlayOneShot(sound1);
        if(life <= 0){
            UnityState = CharaState.Stop;
            adBgm.Stop();

            StartCoroutine(EndTime());
        }
    }

    public void GageDown(float Time)
    {
        downGage.GaugeDownImage(Time);
    }

    IEnumerator EndTime()
    {
        yield return new WaitForSeconds(1f);
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(score);
        titleBtn.SetActive(true);
    }
}