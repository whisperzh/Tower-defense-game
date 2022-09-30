using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Turrent : AbstractBullet
{

    private void Update()
    {
        bool gotTarget = false;
        if (target != null)
        {
            gotTarget = true;
            if (Vector3.Distance(transform.position, target.position) <= 0.5f)
            {
                if(target.GetComponent<AbstractEnemy>())
                {
                    target.GetComponent<AbstractEnemy>().takeDamage(damage);
                }
                Destroy(this.gameObject);
            }
        }
        else
            Destroy(this.gameObject);

        if (gotTarget)
            transform.up = (target.position - transform.position).normalized;

        transform.Translate(Vector2.up * speed * Time.deltaTime);

        //Destroy(this.gameObject, 1);
    }

}
