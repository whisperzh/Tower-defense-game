using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Wave
{
    public string name;

    //public GameObject enemyprefab;
    [HideInInspector]
    public int count;
    public float rate;
    public SingleUnit[] Units; 
}
