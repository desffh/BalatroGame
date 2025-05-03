using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class ButtonManager : Singleton<ButtonManager>
{
    [SerializeField] Button Handbutton; // �ڵ� �÷���
    [SerializeField] Button Treshbutton;//     ������

    [SerializeField] HandCardPoints HandCardPoints;



    

    // ��ư Ȱ��ȭ ���� ����
    private bool isButtonActive = true;

    private void Start()
    {
        Handbutton.interactable = false;
        Treshbutton.interactable = false;
    }

    private void Update()
    {
       if(isButtonActive == true && HandDelete.Instance.Hand > 0 && PokerManager.Instance.cardData.SelectCards.Count > 0) 
       {
            Handbutton.interactable = true;
       }
       else
       {
            Handbutton.interactable = false;
       }
       if(isButtonActive == true && HandDelete.Instance.Delete > 0 && PokerManager.Instance.cardData.SelectCards.Count > 0)
        {
            Treshbutton.interactable = true;
        }
       else
        {
            Treshbutton.interactable = false;

        }
    }

    // �ڵ��ư�� Ŭ������ ��
    public void OnHandButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        StartCoroutine(CardDeletePoint());
        StartCoroutine(CardDeleteSound());  
    }


    IEnumerator CardDeleteSound()
    {
        for (int i = 0; i < PokerManager.Instance.cardData.SelectCards.Count; i++)
        {
            SoundManager.Instance.PlayCardSpawn();
            yield return new WaitForSeconds(0.12f);
        }

    }

    IEnumerator CardDeletePoint()
    {
        // ī�� Ŭ�� ��Ȱ��ȭ
        CardManager.Instance.TurnOffAllCardColliders();

        for (int i = 0; i < PokerManager.Instance.cardData.SelectCards.Count; i++)
        {
            // ����� ī���� ��ũ��Ʈ ��������
            Card selectedCard = PokerManager.Instance.cardData.SelectCards[i].gameObject.GetComponent<Card>();

            // ����� �������� ��ġ���� �����ϱ� ���� ������Ʈ ��������
            PokerManager.Instance.cardData.SelectCards[i].gameObject.GetComponent<Transform>();

            // ��ġ�� HandCardPoints�� �̵�
            PokerManager.Instance.cardData.SelectCards[i].gameObject.transform.
                DOMove(HandCardPoints.HandCardpos[i].transform.position, 0.5f);
            // ȸ�� 0
            PokerManager.Instance.cardData.SelectCards[i].gameObject.transform.rotation = Quaternion.identity;

            // myCards ����Ʈ���� �ش� ī�� ���� (���ۿ��� �����ͼ� �����ϴ� ��)
            if (selectedCard != null && CardManager.Instance.myCards.Contains(selectedCard))
            {
                CardManager.Instance.myCards.Remove(selectedCard);
            }

            yield return new WaitForSeconds(0.15f);
        }
        // ���� ī��� ������ �Ǳ�
        CardManager.Instance.SetOriginOrder();
        CardManager.Instance.CardAlignment();

        // ���ϱ� ���
        HoldManager.Instance.Calculation();
    }

    // ������ ��ư�� Ŭ������ ��
    public void OnDeleteButtonClick()
    {
        SoundManager.Instance.ButtonClick();

        isButtonActive = false;

        // ī�� Ŭ�� ��Ȱ��ȭ
        CardManager.Instance.TurnOffAllCardColliders();

        int cardCount = PokerManager.Instance.cardData.SelectCards.Count;
        int completeCount = 0;

        for (int i = 0; i < cardCount; i++)
        {
            Card selectedCard = PokerManager.Instance.cardData.SelectCards[i].GetComponent<Card>();

            PokerManager.Instance.cardData.SelectCards[i].transform
                .DOMove(HandCardPoints.DeleteCardpos.position, 0.5f)
                .OnComplete(() => {
                    completeCount++;

                    if (completeCount >= cardCount)
                    {
                        // ��� ī�� �̵��� ���� ������ ����
                        AfterDeleteAnimationComplete();
                    }
                });

            PokerManager.Instance.cardData.SelectCards[i].transform
                .DORotate(new Vector3(58, 122, 71), 3);

            CardManager.Instance.OnCardUsed(selectedCard);

            if (selectedCard != null && CardManager.Instance.myCards.Contains(selectedCard))
            {
                CardManager.Instance.myCards.Remove(selectedCard);
            }

            StartCoroutine(CardDeleteSound());
        }

        HoldManager.Instance.StartDeleteCard();
    }

    private void AfterDeleteAnimationComplete()
    {
        // ���� ī��� ����
        CardManager.Instance.SetOriginOrder();

        // ���� �� �ִϸ��̼� �Ϸ���� �� �ݶ��̴� �ٽ� �ѱ�
        CardManager.Instance.CardAlignment(() => {
            CardManager.Instance.TurnOnAllCardColliders();
        });
    }


    // �̺�Ʈ�� �� �Լ� -> ����� ���۵Ǹ� ��ư ��ȣ�ۿ� ��Ȱ��ȭ
    public void ButtonActive()
    {
        Handbutton.interactable = false;
        Treshbutton.interactable = false;
        
        isButtonActive = false;
    }

    public void ButtonInactive()
    {
        isButtonActive = true;
    }


    // |-----------------------------------------

    

}
