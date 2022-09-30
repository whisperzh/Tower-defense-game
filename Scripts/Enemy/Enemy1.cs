using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy1 : AbstractEnemy
{
    public float offsetTime;
    private bool firstAttack = true;

    private float timer = 0;

    private void Update()
    {
        base.Update();
        if (FindTarget(Globle.SEARCH_NEAR))
        {

            if (firstAttack)
            {
                nextFire = Time.time + offsetTime;
                firstAttack = false;
            }

            StopMoving();
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Attack();
            }
        }
        else
        {
            BackNormalMovingSpeed();
            firstAttack = true;
        }
    }

    private void Attack()
    {
        audioSource.PlayOneShot(ATKaudio);
        try
        {
            FindTarget(Globle.SEARCH_NEAR).GetComponent<AbstractTurrent>().takeDamage(damage);
        }
        catch (Exception e) { }
    }
}
