using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrent1 : AbstractTurrent
{
    
    private void Update()
    {
        base.Update();
      
        //SearchEnemy();
        if (FindTarget())
        {

            Transform dir = FindTarget().transform;
            float turnDir = (dir.position.x - birdSprit.transform.position.x) * birdSprit.transform.localScale.x;
            //Debug.Log("dir " + turnDir);

            if (turnDir < 0 && !isDead)
            {
                if (birdSprit.transform.localScale.x > 0)
                    birdSprit.transform.localScale = new Vector3(-1, 1, 0);
                else
                    birdSprit.transform.localScale = new Vector3(1, 1, 0);
            }
            isAttack = true;
            if (firstAttack)
            {
                nextFire = Time.time + 0.2f;
                firstAttack = false;
            }

            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Attack();
            }
        }
        else
        {
            isAttack = false;
            FindTarget();
            firstAttack = true;
        }
    }

    private void Attack()
    {
        if (FindTarget())
        {
            if(ATKclip!=null)
            {
                audioSource.PlayOneShot(ATKclip);
            }
            GameObject bullet = Instantiate(bulletPrefab, firePos.position, Quaternion.identity);
            bullet.GetComponent<Bullet_Turrent>().SetBulletSpeed(bulletSpeed);
            bullet.GetComponent<Bullet_Turrent>().SetBulletDamage(damage);
            bullet.GetComponent<Bullet_Turrent>().setTarget(FindTarget().transform);
            bullet.transform.parent = this.transform;
        }
    }
  
}
