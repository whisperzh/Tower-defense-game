using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [HideInInspector]
    public int num = 0;

    public  Transform []positions;
    private void Awake()
    {
        positions = gameObject.GetComponentsInChildren<Transform>();
        num = positions.Length;
    }
}
