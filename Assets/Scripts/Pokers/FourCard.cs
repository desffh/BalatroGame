using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FourCard : IsStrightPlush, IPokerHandle
{
    public string pokerName => "포 카드";

    // 기본 데이터 
    private int basePlus = 60;
    private int baseMultiple = 7;

    // 업그레이드 시 추가
    private int upgradePlus = 0;
    private int upgradeMultiple = 0;

    public int plus => basePlus + upgradePlus;
    public int multiple => baseMultiple + upgradeMultiple;

    // 행성카드를 사용하면 추가
    public void ApplyUpgrade(int plusData, int multipleData)
    {
        upgradePlus += plusData;
        upgradeMultiple += multipleData;
    }


    public void PokerHandle(List<Card> cards, List<int> saveNum)
    {
        var cardCount = CardCount.Hand(cards);

        // 숫자 4개인 항목이 하나라도 있으면 포카드
        var fourCardNumbers = cardCount
            .Where(x => x.Value == 4)
            .Select(x => x.Key)
            .ToList();

        if (fourCardNumbers.Count == 0)
            return;

        // 해당 숫자와 일치하는 카드만 추출 (4장만)
        var matchedCards = cards
            .Where(card => fourCardNumbers.Contains(card.itemdata.id))
            .ToList();


        if (matchedCards.Count == 4)
        {

            foreach (var card in matchedCards)
            {
                saveNum.Add(card.itemdata.id);
            }
        }
        else
        {
            // 카드가 5장이고 (4+1 구조일 가능성)
            if (cards.Count == 5 && cardCount.Count == 2)
            {
                // cardCount 중 Value == 4인 키를 찾기
                var four = cardCount.FirstOrDefault(x => x.Value == 4);
                if (four.Key != 0)
                {
                    matchedCards = cards
                        .Where(c => c.itemdata.id == four.Key)
                        .ToList();

                    foreach (var card in matchedCards)
                    {
                        saveNum.Add(card.itemdata.id);
                    }
                }
            }
        }
    }

    public void ResetUpgrade()
    {
        upgradePlus = 0;
        upgradeMultiple = 0;
    }
}
