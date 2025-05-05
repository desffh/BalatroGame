using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
public class PokerResult
{
    public List<int> SaveNum = new();
    public int Plus;
    public int Multiple;
    public string PokerName;
}

public class PokerManager : Singleton<PokerManager>
{
    private List<IPokerHandle> pokerHands;
    
    // �����ص� ����
    [SerializeField] public List<int> saveNum;
    
    // |------------------------------------------------

    [SerializeField] HandCardPoints deleteCardPoint;




    [SerializeField] public CardData cardData;

    protected override void Awake()
    {
        base.Awake();

        saveNum = new List<int>();
        // ���� ����Ʈ �ʱ�ȭ

        pokerHands = new List<IPokerHandle>
        {
            new RoyalStraightFlush(),
            new straightFlush(),
            new FourCard(),
            new fullHouse(),
            new Flush(),
            new straight(),
            new Triple(),
            new TwoPair(),
            new OnePair(),
            new HighCard()
        };
        saveNum = new List<int>();  // saveNum ����Ʈ�� ���⼭ �� ���� ����
    }


    public int plus;
    public int multiple;
    public string pokerName;

    // ī�� ����Ʈ�� ���Ͽ� ���� �̸��� ������ ���
    public void EvaluatePokerHands(List<Card> cards)
    {
        saveNum.Clear();

        foreach (var hand in pokerHands)
        {
            List<int> tempSaveNum = new List<int>(); // �ӽ� ���� ����Ʈ

            hand.PokerHandle(cards, tempSaveNum);

            plus = hand.plus;
            multiple = hand.multiple;
            pokerName = hand.pokerName;

            if (tempSaveNum.Count > 0) // ù ��°�� ��Ī�Ǵ� ������ ����
            {
                saveNum = tempSaveNum;
                //Debug.Log($"���� ���õ�: {hand.pokerName}");
                break;
            }
        }
    }

    public void SelectCard(Card card)
    {
        if (cardData.SelectCards.Count >= 5 || cardData.SelectCards.Contains(card))
            return;

        cardData.AddSelectCard(card);
        EvaluateAndUpdateUI();
    }

    public void DeselectCard(Card card)
    {
        cardData.RemoveSelectCard(card);
        EvaluateAndUpdateUI();
    }

    public void ClearSelection()
    {
        cardData.ClearSelectCard();
        EvaluateAndUpdateUI();
    }
    private void EvaluateAndUpdateUI()
    {
        var selected = cardData.SelectCards;

        if (selected.Count > 0)
        {
            

            EvaluatePokerHands(selected.ToList());

            TextManager.Instance.UpdateText(1, plus);
            TextManager.Instance.UpdateText(2, multiple);
            TextManager.Instance.stringUpdateText(0, pokerName);
        }
        else
        {
            saveNum.Clear();

            TextManager.Instance.UpdateText(1);       // �⺻�� �ʱ�ȭ
            TextManager.Instance.UpdateText(2);
            TextManager.Instance.stringUpdateText(0);
        }
    }

    public void QuitCollider2()
    {
        for (int i = 0; i < cardData.SelectCards.Count; i++)
        {
            cardData.SelectCards[i].gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    // �� �Ǻ��ϰ� ������
    public void DeleteMove()
    {
        for (int i = 0; i < cardData.SelectCards.Count; i++)
        {
            cardData.SelectCards[i].transform.GetComponent<Transform>();

            cardData.SelectCards[i].transform.
                DOMove(deleteCardPoint.DeleteCardpos.transform.position, 1).SetDelay(i * 0.15f);

            cardData.SelectCards[i].transform.
                DORotate(new Vector3(-45, -60, -25), 0.5f).SetDelay(i * 0.15f);

            StartCoroutine(CardDeleteSound());


            CardManager.Instance.OnCardUsed(cardData.SelectCards[i]);
        }
    }

    IEnumerator CardDeleteSound()
    {
        for (int i = 0; i < cardData.SelectCards.Count; i++)
        {
            yield return new WaitForSeconds(0.12f);
        }
    }

    public void DelaySetActive()
    {
        for (int i = 0; i <cardData.SelectCards.Count; i++)
        {
            cardData.SelectCards[i].gameObject.SetActive(false);
        }
    }

    public PokerResult GetPokerResult()
    {
        var result = new PokerResult();
        var selected = cardData.SelectCards.OrderBy(c => c.itemdata.id).ToList();

        foreach (var hand in pokerHands)
        {
            List<int> temp = new();
            hand.PokerHandle(selected, temp);

            if (temp.Count > 0)
            {
                result.SaveNum = temp.OrderBy(x => x).ToList();
                result.Plus = hand.plus;
                result.Multiple = hand.multiple;
                result.PokerName = hand.pokerName;
                return result;
            }
        }

        // ������ ������ ����ī��
        result.SaveNum.Add(selected.Max(c => c.itemdata.id));
        result.Plus = 0;
        result.Multiple = 1;
        result.PokerName = "����ī��";
        return result;
    }

}