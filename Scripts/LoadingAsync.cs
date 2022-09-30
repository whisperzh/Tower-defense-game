using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingAsync : MonoBehaviour
{
    //加载菜单文本
    public string tipTextName = null;
    private XmlDocument tipXml = new XmlDocument();
    private List<string> tipStringList = new List<string>();
    public Text tipText;

    public GameObject[] MapsIntro;
    public Text loadingText;
    public Image progressBar;
    
    private int currentValue = 0;

    private AsyncOperation operation;

    public GameObject LoadingChange;

    public bool IsLoading;
    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1;
        IsLoading = false;
        loadXml();
        tipText.text = tipStringList[Random.Range(0, tipStringList.Count)];
        if (SceneManager.GetActiveScene().name == "Loading")
        {
            MapsIntro[MenuManager.gameLevel-1].gameObject.SetActive(true);
            //启动协程
            StartCoroutine(AsyncLoading());
        }
    }

    IEnumerator AsyncLoading()
    {
        operation = SceneManager.LoadSceneAsync("game0"+ MenuManager.gameLevel);
        //阻止当加载完成自动切换
        operation.allowSceneActivation = false;

        yield return operation;
    }

    // Update is called once per frame
    void Update()
    {

        int MaxValue = 100;

        if (currentValue < MaxValue)
        {
            currentValue++;
        }

        loadingText.text = currentValue + "%";//实时更新进度百分比的文本显示  

        progressBar.fillAmount = currentValue / 100f;//实时更新滑动进度图片的fillAmount值  

        if (currentValue == 100)
        {
            loadingText.text = "按下任意键开始游戏";//文本显示完成OK  
            if(Input.anyKey && !IsLoading)
            {
                IsLoading = true;
                Invoke("LoadScene", 1f);
                Instantiate(LoadingChange);    
            }
        }
    }

    public void LoadScene()
    {
        operation.allowSceneActivation = true;//启用自动加载场景  
    }

    public void loadXml()
    {
        TextAsset textAsset = (TextAsset)Resources.Load(tipTextName);
        tipXml.LoadXml(textAsset.text);
            //(Application.dataPath + "/Scripts/" + tipTextName + ".xml");
        XmlNodeList nodeList = tipXml.SelectSingleNode("LoadingTip").ChildNodes;
        foreach(XmlNode xmlNode in nodeList)
        {
            XmlElement tip = xmlNode as XmlElement;
            tipStringList.Add(xmlNode.InnerText);
            Debug.Log(tip.GetAttribute("tips"));
        }
    }
}
