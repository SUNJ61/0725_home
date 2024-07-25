using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
    void Start()
    {

    }
    public void OnDrop(PointerEventData eventData) //오브젝트를 드랍했다면
    {
        if(transform.childCount == 0) //슬롯 밑에 자식 오브젝트가 없으면
            Drag.draggingItem.transform.SetParent(transform, false); //드래그된 아이템 오브젝트의 부모를 슬롯으로 바꾼다.
                                                                     //이때 아이템 오브젝트의 절대 좌표는 끈다.
    }
}
