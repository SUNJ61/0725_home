using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Utility;

public class SkeletonCtrl : MonoBehaviour
{
    [Header("컴포넌트")]
    public Transform Skel;
    public Transform Player;
    public NavMeshAgent nav;
    public Animator animator;
    public SkeletonDamage Skel_D;

    [Header("사용 변수")]
    public string player = "Player";
    public string At = "isAttack";
    public string Tr = "isTrace";
    public string playerDie = "PlayerDie";
    public float AttackDis = 3.0f;
    public float TraceDis = 20.0f;

    void Awake()
    {
        Skel = gameObject.GetComponent<Transform>();
        Player = GameObject.FindWithTag(player).GetComponent<Transform>();
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        Skel_D = GetComponent<SkeletonDamage>();
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
    }

    private void SkelCtrl()
    {
        float Dist = Vector3.Distance(Player.position, Skel.position);

        if (Dist <= AttackDis)
        {
            nav.isStopped = true;
            animator.SetBool(At, true);

            Vector3 playerpos = (Player.position - Skel.position).normalized;
            //Quaternion rot = Quaternion.FromToRotation(Skel.position, playerpos); // 파티클을 조정할때 주로 사용.
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
