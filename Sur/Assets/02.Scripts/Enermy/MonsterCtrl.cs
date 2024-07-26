using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{
    [Header("������Ʈ")]
    public NavMeshAgent agent;
    public Transform Player;
    public Transform thisMonster;
    public Animator animator;
    public MonsterDamage monsterDamage;
    public CapsuleCollider capsule;
    public Rigidbody rb;

    [Header("���� ����")]
    public float traceDis = 20.0f;
    public float attackDis = 3.0f;

    public string playerDie = "PlayerDie";
    public string dieTrigger = "DieTrigger";
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.FindWithTag("Player").transform;
        thisMonster = transform;
        animator = GetComponent<Animator>();
        monsterDamage = GetComponent<MonsterDamage>();
        capsule = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        StartCoroutine(MonsterAI());
    }
    IEnumerator MonsterAI()
    {
        while (!(monsterDamage.isDie || Player.GetComponent<FPS_Damage>().isPlayerDie))
        {
            MonCtrl();
            yield return new WaitForSeconds(0.2f);
        }
        if(monsterDamage.isDie)
        {
            animator.SetTrigger(dieTrigger);
            agent.isStopped = true;
            capsule.enabled = false;
            rb.isKinematic = true;
            GameManager.Instance.KillScore(1);
            yield return new WaitForSeconds(3.0f);
            gameObject.SetActive(false);
        }
    }

    private void MonCtrl()
    {
        float distance = Vector3.Distance(Player.position, thisMonster.position);

        if (distance <= attackDis)
        {
            agent.isStopped = true;
            animator.SetBool("isAttack", true);
            Quaternion rot = Quaternion.LookRotation(Player.position - thisMonster.position);
            thisMonster.rotation = Quaternion.Slerp(thisMonster.rotation, rot, Time.deltaTime * 3.0f);
        }
        else if (distance <= traceDis)
        {
            animator.SetBool("isTrace", true);
            animator.SetBool("isAttack", false);
            agent.isStopped = false;
            agent.destination = Player.position;
        }
        else
        {
            agent.isStopped = true;
            animator.SetBool("isTrace", false);
            animator.SetBool("isAttack", false);

        }
    }

    public void PlayerDeath()
    {
        GetComponent<Animator>().SetTrigger(playerDie);
    }
}
