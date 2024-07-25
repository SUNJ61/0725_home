using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{
    [Header("컴포넌트")]
    public NavMeshAgent agent;
    public Transform Player;
    public Transform thisMonster;
    public Animator animator;
    public MonsterDamage monsterDamage;

    [Header("관련 변수")]
    public float traceDis = 20.0f;
    public float attackDis = 3.0f;

    public string playerDie = "PlayerDie";
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.FindWithTag("Player").transform;
        thisMonster = transform;
        animator = GetComponent<Animator>();
        monsterDamage = GetComponent<MonsterDamage>();
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
            yield return new WaitForSeconds(0.3f);
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
