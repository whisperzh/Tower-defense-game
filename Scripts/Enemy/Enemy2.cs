using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : AbstractEnemy
{
    //获取炮塔名单
    private bool firstAttack = true;
    public float offsetTime = 1f;

    //攻击音效
    
    //private float timer = 0;
    private void Update()
    {
        base.Update();

        if (FindTarget(Globle.SEARCH_ALL))
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
    

    /// <summary>
    /// 找到炮塔
    /// </summary>
    /// <returns></returns>

    private void Attack()
    {
        audioSource.PlayOneShot(ATKaudio);
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet_Enemy>().SetBulletDamage(damage);
        bullet.GetComponent<Bullet_Enemy>().setTarget(FindTarget(Globle.SEARCH_ALL).transform);
    }


}
