using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SingleUnit
{
    public enum EnemyTypes { Enemy_1, Enemy_2, Enemy_3, Enemy_4, Enemy_5 };
    public enum WayTypes { Way1,Way2,Way3}

    [Range(0,10)]
    public float waitingTime_ahead;

    public EnemyTypes enemyType;
    public WayTypes wayNum;

    public int GetEnemyType()
    {
        int num = 0;

        switch(enemyType)
        {
            case EnemyTypes.Enemy_1:
                num = 0;
                break;
            case EnemyTypes.Enemy_2:
                num = 1;
                break;
            case EnemyTypes.Enemy_3:
                num = 2;
                break;
            case EnemyTypes.Enemy_4:
                num = 3;
                break;
            case EnemyTypes.Enemy_5:
                num = 4;
                break;
        }

        return num;
    }

    public int GetWayType()
    {
        int num = 0;

        switch (wayNum)
        {
            case WayTypes.Way1:
                num = 0;
                break;
            case WayTypes.Way2:
                num = 1;
                break;
            case WayTypes.Way3:
                num = 2;
                break;
        }

        return num;
    }


}
