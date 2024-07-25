using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager P_instance;

    public GameObject Bullet;
    public List<GameObject> BulletList;
    private int bulletMax = 10;
    private string BulletStr = "_Bullet";

    private GameObject[] EnemyPrefabs;
    public List<GameObject> EnemyList;
    private int MaxEnemy = 10;
    private string EnemyFile = "Enemy";
    void Awake()
    {
        if(P_instance == null)
            P_instance = this;
        else if(P_instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        Bullet =  Resources.Load(BulletStr) as GameObject;
        EnemyPrefabs = Resources.LoadAll<GameObject>(EnemyFile);
        SpawnBullet();
        SpawnEnemy();
    }
    void SpawnBullet()
    {
        GameObject PlayerBullet = new GameObject("PlayerBullet");
        for(int i = 0; i < bulletMax; i++)
        {
            var _bullet = Instantiate(Bullet, PlayerBullet.transform);
            _bullet.name = $"{(i + 1).ToString()}¹ß";
            _bullet.SetActive(false);
            BulletList.Add(_bullet);
        }
    }

    public GameObject GetBullet()
    {
        for(int i = 0; i < BulletList.Count; i++)
        {
            if(BulletList[i].activeSelf == false)
                return BulletList[i];
        }
        return null;
    }

    void SpawnEnemy()
    {
        GameObject enemy = new GameObject("Enemy");
        for(int i = 0; i < MaxEnemy; i++)
        {
            var _enemy = Instantiate(EnemyPrefabs[Random.Range(0,EnemyPrefabs.Length)], enemy.transform);
            _enemy.name = $"{(i + 1).ToString()}¸¶¸®";
            _enemy.SetActive(false);
            EnemyList.Add(_enemy);
        }
    }

    public GameObject GetEnemy()
    {
        for(int i = 0; i <  EnemyList.Count; i++)
        {
            if (EnemyList[i].activeSelf == false)
                return EnemyList[i];
        }
        return null;
    }
}
