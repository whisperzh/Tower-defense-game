using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileAttributes : MonoBehaviour
{
    public bool isOccupied = false;
    [HideInInspector]
    public GameObject turrentGo;

    private SpriteRenderer sr;
    public Sprite selected,restrictedArea;
    private GameManager gameManager;
    private Sprite origin;
    private string ATG = "AttackTurrentGround";
    private string DTG = "DefendTurrentGround";

    Vector3 startpoint;
    Vector3 endpoint;

    //private string turrentTag;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        origin = sr.sprite;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        startpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        endpoint = Camera.main.transform.position;
        endpoint.z = 11;
        //如果已经选择塔防
        if (gameManager.GetTurrentPermission() && gameManager.GetCurrentTurrent() != null)
        {
            sr.enabled = true;
            TileSelectedandBuile(gameManager.GetCurrentTurrent().turrentType.ToString());
        }
        else
            sr.enabled = false;
       
       GetComponent<BoxCollider2D>().enabled = !isOccupied;
      

    }

    /// <summary>
    /// 砖块检测
    /// </summary>
    public void TileSelectedandBuile(string turrentTag)
    {
        //检测塔防种类以限制放置限制
        switch (turrentTag)
        {
            case "AttackTurrent":
                turrentTag = ATG;
                break;
            case "DefendTurrent":
                turrentTag = DTG;
                break;
            default:
                break;
        }
        
        Debug.DrawRay(startpoint, endpoint, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(startpoint, endpoint, 1000f, LayerMask.GetMask("Tiles"));

        if (transform.tag == turrentTag)
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    //Debug.Log(hit.collider.name);
                    sr.sprite = selected;
                    if (Input.GetMouseButton(0) && !isOccupied)
                    {
                        isOccupied = true;
                        sampleTurrent sampleTurrent = gameManager.GetCurrentTurrent();
                        buildTurrent(sampleTurrent.turrentPrefab);//放置塔防
                        gameManager.PayMoney(sampleTurrent.cost);//确认放置后再扣去金钱
                        sampleTurrent.Set_canSelect(false);//确认放置后再进行冷却
                        gameManager.ReSetCurrentTurrent();//重置所选炮塔信息
                        sr.sprite = origin;
                        //Debug.Log("生成塔防 " + sampleTurrent.turrentPrefab.name);
                    }
                }
            }else
                sr.sprite = origin;
        }

        else {
            sr.sprite = restrictedArea;
        }
      
    }

    public void OnMouseExit()
    {
        try
        {
            if (transform.tag == gameManager.GetCurrentTurrent().turrentType.ToString())
                sr.sprite = origin;
      
        }
        catch (NullReferenceException e) {

        }
    }

    /// <summary>
    /// 生成塔防
    /// </summary>
    /// <param name="turrentPrefab"></param>
    public void buildTurrent(GameObject turrentPrefab)
    {
        GameObject g = GameObject.Instantiate(turrentPrefab, transform.position, Quaternion.identity);
        g.transform.parent = this.transform;
    }

}
