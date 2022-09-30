using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrent2 : AbstractTurrent
{

    private void Update()
    {

        birdAnim.SetBool("IsSpeedUpdate", IsSpUpdate);
        base.Update();

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
                if(IsSpUpdate)
                {
                    nextFire = Time.time+0.25f;
                }
                else
                {
                    nextFire = Time.time + 0.5f;
                }
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
            FindTarget().GetComponent<AbstractEnemy>().takeDamage(damage);
        }
    }
}
