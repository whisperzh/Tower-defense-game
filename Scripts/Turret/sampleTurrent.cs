using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class sampleTurrent {
    public string name;

    public GameObject turrentPrefab;
    public int cost;
    public int UpgaradeCost;
    public float CD ;
    private float WaitTime;

    public Toggle ToggleButton;
    public Image TurretImage;

    private bool canSelect = true;
    public TurrentType turrentType;
    public enum TurrentType {
        AttackTurrent,
        DefendTurrent
    }

    /// <summary>
    /// CD时间计算
    /// </summary>
    public void CoolDownTimeCal()
    {
        ToggleButton.interactable = canSelect;
        if (canSelect)
        {
            WaitTime = CD;
            TurretImage.fillAmount = 0;
            
        }
        
        if(!canSelect)
        {
            //ToggleButton.interactable = canSelect;
            ToggleButton.isOn = false;
            WaitTime -= Time.deltaTime;
            TurretImage.fillAmount = 1 - (CD - WaitTime) / CD;
            if(WaitTime <=0)
            {
                canSelect = true;
            }
        }
        
    }

    public void Set_canSelect(bool canSelect)
    {
        this.canSelect = canSelect;
    }

    public bool Get_canSelect()
    {
        return canSelect;
    }
}
