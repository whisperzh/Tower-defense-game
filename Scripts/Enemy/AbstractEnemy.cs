using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


public abstract class AbstractEnemy : MonoBehaviour
{
    public Waypoints waypoints;
    public int flickingDuration;
    //游戏控制菜单
    private GameManager gameManager;
    private EnemySpawner enemySpawner;
    public GameObject[] turrentList;
    //敌人速度
    [Header("敌人速度")]
    public float speed = 10;
    protected float currentSpeed;
    protected bool IsMoving = true;//protect

    //生命值
    [Header("生命值")]
    public Slider healthSlider;
    public int spendLIFE = 1;
    public float maxhealth;
    private float currenthealth;

    //移动目标
    public Transform[] pos;
    public int index = 1;

    //是否具有主动攻击能力 (如果敌人是主动攻击类型一定要勾选 ActiveAttack！！)
    public bool ActiveAttack = false;

    //子弹信息
    [Header("子弹信息")]
    public GameObject bulletPrefab;
    public float damage;
    public float bulletSpeed;

    //开火的位置
    [Header("开火范围")]
    protected Transform firePos;
    public float attackingRange;

    //攻击频率
    [Header("攻击频率")]
    public float fireRate = 1f;
    protected float nextFire = 0;

    //被击败后获得金钱数
    [Header("被击败后获得金钱数")]
    public int money = 0;
    private bool IsGetMoney = false;
    //是否被塔防检测到
    [Header("是否被塔防检测到")]
    public bool Becatched = false;
    public bool IsDead = false;

    //动画角色
    [Header("动画角色")]
    private Color originalColor;
    public GameObject enemySprit;
    protected Animator enemyAnim;
    protected bool isWalk;
    protected bool isAttack=true;
    protected bool isDead;
    public SpriteRenderer spr;
    public bool TimeToChange;

    //音效
    [Header("音效")]
    protected AudioSource audioSource;
    public AudioClip ATKaudio;

    // Start is called before the first frame update
    public void Start()
    {
        TimeToChange = false;
        originalColor = spr.color;   
        enemyAnim = enemySprit.GetComponentInChildren<Animator>();
        firePos = transform;
        gameManager = FindObjectOfType<GameManager>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        //血量设置
        currenthealth = maxhealth;    
        healthSlider.maxValue = maxhealth;
        healthSlider.value = maxhealth;

        currentSpeed = speed;
        pos = waypoints.positions;
        //如果敌人是主动攻击类型一定要勾选 ActiveAttack！！
        if (ActiveAttack)
        {
            BulletInitialize();
        }
        //攻击范围设置
        attackingRange += UnityEngine.Random.Range(-0.3f, 0.3f);
        //动画设置
        isWalk = true;
        isDead = false;
        //音效设置
        audioSource = GetComponent<AudioSource>();

    }

    /// <summary>
    /// 传入子弹信息
    /// </summary>
    public void BulletInitialize()
    {
        bulletPrefab.GetComponent<Bullet_Enemy>().SetBulletDamage(damage);
        bulletPrefab.GetComponent<Bullet_Enemy>().SetBulletSpeed(bulletSpeed);
    }

    public void Update()
    {
        ChangeColorWhenHit();
        turrentList = GameObject.FindGameObjectsWithTag("turrents");
        Move();
        AnimUpdate();
    }

    public void AnimUpdate()
    {
        try
        {
            enemyAnim.SetBool("IsWalk", isWalk);
            enemyAnim.SetBool("IsDead", isDead);
        }
        catch (Exception e) { }
    }

    /// <summary>
    /// 敌人移动
    /// </summary>
    public void Move()
    {
        if (index > pos.Length - 1)
        {
            gameManager.LIFEdecrease(spendLIFE);
            Destroy(this.gameObject);
            //IsDead = true;
            return;
        }

        //遇到防御塔防就停止移动
        Vector2 dir = (pos[index].position - this.transform.position).normalized;
        Debug.DrawRay(transform.position, dir, Color.red);
        RaycastHit2D infor = Physics2D.Raycast(transform.position, dir, 0.5f, LayerMask.GetMask("Turrent"));
        Vector2 dirc = new Vector2(transform.position.x + dir.x, transform.position.y + dir.y);
        Collider2D[] col = Physics2D.OverlapPointAll(dirc);
       // Debug.Log(gameObject.name + dirc.x + "," + dirc.y);
        if (infor.collider != null)
        {
            //Debug.Log("name " + infor.collider.gameObject.name);
            GameObject obj = infor.collider.gameObject;
            string type = obj.GetComponent<AbstractTurrent>().typeofTurrent;
            if (type == "DefendTurrent")
            {
               // Debug.Log("停止移动1");
                foreach (Collider2D c in col)
                {
                    if (c == obj.GetComponent<BoxCollider2D>())
                    {
                        StopMoving();
                        IsMoving = false;
                       // Debug.Log("停止移动2");
                     }
                }               
            }
            else
            {
                if (IsMoving == false)
                {
                    Debug.Log("开始移动");
                    Invoke("BackNormalMovingSpeed", 0.5f);
                    IsMoving = true;
                }
            }
        }
        else
        {
            if (IsMoving == false)
            {
                Invoke("BackNormalMovingSpeed", 0.5f);
                IsMoving = true;
            }
        }
        

        transform.Translate(dir * Time.deltaTime * currentSpeed);
        if (Vector2.Distance(pos[index].position, transform.position) < 0.1f)
        {
            index++;
        }

        if (currenthealth <= 0 )
        {
            if (!IsGetMoney)
            {
                gameManager.GetMoney(money);
                IsGetMoney = true;
            }
            isDead = true;
            //StopMoving();
            this.tag ="Untagged";
            this.currentSpeed = 0;
            this.GetComponent<Collider2D>().enabled = false;
            Destroy(this.gameObject,0.7f); 
        }
    }
   
    /// <summary>
    /// 停止移动
    /// </summary>
    public void StopMoving()
    {
        isWalk = false;
        currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime * 12);
    }

    /// <summary>
    /// 返回正常速度
    /// </summary>
    public void BackNormalMovingSpeed()
    {
        if (!isDead) {
            isWalk = true;
            currentSpeed = speed;
        }
    }

    /// <summary>
    /// 收到伤害
    /// </summary>
    /// <param name="damage"></param>
    public void takeDamage(float damage)
    {
        Thread thread = new Thread(new ThreadStart( flickingThread));
        thread.Start();
        TimeToChange = true;
        currenthealth -= damage;
        healthSlider.value = currenthealth;
    }

    public void OnDestroy()
    {
        enemySpawner.Set_EnemyDown();
        EnemySpawner.AliveEnemy--;       
    }

    public GameObject FindTarget(int MODE)
    {
        float shortestDist = 500;
        if (MODE == Globle.SEARCH_ALL)
        {
            foreach (GameObject turrent in turrentList)
            {
                float dist = Vector3.Distance(turrent.transform.position, this.gameObject.transform.position);
                if (dist <= (attackingRange))
                {
                    if (dist <= shortestDist)
                    {
                        shortestDist = dist;
                        return turrent;
                    }
                }
            }
            return null;
        }
        else if (MODE == Globle.SEARCH_NEAR)
        {
            foreach (GameObject turrent in turrentList)
            {
                float dist = Vector3.Distance(turrent.transform.position, this.gameObject.transform.position);
                if (dist <= (attackingRange))
                {
                    if (dist <= shortestDist)
                    {
                        shortestDist = dist;
                        if (turrent.GetComponent<Turrent2>() != null)
                            return turrent;
                    }
                }
            }
            return null;
        }
        else {
            return null;
        }
    }

    public void ChangeColorWhenHit() {
        if (TimeToChange)
        {
            Color c = new Color(1, 150/255f, 150/255f);
            spr.color =c;
        }
        else {
            spr.color = originalColor;
        }
    }


    public void flickingThread() {
        TimeToChange = true;
            //Debug.Log("ht");
            Thread.Sleep(flickingDuration);
        TimeToChange = false;
    }

}
