using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Utility;

public class SkeletonCtrl : MonoBehaviour
{
    [Header("������Ʈ")]
    public Rigidbody rb;
    public CapsuleCollider capCol;
    public Transform Skel;
    public Transform Player;
    public NavMeshAgent nav;
    public Animator animator;
    public SkeletonDamage Skel_D;

    [Header("��� ����")]
    public string player = "Player";
    public string At = "isAttack";
    public string Tr = "isTrace";
    public string playerDie = "PlayerDie";
    public string die_T = "dieTrigger";
    public float AttackDis = 3.0f;
    public float TraceDis = 20.0f;

    void Awake()
    {
        Skel = gameObject.GetComponent<Transform>();
        Player = GameObject.FindWithTag(player).GetComponent<Transform>();
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        Skel_D = GetComponent<SkeletonDamage>();
        rb = GetComponent<Rigidbody>();
        capCol = GetComponent<CapsuleCollider>();
    }
    private void OnEnable()
    {
        StartCoroutine(SkelAI());
    }
    IEnumerator SkelAI()
    {
        while (!(Skel_D.isDie || Player.GetComponent<FPS_Damage>().isPlayerDie))
        {
            SkelCtrl();
            yield return new WaitForSeconds(0.3f);
        }
        if(Skel_D.isDie)
        {
            animator.SetTrigger(die_T);
            nav.isStopped = true;
            rb.isKinematic = true;
            capCol.enabled = false;
            GameManager.Instance.KillScore(1);
            yield return new WaitForSeconds(3.0f);
            gameObject.SetActive(false);
        }
    }

    private void SkelCtrl()
    {
        float Dist = Vector3.Distance(Player.position, Skel.position);

        if (Dist <= AttackDis)
        {
            nav.isStopped = true;
            animator.SetBool(At, true);

            Vector3 playerpos = (Player.position - Skel.position).normalized;
            //Quaternion rot = Quaternion.FromToRotation(Skel.position, playerpos); // ��ƼŬ�� �����Ҷ� �ַ� ���.
            Quaternion rot = Quaternion.LookRotation(playerpos);
            Skel.rotation = Quaternion.Slerp(Skel.rotation, rot, Time.deltaTime * 3.0f);

        }
        else if (Dist <= TraceDis)
        {
            nav.isStopped = false;
            animator.SetBool(At, false);
            animator.SetBool(Tr, true);
            nav.destination = Player.position;
        }
        else if (Dist > TraceDis)
        {
            nav.isStopped = true;
            animator.SetBool(At, false);
            animator.SetBool(Tr, false);
        }
    }

    public void PlayerDeath()
    {
        GetComponent<Animator>().SetTrigger(playerDie);
    }
}
