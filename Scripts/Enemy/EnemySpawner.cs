using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static int AliveEnemy = 0;
    public Wave[] waves;
    private bool IsFinal = false;
    private bool IsOVER = false;
    public GameObject[] enemyType;
    public Waypoints[] wayPoints;

    private int TotalEnemyNum;
    private int EnemyDownNum;

    private int waveCount;

    private void Start()
    {
        EnemyDownNum = 0;
        TotalEnemyNum = 0;
        waveCount = 0;
        
        foreach (Wave wave in waves)
        {

            //60% 80% 100%
            wave.count = (int)(wave.Units.Length * (1- 0.2f * (3 - GameManager.EnemyNumber)));
            TotalEnemyNum += wave.count;
        }

        Debug.Log("TotalEnemyNum:"+TotalEnemyNum);
        StartCoroutine("SpawnEnermy");
    }

    IEnumerator SpawnEnermy() {
        foreach (Wave wave in waves)
        {
            yield return new WaitForSeconds(wave.rate);

            waveCount++;
            if (waveCount == waves.Length)
            {
                IsFinal = true;
                Debug.Log("IsFinalWave!");
            }                     
               
            for (int i = 0; i < wave.count; i++)
            {
                if (i!= wave.count-1)
                    yield return new WaitForSeconds(wave.Units[i].waitingTime_ahead);
                SingleUnit singleUnit = wave.Units[i];
                GameObject enemy = Instantiate(enemyType[wave.Units[i].GetEnemyType()]) as GameObject;
                enemy.GetComponent<AbstractEnemy>().waypoints= wayPoints[singleUnit.GetWayType()];
                enemy.transform.parent = transform;
                enemy.transform.position = new Vector3(transform.position.x, transform.position.y,-0.02f);
                AliveEnemy++;     
            }

            while (AliveEnemy > 0)
                yield return 0;

            
        }
    }

    public float Get_EnemyDownVsTotalEnemyNum()
    {
        float num = (float)EnemyDownNum/ (float)TotalEnemyNum;
        return num;
    }

    public void Set_EnemyDown()
    {
        EnemyDownNum++;
    }

    public bool Get_IsFinal()
    {
        return IsFinal;
    }
   
    public bool Get_IsWin()
    {
        return TotalEnemyNum == EnemyDownNum;
    }

    public int Get_WaveNum()
    {
        return (waves.Length < waves.Length-waveCount+1 ? waves.Length: waves.Length - waveCount + 1);
    }

}
