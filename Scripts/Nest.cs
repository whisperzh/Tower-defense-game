using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour
{
  
    public static bool IsDead;

    public int LIFE = 0;

    public List<GameObject> eggs;
    void Start()
    {
        IsDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (LIFE >= 3)
            IsDead = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsDead&& LIFE <= 2)
            if (collision.gameObject.tag=="Enemy")
            {
                Destroy(collision.gameObject);
                eggs[LIFE].GetComponent<SpriteRenderer>().enabled = false;
                LIFE++;
            }
    }


}
