using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager G_instance;
    public bool isGameOver = false;
    public bool isInvenopen = false;

    private Text KillTXT;
    private string CanvasObj = "Canvas_UI";
    private string KillKey = "KILLCOUNT";
    private int KillCount = 0;
    void Awake()
    {
        if (G_instance == null)
            G_instance = this;
        else if(G_instance != this)
            Destroy(G_instance);
        DontDestroyOnLoad(G_instance);
        KillTXT = GameObject.Find(CanvasObj).transform.GetChild(7).GetComponent<Text>();
        LoadGameData();
    }
    private void LoadGameData()
    {
        KillCount = PlayerPrefs.GetInt(KillKey, 0);
        //플레이어 프리퍼런스의 약자. 해당 클래스는 "KILLCOUNT"라는 키에 0의 값을 저장. 이 0값을 KillCount에 할당한다.
        KillTXT.text = "<color=#FFFFFF>KILL</color> : " + KillCount.ToString("0000"); //숫자를 4번째 자리까지 표현
    }
    void Update()
    {
        
    }
    private bool isPaused = false;
    private string playertag = "Player";
    private string panel_WPname = "Panel_Weapon";
    public void OnPauseClick()
    {
        isPaused = !isPaused;
        Time.timeScale = (isPaused) ? 0.0f : 1.0f; //isPaused가 true면 0, false면 1이 된다. 
        var playerObj = GameObject.FindGameObjectWithTag(playertag); //플레이어 오브젝트를 찾아 저장
        var scripts = playerObj.GetComponents<MonoBehaviour>(); //플레이어 오브젝트에 있는 모든 스크립트를 저장한다.
        //MonoBehaviour라는 클래스를 상속 받았기 때문에 오브젝트에 스크립트를 달 수 있다. 즉, MonoBehaviour는 스크립트를 다는 기능.
        foreach( var script in scripts )
        {
            script.enabled = !isPaused; //퍼즈를 하면 스크립트 비활성화, 퍼즈를 풀면 스크립트 활성화. 
        }
        var canvasGroup = GameObject.Find(panel_WPname).GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = !isPaused;
    }
    public void KillScore()
    {
        ++KillCount;
        KillTXT.text = "<color=#FFFFFF>KILL</color> : " + KillCount.ToString("0000");
        PlayerPrefs.SetInt(KillKey, KillCount); //KillCount를 "KILLCOUNT"키에 저장
        //해당 클래스는 보안성이 없다. 실무에서는 암호화해서 저장해야한다.
    }
}
