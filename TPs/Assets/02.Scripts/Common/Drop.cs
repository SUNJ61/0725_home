using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
    void Start()
    {

    }
    public void OnDrop(PointerEventData eventData) //������Ʈ�� ����ߴٸ�
    {
        if(transform.childCount == 0) //���� �ؿ� �ڽ� ������Ʈ�� ������
            Drag.draggingItem.transform.SetParent(transform, false); //�巡�׵� ������ ������Ʈ�� �θ� �������� �ٲ۴�.
                                                                     //�̶� ������ ������Ʈ�� ���� ��ǥ�� ����.
    }
}
