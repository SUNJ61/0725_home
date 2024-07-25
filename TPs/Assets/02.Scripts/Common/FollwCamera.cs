using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollwCamera : MonoBehaviour
{
    [SerializeField]private Transform Target; //따라갈 타겟 위치정보가 필요하다, 카메라가 붙을 타겟이 고정이 아니기에 직접 넣음.
    private Transform CamTr; //카메라 위치

    private float height = 5.0f; //카메라가 위치할 높이
    private float distance = 7.0f; //타겟과 카메라의 거리
    private float movedamping = 10.0f; //카메라의 이동을 부드럽게 완화하는 값.
    private float rotdamping = 15.0f; //카메라의 회전을 부드럽게 완화하는 값.
    private float targetOffset = 2.0f; //타겟에서의 카메라 높이값
    void Start()
    {
        CamTr = GetComponent<Transform>();
    }
    void LateUpdate() // update함수가 먼저 된다음 lateupdate로 따라간다.
    {//CamPos계산에서 Vector3.forward와 up은 절대 좌표라 캐릭터가 움직이면 카메라가 같이 회전을 안함. 
        var CamPos = Target.position - (Target.forward * distance) + (Target.up * height); //캠은 타겟의 후방 위쪽에 위치.
        CamTr.position = Vector3.Slerp(CamTr.position, CamPos, movedamping * Time.deltaTime);
        //곡면 보간(원래 캠위치에서 Campos까지 movedamping * 델타타임의 속도로 부드럽게 이동.) 카메라가 바라보는 위치 조절
        CamTr.rotation = Quaternion.Slerp(CamTr.rotation, Target.rotation, rotdamping * Time.deltaTime);
        //곡면 보간(원래 본인 로테이션에서 타겟의 로테이션까지 rotdamping * 델타타임의 속도로 부드럽게 이동.) 바라보는 각도 조절
        CamTr.LookAt(Target.position + (Target.up * targetOffset));
        //기존 타겟의 targetOffset의 높이만큼 위쪽을 카메라가 바라본다. 카메라가 보는 타켓 위치 조절


    }

    private void OnDrawGizmos() //씬 화면에서 색상이나 선을 그려주는 함수
    {
        Gizmos.color = new Color(0, 255, 0, 255);
        Gizmos.DrawSphere(Target.position + (Target.up * targetOffset), 0.1f); //카메라가 바라보는 위치를 알려줌
        //Gizmos.DrawLine(Target.position + (Target.up * targetOffset), CamTr.position); //카메라가 바라보는 위치와 카메라를 잇는 선
        //해당 코드에서 오류 발생, LateUpdate에서 카메라위치가 늦게 변화하여 발생 하는 오류
    }
}
