using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;


// 포커 이름, 저장된 SaveNum을 HoldManager로 전달
public class PokerResult
{
    public List<int> SaveNum = new();
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
        saveNum = new List<int>();  // saveNum 리스트는 여기서 한 번만 생성
    }

    private void Start()
    {
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

    // |---

    // 현재 족보가 포함하는 하위 족보까지 반환
    public List<string> GetContainedHandTypes()
    {
        switch (pokerName)
        {
            case "스트레이트 플러시":
                return new List<string> { "스트레이트", "플러시", "스트레이트 플러시" };
            case "풀 하우스":
                return new List<string> { "페어", "트리플", "풀 하우스" };
            case "포 카드":
                return new List<string> { "페어", "트리플", "포 카드" };
            case "플러시":
                return new List<string> { "플러시" };
            case "스트레이트":
                return new List<string> { "스트레이트" };
            case "투 페어":
                return new List<string> { "페어", "투 페어" };
            default:
                return new List<string> { pokerName };
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

            StateManager.Instance.multiplyChipSetting.SetPlus(plus);
            StateManager.Instance.multiplyChipSetting.SetMultiply(multiple);

            TextManager.Instance.stringUpdateText(0, pokerName);
        }
        else
        {
            saveNum.Clear();

            StateManager.Instance.multiplyChipSetting.SetPlus();
            StateManager.Instance.multiplyChipSetting.SetMultiply();

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



                StateManager.Instance.multiplyChipSetting.SetPlus(hand.plus);
                StateManager.Instance.multiplyChipSetting.SetMultiply(hand.multiple);


                result.PokerName = hand.pokerName;
                return result;
            }
        }

        // 족보가 없으면 하이카드
        result.SaveNum.Add(selected.Max(c => c.itemdata.id));
        result.PokerName = "하이카드";
        return result;
    }

}