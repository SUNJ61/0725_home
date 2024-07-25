using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [Header("������Ʈ")] //Attribute : �����ڰ� ���� ��ǻ��(����Ƽ)�� �о� �ش� ��������� �����ش�. 
    public NavMeshAgent agent; //������ ����� ã�� �׺�
    public Transform Player; //������ ���� �������� �Ÿ��� ������� ���
    public Transform thisZombie; 
    public Animator animator;
    public ZombieDamage zombieDamage;

    [Header("���� ����")]
    public float attackDist = 3.0f; //���� ����
    public float traceDist = 20f; //���� ����

    public string playerDie = "PlayerDie";
    void Awake()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>(); //�ڱ��ڽ� ������Ʈ�� �ȿ� �ִ� nav���۳�Ʈ�� �����Ų��.
        thisZombie = this.gameObject.GetComponent<Transform>(); //�ڱ��ڽ� ������Ʈ�� �ִ� transform���۳�Ʈ�� �����Ų��.
        Player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        //���̶�Ű�ȿ� �ִ� ���ӿ�����Ʈ�� �±׸� �о �����´�. GetComponent<Transform>()�� transform���� �ٿ� �� �� �ִ�.
        animator = GetComponent<Animator>();//�ڱ��ڽ� ������Ʈ���� ������Ʈ�� �����ö� this.gameObject ��������
        zombieDamage = GetComponent<ZombieDamage>();
    }
    private void OnEnable()
    {
        StartCoroutine(MonsterAI());
    }
    IEnumerator MonsterAI()
    {
        while(!(zombieDamage.isDie || Player.GetComponent<FPS_Damage>().isPlayerDie))
        {
            ZomCtrl();
            yield return new WaitForSeconds(0.3f);
        }
    }
    //void Update()
    //{
    //    if (zombieDamage.isDie || Player.GetComponent<FPS_Damage>().isPlayerDie) return; //������������ ���������ʰ� �Լ��� ����ȴ� ��, ������Ʈ�� �����
    //                                                                                     //�Ÿ��� ���
    //    ZomCtrl();
    //}

    private void ZomCtrl()
    {
        float distance = Vector3.Distance(thisZombie.position, Player.position); //�� ������Ʈ�� 3���� ��ǥ �Ÿ��� �����Ѵ�.
        if (distance <= attackDist)
        {
            agent.isStopped = true; // ���� ����
            animator.SetBool("isAttack", true);
            //Debug.Log("����");
            Vector3 playerpos = Player.position - thisZombie.position; // �÷��̾ �ٶ󺸴� ����, �Ÿ��� ��´�.
            playerpos = playerpos.normalized; //���⸸ ����
            Quaternion rot = Quaternion.LookRotation(playerpos);
            thisZombie.rotation = Quaternion.Slerp(thisZombie.rotation, rot, Time.deltaTime * 3.0f);
        }
        else if (distance <= traceDist)
        {
            animator.SetBool("isAttack", false);
            animator.SetBool("isTrace", true);
            agent.isStopped = false; //���� ����
            agent.destination = Player.position; //���� ����� �÷��̾� ��ǥ
            //Debug.Log("����");
        }
        else
        {
            agent.isStopped = true;
            animator.SetBool("isAttack", false);
            animator.SetBool("isTrace", false);
            //Debug.Log("���� ���� ���");
        }
    }

    public void PlayerDeath()
    {
        GetComponent<Animator>().SetTrigger(playerDie);
    }
}
