using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 方便Turret选择脚本 只作用于Toggle上
/// </summary>
public class TurretButtonChoose : MonoBehaviour
{
    public enum TurretTypes { Turret_1,Turret_2, Turret_3, Turret_4, Turret_5};
    public TurretTypes type;

    public int getTurrentNum()
    {
        int num = -1;
        switch (type)
        {
            case TurretTypes.Turret_1:
                num = 0;
                break;
            case TurretTypes.Turret_2:
                num = 1;
                break;
            case TurretTypes.Turret_3:
                num = 2;
                break;
            case TurretTypes.Turret_4:
                num = 3;
                break;
            case TurretTypes.Turret_5:
                num = 4;
                break;
        }
        return num;
    }
}
