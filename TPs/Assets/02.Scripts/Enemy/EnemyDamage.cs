using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private GameObject bloodEffect;

    private readonly string bulletTag = "BULLET";
    private readonly string S_bulletTag = "S_BULLET";
    private readonly string BLDeffTag = "Effects/BulletImpactFleshBigEffect";

    private float Hp = 100.0f;
    void Start()
    {
        bloodEffect = Resources.Load<GameObject>(BLDeffTag);
        Hp = Mathf.Clamp(Hp, 0f, 100f);
    }
    #region ������Ÿ�� ����� �浹����
    //private void OnCollisionEnter(Collision col)
    //{
    //    if (col.collider.CompareTag(bulletTag) || col.collider.CompareTag(S_bulletTag))
    //    {
    //        col.gameObject.SetActive(false);
    //        ShowBLD_Effect(col);
    //        Hp -= col.gameObject.GetComponent<Bullet>().Damage;
    //        if (Hp <= 0)
    //            Die();
    //    }
    //}//����ĳ��Ʈ������ ������� ���Ѵ�.
    #endregion
    void OnDamage(object[] _params)
    {
        ShowBLD_Effect((Vector3)_params[0]);
        Hp -= (float)_params[1];
        if (Hp <= 0)
            Die();
    }
    #region ������Ÿ�� �� �ڵ�
    //private void ShowBLD_Effect(Collision col)
    //{ 
    //    Vector3 pos = col.contacts[0].point;
    //    //collision����ü �ȿ� contacts��� �迭�� �ִµ� ���⿣ ������ ��ġ�� ����Ǿ� �ִ�.
    //    Vector3 _Normal = col.contacts[0].normal; // ������Ʈ�� ������ ������ ��´�.
    //    Quaternion rot = Quaternion.FromToRotation(-transform.forward, _Normal);
    //    //�� ���⿡�� ������Ʈ�� ���󰡴� �������� ����Ʈ�� ���
    //    GameObject blood = Instantiate(bloodEffect, pos, rot);
    //    Destroy(blood, 1.0f);
    //}
    #endregion
    private void ShowBLD_Effect(Vector3 col)
    {
        Vector3 pos = col;
        Vector3 _Normal = col.normalized; // ������Ʈ�� ������ ������ ��´�.
        Quaternion rot = Quaternion.FromToRotation(-transform.forward, _Normal);
        //�� ���⿡�� ������Ʈ�� ���󰡴� �������� ����Ʈ�� ���
        GameObject blood = Instantiate(bloodEffect, pos, rot);
        Destroy(blood, 1.0f);
    }
    void Die()
    {
        GetComponent<EnemyAI>().state = EnemyAI.State.DIE; //���ʹ� ���¸� �����ϴ� ��ũ��Ʈ�� �ҷ��� ���� ����
        GameManager.G_instance.KillScore();
        Hp = 100.0f;
    }
    void ExpDie()
    {
        GetComponent<EnemyAI>().state = EnemyAI.State.EXPDIE;
        GameManager.G_instance.KillScore();
        Hp = 100.0f;
    }
}
