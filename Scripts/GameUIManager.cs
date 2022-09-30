using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    

    public GameManager gameManager;
    public EnemySpawner enemySpawner;
    private AudioSource gameManageraudioSource;

    //进度条UI
    public Slider ProgressSlider;
    private float psSliderValue;
    public Image WaveNumImage;
    public Sprite[] WaveNum4UI;

    //HideUI
    public GameObject PauseUI;
    public GameObject WinUI;
    public GameObject LoseUI;
    private bool isPause;

    //Money
    private bool ICTime2Change;
    public Image Num1, Num2;
    public Sprite[] NumImage;

    //音效
    private AudioSource audioSource;
    public AudioClip ButtonAudio;
    public AudioClip ChoseTurret;
    public AudioClip CanNotAffordMoney;

    //

    //
    public GameObject SetUI;

    //音效
    

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;

        ICTime2Change = false;
        SetUI.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        gameManageraudioSource = gameManager.GetComponent<AudioSource>();
        audioSource.clip = ButtonAudio;
        try
        {
            isPause = false;
            PauseUI.gameObject.SetActive(isPause);
            WinUI.gameObject.SetActive(isPause);
            LoseUI.gameObject.SetActive(isPause);
        }
        catch (Exception e ) {

        }
    }

    private void Update()
    {
        ProgressSlider.value = Mathf.Lerp(ProgressSlider.value, enemySpawner.Get_EnemyDownVsTotalEnemyNum(), Time.deltaTime);
        WaveNumImage.sprite = WaveNum4UI[enemySpawner.Get_WaveNum()];
        UpdateMoney(gameManager.GetTotalMoney());
        ChangeColor();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUI_press();
        }
    }

    /// <summary>
    /// 更新Money图片
    /// </summary>
    public void UpdateMoney(int TotalMoney)
    {
        int num1 = TotalMoney / 10;
        int num2 = TotalMoney % 10;
        Num1.sprite = NumImage[num1];
        Num2.sprite = NumImage[num2];
    }


    public void PauseUI_press()
    {
        audioSource.Play();
        isPause = !isPause;
        if(isPause)
        {
            gameManageraudioSource.Pause();
            gameManager.StartGameAnim.GetComponent<AudioSource>().Pause();
        }
        else
        {
            gameManageraudioSource.Play();
            gameManager.StartGameAnim.GetComponent<AudioSource>().Play();
        }
        PauseUI.gameObject.SetActive(isPause);
        Time.timeScale = (isPause == true ? 0 : 1);
    }

    public void NextLevel()
    {
        audioSource.Play();
        if (MenuManager.gameLevel == 3)
        {
            SceneManager.LoadScene("Menu");
        }
        else
        {
            MenuManager.gameLevel += 1;
            SceneManager.LoadScene("Loading");
        }

    }

    public void B2M()
    {
        audioSource.Play();
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        audioSource.Play();
        Application.Quit();
    }

    public void OpenSet()
    {
        audioSource.Play();
        SetUI.gameObject.SetActive(true);
    }

    public void ReStartLevel()
    {
        audioSource.Play();
        SceneManager.LoadScene("game0" + MenuManager.gameLevel);
    }

    public void ShowWinUI(bool IsWin)
    {
        WinUI.gameObject.SetActive(IsWin);
    }

    public void ShowLoseUI()
    {
        LoseUI.gameObject.SetActive(true);
    }

    /// <summary>
    /// 成功选择塔防音效
    /// </summary>
    public void PlayChoseTurret()
    {
        audioSource.PlayOneShot(ChoseTurret);
    }


    /// <summary>
    /// 发出钱不够的声音
    /// </summary>
    public void PlayCanNotAffordMoney()
    {
        audioSource.PlayOneShot(CanNotAffordMoney);

        Thread thread = new Thread(new ThreadStart(flickingThread));
        thread.Start();
    }

    public void ChangeColor()
    {
        if (ICTime2Change)
        {
            Color c = new Color(0.8f,0.27f,0.23f);
            Num1.color = c;
            Num2.color = c;
        }
        else
        {
            Num1.color = Color.white;
            Num2.color = Color.white;
        }
    }


    public void flickingThread()
    {
        ICTime2Change = true;
        Thread.Sleep(100);
        ICTime2Change = false;
    }

    
}
