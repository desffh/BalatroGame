using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class OnePair : IPokerHandle
{
    public string pokerName => "페어";

    // 기본 데이터 
    private int basePlus = 10;
    private int baseMultiple = 2;

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

        // 숫자 2개짜리 한 쌍이 존재하면
        if (cardCount.Values.Contains(2))
        {
            // 페어인 숫자 추출 (보통 하나만 있을 것)
            var pairNumbers = cardCount
                .Where(x => x.Value == 2)
                .Select(x => x.Key)
                .ToList();

            // 카드 중에서 해당 숫자만 필터링
            var matchedCards = cards
                .Where(c => pairNumbers.Contains(c.itemdata.id))
                .ToList();

            
            // 디버프 적용 후 저장
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
