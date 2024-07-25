using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    private GameObject BLDeff;
    private Image BloodScreen;
    private Image Hpbar;

    private readonly string e_bullettag = "E_BULLET";
    private readonly string s_bullettag = "S_BULLET";
    private readonly string enemytag = "ENEMY";
    private readonly string swattag = "SWAT";
    private readonly string BLDeffStr = "Effects/BulletImpactFleshBigEffect";
    private readonly string UIObj = "Canvas_UI";

    private float Hp = 0f;
    private float maxHp = 100f;
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;//static�� �޾Ƽ� class�ȿ����� ����ϴ� ���� �ƴ� �ܺο����� ����� �� �ֵ��� �Ѵ�.

    void Start()
    {
        BLDeff = Resources.Load<GameObject>(BLDeffStr);
        Hp = Mathf.Clamp(Hp, 0f, maxHp);
        Hp = maxHp;

        BloodScreen =GameObject.Find(UIObj).transform.GetChild(0).GetComponent<Image>();
        Hpbar = GameObject.Find(UIObj).transform.GetChild(2).GetChild(2).GetComponent<Image>();
        Hpbar.color = Color.green;
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag(e_bullettag) || col.gameObject.CompareTag(s_bullettag))
        {
            col.gameObject.SetActive(false);
            Hp -= 5;
            ShowBLD_Eff(col);
            HpBarCtrl();
            if (Hp <= 0f)
            {
                PlayerDie();
            }
            StartCoroutine(ShowBLD_Screen());
        }
    }

    private void HpBarCtrl()
    {
        Hpbar.fillAmount = (float)Hp / (float)maxHp;
        if (Hp <= 30f)
            Hpbar.color = Color.red;
        else if (Hp <= 50f)
            Hpbar.color = Color.yellow;
        else
            Hpbar.color = Color.green;
    }

    IEnumerator ShowBLD_Screen()
    {
        BloodScreen.color = new Color(1, 0, 0, Random.Range(0.25f,0.35f));
        yield return new WaitForSeconds(1f);
        BloodScreen.color = Color.clear; //����, ���İ��� ���� 0���� �����.
    }
    public void PlayerDie()
    {
        #region �÷��̾ �׾��� �� ��� ���� �迭�� ��� for������ ��Ȱ��ȭ
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemytag);// ���� �������� for������ �ϱ⿡�� ȿ���� ��������.
        //GameObject[] swats = GameObject.FindGameObjectsWithTag(swattag);
        //for(int i = 0; i < enemies.Length; i++)
        //{
        //    enemies[i].gameObject.SendMessage("OnPlayerDie",SendMessageOptions.DontRequireReceiver);
        //}
        //for(int i = 0; i < swats.Length;i++)
        //{
        //    swats[i].gameObject.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        //}
        #endregion
        OnPlayerDie(); // delegate void PlayerDie() �븮�ڸ� ���Ĺ��� ����.
    }
    private void ShowBLD_Eff(Collision col)
    {
        Vector3 pos = col.contacts[0].point;
        Vector3 _Normal = col.contacts[0].normal;
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _Normal);
        GameObject BLD = Instantiate(BLDeff, pos, rot);
        Destroy(BLD, 1.0f);
    }
}
