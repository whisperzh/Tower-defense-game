using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public abstract class AbstractTurrent : MonoBehaviour
{
    private Color originalColor;
    public SpriteRenderer spr;
    public bool TimeToChange;
    public int flickingDuration;
    //升级相关组件
    public bool IsUpgrated=false;//控制UI的销毁
    //攻击范围
    [Header("攻击范围升级")]
    public float ParaforRG;
    public int Rpayment;
    protected bool IsRgUpdate;
    //攻击力
    [Header("攻击力升级")]
    public float ParaforATK;  
    public int Apayment;
    private bool IsATKUpdate;
    //攻击速率
    [Header("攻击速率升级")]
    public float ParaforSp;
    public int Spayment;
    protected bool IsSpUpdate;
    //价值
    [Header("价值")]
    public int money;
    //游戏管理
    private GameManager gameManager;
    private CircleCollider2D circleCollider2D;
    //塔防UI管理
    [Header("塔防UI")]
    public GameObject TurretUI;
    public GameObject[] TurretUpdateUI;
    protected Animator TurretUpdateUIanim1, TurretUpdateUIanim2;
    protected Animator TurretUIanim;
    [HideInInspector]
    public bool IsClick = false;

    //生命值
    [Header("生命值")]
    public float maxhealth;
    public float currenthealth;
    protected Slider healthSlider;

    //子弹信息
    [Header("子弹信息")]
    public GameObject bulletPrefab;
    public float damage;
    public float bulletSpeed;
   
    //开火的位置
    protected Transform firePos;

    //攻击范围
    [Header("攻击范围")]
    public float minAttackRange = 1f;
    public GameObject RangeUI;

    //攻击频率
    [Header("攻击频率")]
    public float fireRate = 1f;
    protected float nextFire = 0;
    protected bool firstAttack = true;

    //获取敌人名单
    public List<GameObject> enemyList = new List<GameObject>();
    protected bool hasGetEnemy = false;

    //塔防类型
    public string typeofTurrent = null;

    //动画角色
    [Header("动画角色")]
    public GameObject birdSprit;
    protected Animator birdAnim;
    protected bool isAttack;
    protected bool isDead;

    //音效
    [Header("音效")]
    protected AudioSource audioSource;
    public AudioClip UpdateAudio;
    public AudioClip WithdrawAudio;
    public AudioClip ATKclip;
    public AudioClip[] birdDeployedAudio;
    public AudioClip birdClick;

    //火焰伤害
    [SerializeField]
    private bool IsFire;

    public void Awake()
    {
        originalColor = spr.color;
        TimeToChange = false;
        //UI
        gameManager =GameObject.Find("GameManager").GetComponent<GameManager>();
        TurretUIanim = TurretUI.GetComponent<Animator>();
        TurretUpdateUIanim1 = TurretUpdateUI[0].GetComponent<Animator>();
        TurretUpdateUIanim2 = TurretUpdateUI[1].GetComponent<Animator>();
        //血条
        healthSlider = GetComponentInChildren<Slider>();
        currenthealth = maxhealth;
        healthSlider.maxValue = maxhealth;
        healthSlider.value = maxhealth;
        //获取开火位置
        firePos = GetComponentInChildren<Transform>();
        //SetBulletInfor();

        circleCollider2D = GetComponent<CircleCollider2D>();
        if(circleCollider2D)
        {
            circleCollider2D.radius = minAttackRange;
            RangeUI.transform.localScale = new Vector3(minAttackRange, minAttackRange);
        }

        //动画
        birdAnim = birdSprit.GetComponentInChildren<Animator>();

        //音效
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = birdDeployedAudio[UnityEngine.Random.Range(0, birdDeployedAudio.Length)];
        audioSource.Play();
    }

    //获取敌人
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            enemyList.Add(collision.gameObject);
        }
    }

    //丢失敌人
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            enemyList.Remove(collision.gameObject);
        }
    }


    /// <summary>
    /// 发现目标并返回Target物体
    /// </summary>
    /// <returns></returns>
    public GameObject FindTarget() {
        float MIN = 100;
        int attindex = 0;
        if (enemyList.Count > 0)
        {
            hasGetEnemy = true;
            for (int i = 0; i < enemyList.Count; i++)
            {
                float distance = Vector2.Distance(enemyList[i].transform.position, transform.position);
                if (distance < MIN)
                {
                    MIN = distance;
                    attindex = i;
                }
            }
            
            return enemyList[attindex].gameObject;
        }
        hasGetEnemy = false;
        return null;
    }

    public void Update()
    {
        ChangeColorWhenHit();
        ifOnMouseDown();
        CleanList();
        CheckDie();
        UIFeedback();
        myOnMouseExit();
    }

    /// <summary>
    /// UI动画信息反馈
    /// </summary>
    public void UIFeedback()
    {
        TurretUIanim.SetBool("UIshow", IsClick);
        TurretUpdateUIanim1.SetBool("UIshow", IsClick&&!IsUpgrated);
        TurretUpdateUIanim2.SetBool("UIshow", IsClick && !IsUpgrated);
        birdAnim.SetBool("IsAttack", isAttack);
        birdAnim.SetBool("IsDead", isDead);
        RangeUI.gameObject.SetActive(IsClick);
    }

    /// <summary>
    /// 收到伤害检测
    /// </summary>
    /// <param name="damageNum"></param>
    public void takeDamage(float damageNum)
    {
        Thread thread = new Thread(new ThreadStart(flickingThread));
        thread.Start();
        TimeToChange = true;
        currenthealth -= damageNum;
        healthSlider.value = currenthealth;
    }

    /// <summary>
    /// 检测是否自己已经被打败
    /// </summary>
    public void CheckDie()
    {
        if (currenthealth <= 0)
        {
            isDead = true;
            gameObject.transform.parent.GetComponent<TileAttributes>().isOccupied = false;
            Destroy(gameObject,0.8f);
        }
       
    }

    public void ifOnMouseDown()
    { 
        if (Input.GetMouseButtonDown(0))
        { 
            Collider2D[] col = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            foreach (Collider2D c in col)
            {
                if (c == this.GetComponent<BoxCollider2D>())
                {
                    audioSource.PlayOneShot(birdClick);
                    gameManager.respondtoUI(this);

                    IsClick = !IsClick;
                }
            }
        }
        
    }

    //
    public void myOnMouseExit()
    {
        if (Vector3.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition),transform.position)>= 3.8f)
            IsClick = false;
    }

    public void CleanList() {
        try
        {
            foreach (GameObject g in enemyList)
            {
                if (g != null && g.tag != "Enemy")
                    enemyList.Remove(g);
            }
        }
        catch (InvalidOperationException e) {

        }
    }

    public void DestoryBird()
    {
        audioSource.PlayOneShot(WithdrawAudio);
        gameManager.GetMoney((int)(money * 0.5));
        currenthealth = 0;
    }

    public void OnDestroy()
    {
        currenthealth = 0;
    }

    public void RangeUpgrade() {
        //减少金钱
        //去掉另一个方向
        //增加碰撞盒和范围UI
        if(gameManager.CanAffordMoney(Rpayment))
        {
            audioSource.PlayOneShot(UpdateAudio);
            gameManager.PayMoney(Rpayment);
            IsUpgrated = true;
            minAttackRange *= ParaforRG;
            circleCollider2D.radius = minAttackRange;
            RangeUI.transform.localScale = new Vector3(minAttackRange, minAttackRange);
            IsRgUpdate = true;
        }   
    }

    public void AttackUpgrade()
    {
        //减少金钱
        //去掉另一个方向
        //增加攻击力
        if(gameManager.CanAffordMoney(Apayment))
        {
            audioSource.PlayOneShot(UpdateAudio);
            gameManager.PayMoney(Apayment);
            IsUpgrated = true;
            damage += ParaforATK;
            IsATKUpdate = true;
        } 
    }

    public void SpeedUpgrade()
    {
        //减少金钱
        //去掉另一个方向
        //增加攻速
        if(gameManager.CanAffordMoney(Spayment))
        {
            audioSource.PlayOneShot(UpdateAudio);
            gameManager.PayMoney(Spayment);
            IsUpgrated = true;
            fireRate -= ParaforSp;
            IsSpUpdate = true;
        }   
    }

    public void GetMoneyInfor(int money)
    {
        this.money = money;
    }

    public void ChangeColorWhenHit()
    {
        if (TimeToChange)
        {
            Color c = new Color(1, 150 / 255f, 150 / 255f);
            spr.color = c;
        }
        else
        {
            spr.color = originalColor;
        }
    }


    public void flickingThread()
    {
        TimeToChange = true;
        Thread.Sleep(flickingDuration);
        TimeToChange = false;
    }

    /// <summary>
    /// 确认火焰伤害
    /// </summary>
    /// <param name="isFire"></param>
    public void GetFireDamage(bool isFire)
    {
        this.IsFire = isFire;
    }
}
