using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CancelSelection : MonoBehaviour
{
    Toggle[] obj;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            obj = GetComponentsInChildren<Toggle>();
            foreach (Toggle t in obj)
                t.isOn = false;
        }
    }   
}
