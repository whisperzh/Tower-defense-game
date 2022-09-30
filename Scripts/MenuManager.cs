using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public GameObject MenuUI;
    public GameObject SetUI;
    public GameObject SeletedUI;
    //SelectedMenu
    public GameObject[] Maps;
    public int mapNum;
    //选择关卡
    public static int gameLevel = 1;
    //按钮音效
    private AudioSource audioSource;
    public AudioClip ButtonAudio;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = ButtonAudio;
        MenuUI.gameObject.SetActive(true);
        SeletedUI.gameObject.SetActive(false);
        SetUI.gameObject.SetActive(false);
        mapNum = 0;
        Maps[mapNum].gameObject.SetActive(true);
    }

    public void M2S()
    {
        audioSource.Play();
        MenuUI.gameObject.SetActive(false);
        SeletedUI.gameObject.SetActive(true);
    }

    public void S2M()
    {
        audioSource.Play();
        MenuUI.gameObject.SetActive(true);
        SeletedUI.gameObject.SetActive(false);
    }

    public void OpenSet()
    {
        audioSource.Play();
        SetUI.gameObject.SetActive(true);
    }


    public void PreMap()
    {
        audioSource.Play();
        mapNum = (mapNum - 1 + Maps.Length) % Maps.Length;
        Maps[mapNum].gameObject.SetActive(true);
        for (int n = 0; n< Maps.Length;n++)
        {
            if(n!= mapNum)
            {
                Maps[n].gameObject.SetActive(false);
            }
        }
    }

    public void NextMap()
    {
        audioSource.Play();
        mapNum = (mapNum + 1 ) % Maps.Length;
        Maps[mapNum].gameObject.SetActive(true);
        for (int n = 0; n < Maps.Length; n++)
        {
            if (n != mapNum)
            {
                Maps[n].gameObject.SetActive(false);
            }
        }
    }

    public void postChoise(int scene)
    {
        audioSource.Play();
        gameLevel = scene;
        SceneManager.LoadScene("Loading");
    }

    public void ExitGame()
    {
        audioSource.Play();
        Application.Quit();
    }
}
