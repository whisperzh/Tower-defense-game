using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractBullet : MonoBehaviour
{
    //伤害
    [HideInInspector]
    public float damage;
    //射速
    public float speed;
    //目标
    protected Transform target;

    /// <summary>
    /// 设置子弹目标
    /// </summary>
    /// <param name="target"></param>
    public void setTarget(Transform target)
    {
        this.target = target;
    }

    /// <summary>
    /// 设置子弹伤害
    /// </summary>
    /// <param name="damage"></param>
    public void SetBulletDamage(float damage)
    {
        this.damage = damage;
    }

    /// <summary>
    /// 设置子弹速度
    /// </summary>
    /// <param name="speed"></param>
    public void SetBulletSpeed(float speed)
    {
        this.speed = speed;
    }
}