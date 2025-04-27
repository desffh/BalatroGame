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

    [SerializeField] GameObject PopUpCanvas;
    [SerializeField] GameObject EndPanel;

    

    // ��ư Ȱ��ȭ ���� ����
    private bool isButtonActive = true;

    private void Start()
    {
        Handbutton.interactable = false;
        Treshbutton.interactable = false;

        PopUpCanvas.SetActive(false);
    }

    private void Update()
    {
       if(isButtonActive == true && HandDelete.Instance.Hand > 0 && PokerManager.Instance.CardIDdata.Count > 0) 
       {
            Handbutton.interactable = true;
       }
       else
       {
            Handbutton.interactable = false;
       }
       if(isButtonActive == true && HandDelete.Instance.Delete > 0 && PokerManager.Instance.CardIDdata.Count > 0)
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
        for (int i = 0; i < PokerManager.Instance.CardIDdata.Count; i++)
        {
            SoundManager.Instance.PlayCardSpawn();
            yield return new WaitForSeconds(0.12f);
        }

    }

    IEnumerator CardDeletePoint()
    {

        // ī�� Ŭ�� ��Ȱ��ȭ
        KardManager.Instance.TurnOffAllCardColliders();

        for (int i = 0; i < PokerManager.Instance.CardIDdata.Count; i++)
        {
            // ����� ī���� ��ũ��Ʈ ��������
            Card selectedCard = PokerManager.Instance.CardIDdata[i].gameObject.GetComponent<Card>();

            // ����� �������� ��ġ���� �����ϱ� ���� ������Ʈ ��������
            PokerManager.Instance.CardIDdata[i].gameObject.GetComponent<Transform>();

            // ��ġ�� HandCardPoints�� �̵�
            PokerManager.Instance.CardIDdata[i].gameObject.transform.
                DOMove(HandCardPoints.HandCardpos[i].transform.position, 0.5f);
            // ȸ�� 0
            PokerManager.Instance.CardIDdata[i].gameObject.transform.rotation = Quaternion.identity;

            // myCards ����Ʈ���� �ش� ī�� ���� (���ۿ��� �����ͼ� �����ϴ� ��)
            if (selectedCard != null && KardManager.Instance.myCards.Contains(selectedCard))
            {
                KardManager.Instance.myCards.Remove(selectedCard);
            }

            yield return new WaitForSeconds(0.15f);
        }
        // ���� ī��� ������ �Ǳ�
        KardManager.Instance.SetOriginOrder();
        KardManager.Instance.CardAlignment();

        // ���ϱ� ���
        HoldManager.Instance.Calculation();
    }

    // ������ ��ư�� Ŭ������ ��
    public void OnDeleteButtonClick()
    {
        SoundManager.Instance.ButtonClick();

        isButtonActive = false;

        // ī�� Ŭ�� ��Ȱ��ȭ
        KardManager.Instance.TurnOffAllCardColliders();

        int cardCount = PokerManager.Instance.CardIDdata.Count;
        int completeCount = 0;

        for (int i = 0; i < cardCount; i++)
        {
            Card selectedCard = PokerManager.Instance.CardIDdata[i].GetComponent<Card>();

            PokerManager.Instance.CardIDdata[i].transform
                .DOMove(HandCardPoints.DeleteCardpos.position, 0.5f)
                .OnComplete(() => {
                    completeCount++;

                    if (completeCount >= cardCount)
                    {
                        // ��� ī�� �̵��� ���� ������ ����
                        AfterDeleteAnimationComplete();
                    }
                });

            PokerManager.Instance.CardIDdata[i].transform
                .DORotate(new Vector3(58, 122, 71), 3);

            KardManager.Instance.OnCardUsed(selectedCard);

            if (selectedCard != null && KardManager.Instance.myCards.Contains(selectedCard))
            {
                KardManager.Instance.myCards.Remove(selectedCard);
            }

            StartCoroutine(CardDeleteSound());
        }

        HoldManager.Instance.StartDeleteCard();
    }

    private void AfterDeleteAnimationComplete()
    {
        // ���� ī��� ����
        KardManager.Instance.SetOriginOrder();

        // ���� �� �ִϸ��̼� �Ϸ���� �� �ݶ��̴� �ٽ� �ѱ�
        KardManager.Instance.CardAlignment(() => {
            KardManager.Instance.TurnOnAllCardColliders();
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


    public void RunOnClick()
    {
        SoundManager.Instance.ButtonClick();
        PopUpCanvas.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void RunDeleteClick()
    {
        PopUpCanvas.SetActive(false);
        Time.timeScale = 1.0f;
    }

    // |-----------------------------------------

    

}
