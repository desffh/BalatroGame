using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Interactable : MonoBehaviour
{
    [SerializeField] Button rankButton;


    private void Awake()
    {
        rankButton = GetComponent<Button>();
    }

    private void Update()
    {
        // �̰͵� ���߿� ī�尡 Ŭ�� �Ǿ������� �Ǻ��ϵ��� �ϸ� Update()���� �����ص���
        if (PokerManager.Instance.cardData.SelectCards.Count > 0)
        {
            OffButton();
        }
        else
        {
            OnButton();
        }
    }

    public void OffButton()
    {
        rankButton.interactable = false;
    }

    public void OnButton()
    {
        //Debug.Log("��ũ ��ư Ȱ��ȭ");
        rankButton.interactable = true;
    }
}
