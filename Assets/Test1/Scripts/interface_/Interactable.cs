using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Interactable : MonoBehaviour
{
    [SerializeField] Button rankButton;

    [SerializeField] Button suitButton;


    private void Update()
    {
        // �̰͵� ���߿� ī�尡 Ŭ�� �Ǿ������� �Ǻ��ϵ��� �̺�Ʈ ����ϸ� Update()���� �����ص���
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
        suitButton.interactable = false;
    }

    public void OnButton()
    {
        //Debug.Log("��ũ ��ư Ȱ��ȭ");
        rankButton.interactable = true;
        suitButton.interactable = true;
    }
}
