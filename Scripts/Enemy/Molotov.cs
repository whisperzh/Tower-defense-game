using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov : Bullet_Enemy
{
    private float a, b, c;
    private float dist;
    public GameObject Fire;
    Vector3 startpos, center;
    private bool stage1 = true;
    private bool isExploded = false;
    private AudioSource audioSource;
    private void Start()
    {
        startpos = this.transform.position;
        dist = Vector2.Distance(startpos, target.position);
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, 100), Space.Self);
        bool gotTarget = false;
        if (target != null)
        {
            center = calculatePara(startpos, target.position);
            gotTarget = true;
            if (Vector3.Distance(transform.position, target.position) <= 0.5f)
            {
           
                if (!isExploded)
                    burn();
            }
        }
        else
            Destroy(this.gameObject);

        if (gotTarget)
            transform.up = (target.position - transform.position).normalized;

        //弧形插值 
        if (Vector3.Distance(transform.position, center) >= 0.5f && stage1)
        {
            transform.position = Vector3.Lerp(transform.position, center, Time.deltaTime*speed);
        }
        else
        {
            stage1 = false;
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);
        }
    }

    public void burn()
    {

        float PosX = target.position.x;

        float PosY = target.position.y < 0 ? (int)(target.position.y - 0.5f) : (int)(target.position.y + 0.5f);

        GameObject temp = Instantiate(Fire,new Vector3(PosX,PosY,-0.01f), Quaternion.identity);
        temp.GetComponent<buringFire>().damage = damage;//delivering damage
        isExploded = true;

        Destroy(this.gameObject);
    }

    public Vector2 calculatePara(Vector2 startpoint, Vector2 endpoint)
    {

        Vector2 center = Vector2.zero;
        float cx = (startpoint.x + endpoint.x) / 2;
        if (startpoint.y > endpoint.y)
        {
            center = startpoint;
            center = new Vector2(cx, center.y + 2);
        }
        else
        {
            center = endpoint;
            center = new Vector2(cx, center.y + 2);
        }

        return center;

    }
    public float getFunctionY(float x)
    {
        float res = a * x * x + b * x + c;
        return res;
    }
}