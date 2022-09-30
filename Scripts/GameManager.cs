using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //难度调整
    public static int EnemyNumber = 3;
    public static int TurretLimit = 1;
    public static int MoneyStart = 3;
    //塔防限制
    public GameObject[] Limit_Grouds;
    public GameObject[] Limit_Maps;

    public int[] StartMoney = new int[3];
    [HideInInspector]
    public int TotalMoney;

    private bool goodtoGo;
    private bool isSelectedTurrent;
    public sampleTurrent[] turrets = new sampleTurrent[5];
    public  GameObject follower;

    [SerializeField]
    private sampleTurrent selectedTurretData;//当前选择的

    //获取UImanager
    public GameUIManager gameUIManager;
    public EnemySpawner enemySpawner;
    //游戏进度调查
    public int LIFE = 3;
    public GameObject[] LIFEeggs;
    //游戏胜利
    private bool IsWin;
    //游戏失败
    private bool IsLose;
    //开场提示
    public GameObject StartGameAnim;

    //BGM
    private AudioSource playBgm;
    public AudioClip WinAduio, LoseAudio;
    public AudioClip[] BGMs;
    private float gameStartTimedown;

    private void Awake()
    {
        StartGameAnim.SetActive(false);
        TotalMoney = StartMoney[MoneyStart - 1];
        //塔防限制
        for (int i = 0; i < TurretLimit - 1; i++)
        {
            Limit_Grouds[i].SetActive(false);
            Limit_Maps[i].SetActive(true);
        }
    }

    void Start()
    {
        //Debug.Log("EnemyNumber:" + EnemyNumber);
        //Debug.Log("TurretLimit:" + TurretLimit);
        //Debug.Log("MoneyReturn:" + MoneyStart);
        Time.timeScale = 1;
        isSelectedTurrent = false;
        goodtoGo = false;
        IsWin = false;
        IsLose = false;
        //背景音乐设置
        gameStartTimedown = enemySpawner.waves[0].rate + Time.time;
        playBgm = GetComponent<AudioSource>();
        playBgm.clip = BGMs[UnityEngine.Random.Range(0, BGMs.Length)];
        playBgm.volume = 0;
        playBgm.Play();

        foreach (sampleTurrent turrent in turrets)
        {
            turrent.Set_canSelect(true);
            if (turrent.turrentPrefab.GetComponent<AbstractTurrent>() == null)
                return;
            turrent.turrentPrefab.GetComponent<AbstractTurrent>().typeofTurrent = turrent.turrentType.ToString();
            turrent.turrentPrefab.GetComponent<AbstractTurrent>().GetMoneyInfor(turrent.cost);
        }



    }

    // Update is called once per frame
    void Update()
    {
        if ((gameStartTimedown - Time.time) <= 5f)
        {
            StartGameAnim.SetActive(true);
        }
        //缓入音乐
        if ((gameStartTimedown - Time.time) <= 1f && playBgm.volume!= SettingManager.BGMvolume)
        {
            StartGameAnim.SetActive(false);
            //Debug.Log("缓入音乐");
            StartGameAnim.GetComponent<AudioSource>().volume = 0;
            playBgm.volume = Mathf.Lerp(playBgm.volume, SettingManager.BGMvolume, Time.deltaTime);
        }

        CheckLose();
        CheckWin();

        followtheMouse();
        int ToggleFalseNum = 0;
        foreach (sampleTurrent turrent in turrets)
        {
            turrent.CoolDownTimeCal();
            if (turrent.ToggleButton.isOn == false)
                ToggleFalseNum++;
        }
        if (ToggleFalseNum == turrets.Length)
        {
            goodtoGo = false;
            follower.GetComponent<SpriteRenderer>().sprite = null;
            ReSetCurrentTurrent();
        }
        //Debug.Log("ToggleFalseNum " + ToggleFalseNum + " turrets.Length" + turrets.Length);

        //三项判断是否可以放置 ：
        // 1、塔防是否被选中
        // 2、被选中的塔防CD是否已到 
        // 3、玩家所剩金钱是否足够

        if (isSelectedTurrent)//1
        {
            if(selectedTurretData.Get_canSelect())//2
            {
                if(TotalMoney >= selectedTurretData.cost)//3
                {
                    goodtoGo = true;
                }
                else//钱不够
                {
                    gameUIManager.PlayCanNotAffordMoney();
                    ReSetCurrentTurrent();
                }
                
            }    
            else //冷却时间没到
            {
                goodtoGo = false;
                follower.GetComponent<SpriteRenderer>().sprite = null;
                ReSetCurrentTurrent();
            }
        }
        else //没选中塔防
        {
            goodtoGo = false;
            follower.GetComponent<SpriteRenderer>().sprite = null;
            ReSetCurrentTurrent();
            foreach (sampleTurrent turrent in turrets)
            {
                turrent.ToggleButton.isOn = false;
            }

        }
       
    }

    public bool CanAffordMoney(int cost)
    {
        if (TotalMoney - cost >= 0)
        {
            return true;
        }
        else
        {
            gameUIManager.PlayCanNotAffordMoney();
            return false;
        }
            
    }

    /// <summary>
    /// 减少金钱
    /// </summary>
    /// <param name="cost"></param>
    public void PayMoney(int cost)
    {
        if (TotalMoney - cost >= 0)
        {
            TotalMoney -= cost;  
        }
    }

    /// <summary>
    /// 获得金钱
    /// </summary>
    /// <param name="money"></param>
    public void GetMoney(int money)
    {
        TotalMoney += money;
    }

    public int GetTotalMoney()
    {
        return TotalMoney;
    }

    //选择Turret按钮 
    public void onSelected(Toggle toggle)
    {
        int Num = toggle.GetComponent<TurretButtonChoose>().getTurrentNum();
        selectedTurretData = turrets[Num];
        isSelectedTurrent = true;

        if(TotalMoney >= selectedTurretData.cost)
        {//播放能够选择音效
            gameUIManager.PlayChoseTurret();
        }
        follower.GetComponent<SpriteRenderer>().sprite = turrets[Num].turrentPrefab.GetComponent<SpriteRenderer>().sprite;
    }

    /// <summary>
    /// 获取所选Turrent
    /// </summary>
    /// <returns></returns>
    public sampleTurrent GetCurrentTurrent()
    {
        //这里返回属性由Gameobject改为sampleTurrent是为获得更多相关属性
        return selectedTurretData;
    }

    /// <summary>
    /// 重置当前选择塔防
    /// </summary>
    public void ReSetCurrentTurrent()
    {
        selectedTurretData = null;
        isSelectedTurrent = false;
    }

    /// <summary>
    /// 检测是否已经可以放置塔防
    /// </summary>
    /// <returns></returns>
    public bool GetTurrentPermission()
    {
        return goodtoGo;
    }

    /// <summary>
    /// 点击其他塔防时失效UI
    /// </summary>
    /// <param name="candidate"></param>
    public void respondtoUI(AbstractTurrent candidate) {
        bool sharedpreference = candidate.IsClick;
        GameObject[] assmebly = GameObject.FindGameObjectsWithTag("turrents");
        foreach (GameObject g in assmebly)
        {
            AbstractTurrent a = g.GetComponent<AbstractTurrent>();
            a.IsClick = false;    
        }
        candidate.IsClick = sharedpreference;
    }


    private void followtheMouse() {
        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dir.z = 0;
        follower.transform.position = dir;
    }

    public void LIFEdecrease(int spendLIFE)
    {
        for (int num = 1; num <= spendLIFE && LIFE > 0; num++)
        {
            LIFEeggs[LIFE-1].gameObject.SetActive(false);
            LIFE--;
        }
    }

    public void CheckWin()
    {
        if(enemySpawner.Get_IsWin() && !IsLose && !IsWin)
        {
            IsWin = true;
            playBgm.loop = false;
            playBgm.PlayOneShot(WinAduio, 1f);
            //Time.timeScale = 0;
        }
        gameUIManager.ShowWinUI(IsWin);
    }

    public void CheckLose()
    {
       if (LIFE <= 0 && !IsLose)
       {
            Debug.Log("失败！");
            playBgm.loop = false;
            playBgm.clip = LoseAudio;
            playBgm.PlayOneShot(LoseAudio,1f);
            gameUIManager.ShowLoseUI();
            Time.timeScale = 0;
            IsLose = true;
       }
    }

}
