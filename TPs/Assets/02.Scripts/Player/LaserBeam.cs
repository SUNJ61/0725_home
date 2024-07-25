using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LaserBeam : MonoBehaviour
{
    private Transform Tr;
    private LineRenderer line;
    private Transform firepos;
    private Player player;
    private FireCtrl fireCtrl;
    void Start()
    {
        Tr = transform;
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.enabled = false;
        firepos = transform.GetComponentsInParent<Transform>()[1]; //�ڱ��ڽ��� �����Ͽ� �θ� ���۳�Ʈ�� ��´�.
        //��, �ε����� 0�̸� �ڱ��ڽ�, �ε����� 1�̸� ���� ����� �θ� ������Ʈ�� �ȴ�.
        player = transform.GetComponentInParent<Player>();//���� ���� �ִ� �θ������Ʈ�� �������� ���
        fireCtrl = transform.GetComponentInParent<FireCtrl>();
    }
    void Update()
    {
        if (Time.timeScale <= 0f) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        Ray ray = new Ray(firepos.position, Tr.forward);//Ʈ���� �������� ���� ������ ����
        //���� ��ġ�� ��¦ �ø� ������, ���� ��ǥ�� ���� �ٴ��� �������� �߱� ������ ��¦ �ø�.
        RaycastHit hit;
        Debug.DrawLine(ray.origin,ray.direction * 100f, Color.blue); //������ ���� ���̱�
        if(Input.GetMouseButtonDown(0) && !player.isRun && !fireCtrl.isReloading)
        {
            line.SetPosition(0,Tr.InverseTransformPoint(ray.origin));//���� ������ ù��° ���� ��ġ ����.
            //InverseTransformDirection�� ���� ��ǥ(������ǥ)�� ������ǥ�� �ٲٴ� �Լ� (�������� ���۵Ǵ� ������ ����)
            if(Physics.Raycast(ray,out hit,100f)) //� ��ü�� �¾��� �� ��ġ�� LineRenderer�� �������� ����.
            {
                line.SetPosition(1, Tr.InverseTransformPoint(hit.point));
            }
            else //������ ���𰡿� ���� ������, �ִ� ���� �Ÿ��� ������ �߻� ��ġ�� ���� 100���� �Ѵ�.
            {
                line.SetPosition(1, Tr.InverseTransformPoint(ray.GetPoint(100f)));
            }
            StartCoroutine(ShowLaserBeam());
        }
    }
    IEnumerator ShowLaserBeam()
    {
        line.enabled=true;
        yield return new WaitForSeconds(Random.Range(0.1f,0.2f));
        line.enabled=false;
    }
}
