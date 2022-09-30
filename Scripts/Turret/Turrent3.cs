using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrent3 : AbstractTurrent
{
    private void Update()
    {
        birdAnim.SetBool("IsRangeUpdate", IsRgUpdate);

        base.Update();
        //SearchEnemy();
        if (FindTarget())
        {
            Transform dir = FindTarget().transform;
            float turnDir = (dir.position.x - birdSprit.transform.position.x) * birdSprit.transform.localScale.x;
            //Debug.Log("dir " + turnDir);

            if (turnDir > 0 && !isDead)
            {
                if (birdSprit.transform.localScale.x > 0)
                    birdSprit.transform.localScale = new Vector3(-1, 1, 0);
                else
                    birdSprit.transform.localScale = new Vector3(1, 1, 0);
            }

            isAttack = true;

            if (firstAttack)
            {
                nextFire = Time.time + 0.3f;
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
        if (ATKclip != null)
        {
            audioSource.PlayOneShot(ATKclip);
        }
        if (FindTarget())
        {
            foreach(GameObject enemy in enemyList)
            {
                enemy.GetComponent<AbstractEnemy>().takeDamage(damage);
            }
        }
    }
}
