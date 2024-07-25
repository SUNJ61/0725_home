using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class swatDamage : MonoBehaviour
{
    private GameObject bloodEffect;

    private readonly string bulletTag = "BULLET";
    private readonly string E_bulletTag = "E_BULLET";
    private readonly string BLDeffTag = "Effects/BulletImpactFleshBigEffect";

    private float Hp = 100.0f;
    void Start()
    {
        bloodEffect = Resources.Load<GameObject>(BLDeffTag);
        Hp = Mathf.Clamp(Hp, 0f, 100f);
    }
    #region 프로젝타일 감지방법
    //private void OnCollisionEnter(Collision col)
    //{
    //    if(col.collider.CompareTag(bulletTag)|| col.collider.CompareTag(E_bulletTag))
    //    {
    //        col.gameObject.SetActive(false);
    //        ShowBLD_Effect(col);
    //        Hp -= col.gameObject.GetComponent<Bullet>().Damage;
    //        if(Hp <= 0)
    //            Die();
    //    }
    //}
    #endregion
    void OnDamage(object[] _params)
    {
        ShowBLD_Effect(_params);
        Hp -= (float)_params[1];
        if(Hp <= 0)
            Die();

    }
    private void ShowBLD_Effect(Collision col)
    {
        Vector3 pos = col.contacts[0].point;
        Vector3 _Normal = col.contacts[0].normal;
        Quaternion rot = Quaternion.FromToRotation(-transform.forward, _Normal);
        GameObject blood = Instantiate(bloodEffect, pos, rot);
        Destroy(blood, 1.0f);
    }
    private void ShowBLD_Effect(object[] _params)
    {
        Vector3 pos = (Vector3)_params[0];
        Vector3 _Normal = pos.normalized;
        Quaternion rot = Quaternion.FromToRotation(-transform.forward, _Normal);
        GameObject blood = Instantiate(bloodEffect, pos, rot);
        Destroy(blood, 1.0f);
    }
    void Die()
    {
        GetComponent<swatAI>().state = swatAI.S_State.DIE;
        GameManager.G_instance.KillScore();
        Hp = 100.0f;
    }
    void ExpDie()
    {
        GetComponent<swatAI>().state = swatAI.S_State.EXPDIE;
        GameManager.G_instance.KillScore();
        Hp = 100.0f;
    }
}
