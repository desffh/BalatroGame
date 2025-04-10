using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PokerManager : Singleton<PokerManager>
{
    private List<IPokerHandle> pokerHands;
    
    // 저장해둘 숫자
    [SerializeField] public List<int> saveNum;
    
    // |------------------------------------------------

    [SerializeField] HandCardPoints deleteCardPoint;

    // 카드 타입을 담을 리스트 (최대 5개)
    [SerializeField] public List<Card> CardIDdata = new List<Card>(5);

    protected override void Awake()
    {
        base.Awake();

        saveNum = new List<int>();
    }

    public PokerManager()
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
                Debug.Log($"족보 선택됨: {hand.pokerName}");
                break;
            }
        }
    }

    // 값을 가져오고 리스트에 저장 (순차적으로) / Card 타입을 받기
    public void SaveSuitIDdata(Card card)
    {
        CardIDdata.Add(card);

        // LinQ메서드를 사용한 오름차순정렬 (value 값 (숫자 갯수) 기준으로)
        CardIDdata = CardIDdata.OrderBy(x => x.itemdata.id).ToList();

        if (CardIDdata.Count >= 0)
        {
            EvaluatePokerHands(CardIDdata);
            TextManager.Instance.UpdateText(1,plus);
            TextManager.Instance.UpdateText(2, multiple);
        
            TextManager.Instance.stringUpdateText(0, pokerName);

        }
    }

    public void RemoveSuitIDdata(Card card)
    {
        // 리스트에서 제거 
        CardIDdata.Remove(card);

        if (CardIDdata.Count > 0)
        {
            EvaluatePokerHands(CardIDdata);
        }
        else
        {
            // 카드가 다 제거된 경우에도 saveNum 초기화
            saveNum.Clear();
            Debug.Log("모든 카드가 제거되어 saveNum 초기화됨.");
            TextManager.Instance.UpdateText(1);
            TextManager.Instance.UpdateText(2);

            TextManager.Instance.stringUpdateText(0);
        }

    }

    
    public void QuitCollider2()
    {
        for (int i = 0; i < CardIDdata.Count; i++)
        {
            CardIDdata[i].gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    // 다 판별하고 버리기
    public void DeleteMove()
    {
        for (int i = 0; i < CardIDdata.Count; i++)
        {
            CardIDdata[i].transform.GetComponent<Transform>();

            CardIDdata[i].transform.
                DOMove(deleteCardPoint.DeleteCardpos.transform.position, 1).SetDelay(i * 0.2f);

            CardIDdata[i].transform.
                DORotate(new Vector3(-45, -60, -25), 0.5f).SetDelay(i * 0.2f);
        }
    }

    public void DelaySetActive()
    {
        for (int i = 0; i <CardIDdata.Count; i++)
        {
            CardIDdata[i].gameObject.SetActive(false);
        }
    }
}