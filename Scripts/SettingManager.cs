using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    private GameObject SetUI;
    public Slider BGMslider;
    public static float BGMvolume = 1;
    public AudioSource BGM;

    private AudioSource audioSource;
    public AudioClip ButtonAudio;

    //分辨率
    List<string> options = new List<string>();
    public Dropdown resolutionDropdown;
    Resolution[] resolutions;
    // Start is called before the first frame update
    void Start()
    {
        SetUI = transform.gameObject;
        int currentResolutionIndex = 0;
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();     

        for (int i = resolutions.Length - 1; i>=10; i--)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;

            options.Add(option);
            //Debug.Log(resolutions[i] + " " + i);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = resolutions.Length -1 - i;
                Debug.Log("currentResolutionIndex" + currentResolutionIndex);
            }
            
        }

        Debug.Log(Screen.currentResolution);
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        BGMslider.value = BGMvolume;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        BGMvolume = BGMslider.value;
        BGM.volume = BGMvolume;
    }

    public void CloseSet()
    {
        audioSource.PlayOneShot(ButtonAudio);
        SetUI.gameObject.SetActive(false);
    }

    public void SetResolution(int currentResolutionIndex)
    {
        Resolution resolution = resolutions[resolutions.Length - 1 - currentResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height , Screen.fullScreen);
        Debug.Log("当前屏幕分辨率"+Screen.currentResolution);
    }

    public void SetFullscreen(Toggle isFullscreen)
    {
        Debug.Log("IsFullscreen:" + isFullscreen.isOn);
        Screen.fullScreen = isFullscreen.isOn;
    }


}
