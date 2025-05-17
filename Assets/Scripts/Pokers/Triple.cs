using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class Triple : IPokerHandle
{
    public string pokerName => "트리플";
    
    // 기본 데이터 
    private int basePlus = 30;
    private int baseMultiple = 3;

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
        Dictionary<int, int> cardCount = CardCount.Hand(cards);

        // 숫자 3개가 있는 항목이 존재하는지 확인
        if (cardCount.Values.Contains(3))
        {
            // 트리플 숫자 추출 (예: 6이 3개라면 6)
            var tripleNumbers = cardCount
                .Where(x => x.Value == 3)
                .Select(x => x.Key)
                .ToList();

            // 해당 숫자를 가진 카드들 필터링 (총 3장)
            var matchedCards = cards
                .Where(c => tripleNumbers.Contains(c.itemdata.id))
                .ToList();


            foreach (var card in matchedCards)
            {
                saveNum.Add(card.itemdata.id);
            }
        }
    }
    public void ResetUpgrade()
    {
        upgradePlus = 0;
        upgradeMultiple = 0;
    }
}
