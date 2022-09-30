using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buringFire : MonoBehaviour
{
    // Start is called before the first frame update
    public float duration;//持续时长
    public float interval;//间隔
    [HideInInspector]
    public float damage;

    private AudioSource audioSource;
    public AudioClip cracksound;

    public AbstractTurrent[] allExistingTurrent;
    public List<AbstractTurrent> Targets;

    //ContactFilter2D cd;
    private bool collectingData = true;

    float nextFire = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(cracksound);

        Destroy(gameObject, duration);
        //cd = new ContactFilter2D();
    }

    // Update is called once per frame
    void Update()
    {

        Targets.Clear();
        allExistingTurrent = GameObject.FindObjectsOfType<AbstractTurrent>();

        foreach (AbstractTurrent abst in allExistingTurrent)
        {
            if (Vector3.Distance(abst.gameObject.transform.position, transform.position) <= 2.9f)
            {
                if (abst != null)
                    Targets.Add(abst);
            }

        }

        if (Time.time > nextFire)
        {
            nextFire = Time.time + interval;
            foreach (AbstractTurrent g in Targets)
            {
                try
                {
                    //Debug.Log("燃烧弹伤害：" + damage);
                    g.gameObject.GetComponent<AbstractTurrent>().takeDamage(damage);
                }
                catch (Exception e) { }
            }
        }

    }
  
}
