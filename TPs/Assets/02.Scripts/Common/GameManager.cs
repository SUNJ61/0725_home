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
        //�÷��̾� �����۷����� ����. �ش� Ŭ������ "KILLCOUNT"��� Ű�� 0�� ���� ����. �� 0���� KillCount�� �Ҵ��Ѵ�.
        KillTXT.text = "<color=#FFFFFF>KILL</color> : " + KillCount.ToString("0000"); //���ڸ� 4��° �ڸ����� ǥ��
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
        Time.timeScale = (isPaused) ? 0.0f : 1.0f; //isPaused�� true�� 0, false�� 1�� �ȴ�. 
        var playerObj = GameObject.FindGameObjectWithTag(playertag); //�÷��̾� ������Ʈ�� ã�� ����
        var scripts = playerObj.GetComponents<MonoBehaviour>(); //�÷��̾� ������Ʈ�� �ִ� ��� ��ũ��Ʈ�� �����Ѵ�.
        //MonoBehaviour��� Ŭ������ ��� �޾ұ� ������ ������Ʈ�� ��ũ��Ʈ�� �� �� �ִ�. ��, MonoBehaviour�� ��ũ��Ʈ�� �ٴ� ���.
        foreach( var script in scripts )
        {
            script.enabled = !isPaused; //��� �ϸ� ��ũ��Ʈ ��Ȱ��ȭ, ��� Ǯ�� ��ũ��Ʈ Ȱ��ȭ. 
        }
        var canvasGroup = GameObject.Find(panel_WPname).GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = !isPaused;
    }
    public void KillScore()
    {
        ++KillCount;
        KillTXT.text = "<color=#FFFFFF>KILL</color> : " + KillCount.ToString("0000");
        PlayerPrefs.SetInt(KillKey, KillCount); //KillCount�� "KILLCOUNT"Ű�� ����
        //�ش� Ŭ������ ���ȼ��� ����. �ǹ������� ��ȣȭ�ؼ� �����ؾ��Ѵ�.
    }
}
