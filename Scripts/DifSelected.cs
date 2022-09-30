using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifSelected : MonoBehaviour
{
    public Image HandleImage;
    public Sprite[] DifSelectImage;
    public Text tipText;
    private Slider slider;
    public string[] text = new string[3];

    public enum SliderType { EnemyNumber, TurretLimit, MoneyStart }

    public SliderType sliderType;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleImage.sprite = DifSelectImage[(int)slider.value - 1];
        tipText.text = text[(int)slider.value - 1];

        switch (sliderType)
        {
            case SliderType.EnemyNumber:
                GameManager.EnemyNumber = (int)slider.value;
                break;
            case SliderType.MoneyStart:
                GameManager.MoneyStart = (int)slider.value;
                break;
            case SliderType.TurretLimit:
                GameManager.TurretLimit = (int)slider.value;
                break;
        }
    }
}
