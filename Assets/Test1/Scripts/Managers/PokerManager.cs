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
    
    // 저장해둘 숫자
    [SerializeField] public List<int> saveNum;
    
    // |------------------------------------------------

    [SerializeField] HandCardPoints deleteCardPoint;




    [SerializeField] public CardData cardData;

    protected override void Awake()
    {
        base.Awake();

        saveNum = new List<int>();
        // 족보 리스트 초기화

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
        saveNum = new List<int>();  // saveNum 리스트는 여기서 한 번만 생성
    }


    public int plus;
    public int multiple;
    public string pokerName;

    // 카드 리스트를 평가하여 족보 이름과 점수를 출력
    public void EvaluatePokerHands(List<Card> cards)
    {
        saveNum.Clear();

        foreach (var hand in pokerHands)
        {
            List<int> tempSaveNum = new List<int>(); // 임시 저장 리스트

            hand.PokerHandle(cards, tempSaveNum);

            plus = hand.plus;
            multiple = hand.multiple;
            pokerName = hand.pokerName;

            if (tempSaveNum.Count > 0) // 첫 번째로 매칭되는 족보만 저장
            {
                saveNum = tempSaveNum;
                //Debug.Log($"족보 선택됨: {hand.pokerName}");
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

            TextManager.Instance.UpdateText(1);       // 기본값 초기화
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

    // 다 판별하고 버리기
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

        // 족보가 없으면 하이카드
        result.SaveNum.Add(selected.Max(c => c.itemdata.id));
        result.Plus = 0;
        result.Multiple = 1;
        result.PokerName = "하이카드";
        return result;
    }

}