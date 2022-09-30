using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject pavement, road,spawner,destination;

    private float offset = 128;

    private Vector2 originalPos = new Vector2(-10,-5);

    public int col = 10, row = 6;
    // Start is called before the first frame update
    public int []groundMap=
   {
     0,0,0,0,0,0,0,0,0,0,
     0,0,0,0,0,0,0,0,1,3,
     0,1,1,1,1,1,1,1,1,0,
     0,1,0,0,0,0,0,0,0,0,
     0,1,0,0,0,0,0,0,0,0,
     0,2,0,0,0,0,0,0,0,0
    };

    void Start()
    {
        for (int i = 0; i < row; i++)
            for (int j = 0; j < col; j++)
            {
                GameObject gameObject;
                int sequence = i * col + j;
                switch (groundMap[sequence])
                {
                    case 0:
                        gameObject = Instantiate(pavement);
                        break;
                    case 1:
                        gameObject = Instantiate(road);
                        break;
                    case 2:
                        gameObject = Instantiate(spawner);
                        break;
                    default:
                        gameObject = Instantiate(destination);
                        break;
                 }
                gameObject.transform.position = new Vector2(originalPos.x + 2*j, originalPos.y + 2*i);
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setGround(int para,Vector2 pos)
    {
        
    }

}
