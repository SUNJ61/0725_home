using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
//enemy가 태어나는 로직과 더불어 게임전체를 아우르는 기능을 조정, 즉 게임 전체를 조정 클래스
//1. 적 프리팹, 2. 태어날 위치 3. 시간 간격(몇초마다 태어나는가) 4. 몇마리 까지 태어나는가
//게임매니저는 게임 전체를 컨트롤 해야하므로 접근이 쉬어야함, static변수를 만든후 이 변수가 대표해서 게임매니저에 접근하도록 한다.
//이것은 무분별한 객체 생성을 막고 하나만 생성하게 하는 기법이다. -> 싱글톤 기법
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Text Killtxt;
    public static int Killcount = 0;

    public GameObject Zom_fb; //1번
    public GameObject Monster_fb;
    public GameObject Skel_fb;
    public Transform[] point; //2번
    public GameObject[] Mob;
    private CanvasGroup Inventory;

    private float timePrev_Z; //3번 - 과거시 저장용
    private float timePrev_M; //배열로 묶으면 깔끔할듯
    private float timePrev_S;
    private float timePrev;

    private float SpawnTime_Z = 3.0f; //3번 - 3초간격
    private float SpawnTime_M = 8.0f;
    private float SpawnTime_S = 5.0f;
    private float SpawnTime = 3.0f;

    private int MaxCount_Z = 10; //4번
    private int MaxCount_M = 3;
    private int MaxCount_S = 5;
    private int MaxCount_MOB = 10;

    private string Zom = "ZOMBIE";
    private string Mon = "MONSTER";
    private string Skel = "SKELETON";
    private string mob = "MOB";
    private string SP = "SpawnPoints";
    public string player = "Player";
    private string invenStr = "Inventory";

    private bool IsOpen = false;
    void Start()
    {
        //하이라키에서 SpawnPoints라는 오브젝트를 찾고 그 오브젝트의 속한 위치 컴포넌트들을 저장한다.(##자기 자신위치 정보 포함##)
        point = GameObject.Find(SP).GetComponentsInChildren<Transform>(); //동적 할당
        timePrev_Z = Time.time; // 업데이트로 내려가면 과거시간이 됨.
        timePrev_M = Time.time;
        timePrev_S = Time.time;
        timePrev = Time.time;
        Inventory = GameObject.Find(invenStr).GetComponent<CanvasGroup>();

        Inventory.blocksRaycasts = IsOpen;
        Inventory.alpha = 0f;

        Instance = this; // 객체를 1개만 생성, Instance를 통해 해당 클래스안에 public으로 생성된 변수나 매소드에 접근 할 수 있다.
    }


    void Update()
    {
        //Spawn_Zom();
        //Spawn_Mon();
        //Spawn_Skel();
        Spawn_Mob();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPaused();
        }
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            OnInventory(IsOpen);
        }
    }

    public void KillScore(int score)
    {
        Killcount += score;
        Killtxt.text = $"KILL : <color=#FF0000>{Killcount.ToString()}</color>";
    }

    private void Spawn_Mob()
    {
        timePrev += Time.deltaTime;
        int mobcounter = GameObject.FindGameObjectsWithTag(mob).Length;
        if (timePrev > SpawnTime)
        {
            if (mobcounter < MaxCount_MOB)
            {
                int pos = Random.Range(1, point.Length);
                int i = Random.Range(0, Mob.Length);
                Instantiate(Mob[i].gameObject, point[pos].position, point[pos].rotation);
                timePrev = 0f;
            }
        }
    }

    private void Spawn_Skel()
    {
        if (Time.time - timePrev_S > SpawnTime_S)
        {
            int Skelcounter = GameObject.FindGameObjectsWithTag(Skel).Length;
            if (Skelcounter < MaxCount_S)
            {
                int randPos_S = Random.Range(1, point.Length);
                Instantiate(Skel_fb, point[randPos_S].position, point[randPos_S].rotation);
                timePrev_S = Time.time;
            }
        }
    }

    private void Spawn_Mon()
    {
        if (Time.time - timePrev_M > SpawnTime_M)
        {
            int Moncounter = GameObject.FindGameObjectsWithTag(Mon).Length;
            if (Moncounter < MaxCount_M)
            {
                int randPos_M = Random.Range(1, point.Length);
                Instantiate(Monster_fb, point[randPos_M].position, point[randPos_M].rotation);
                timePrev_M = Time.time;
            }
        }
    }

    private void Spawn_Zom()
    {
        if (Time.time - timePrev_Z >= SpawnTime_Z)
        {
            //하이라키에서 몬스터 태그를 가진 오브젝트의 갯수를 카운트해서 넘김
            int Zomcounter = GameObject.FindGameObjectsWithTag(Zom).Length;
            if (Zomcounter < MaxCount_Z)
            {
                int randPos_Z = Random.Range(1, point.Length);
                Instantiate(Zom_fb, point[randPos_Z].position, point[randPos_Z].rotation);
                timePrev_Z = Time.time; //과거시간 업데이트
            }
        }
    }
    private bool isPaused = false;
    public void OnPaused()
    {
        isPaused = !isPaused;
        Time.timeScale = (isPaused) ? 0f : 1f;
        var player = GameObject.FindGameObjectWithTag("Player");
        var scripts = player.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = !isPaused;
        }
    }
    public void OnInventory(bool isOpen)
    {
        IsOpen = !IsOpen;
        isOpen = IsOpen;

        var playerObj = GameObject.FindGameObjectWithTag(player);
        var scripts = playerObj.GetComponents<MonoBehaviour>();

        foreach(var script in scripts)
            { script.enabled = !isOpen; }

        Cursor.visible = isOpen;
        Cursor.lockState = CursorLockMode.None;

        
        Time.timeScale = (isOpen) ? 0f : 1f;
        Inventory.blocksRaycasts = isOpen;
        Inventory.alpha = isOpen ? 1f : 0f;
    }
}
