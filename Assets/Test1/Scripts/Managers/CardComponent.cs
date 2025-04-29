using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardComponent : MonoBehaviour
{
    // �ݶ��̴� Ȱ��ȭ
    public abstract void OnCollider();

    // �ݶ��̴� ��Ȱ��ȭ
    public abstract void OffCollider();

    // ���콺�� Ŭ������ �� ȣ���ϴ� �Լ� 
    public abstract void OnCardClicked();
}
