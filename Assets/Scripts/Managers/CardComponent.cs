using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardComponent : MonoBehaviour
{
    // 콜라이더 활성화
    public abstract void OnCollider();

    // 콜라이더 비활성화
    public abstract void OffCollider();

    // 마우스로 클릭했을 때 호출하는 함수 
    public abstract void OnCardClicked();
}
