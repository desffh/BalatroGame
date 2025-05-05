using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SuitBonusEffect : IJokerEffect
{
    private string targetSuit;  // 예: "하트"
    private int bonus;
    private string category;

    public SuitBonusEffect(string suit, int bonus, string category)
    {
        targetSuit = suit;
        this.bonus = bonus;
        this.category = category;
    }

    public bool ApplyEffect(List<Card> selectedCards, string currentHandType, HoldManager holdManager, string jokerCategory, JokerCard myJoker)
    {
        var matchedCard = selectedCards.Where(card => card.itemdata.suit == targetSuit).ToList();

        if (matchedCard.Count > 0)
        {
            holdManager.MultiplySum += bonus;

            AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);

            // 문양과 일치하는 카드들의 애니메이션 실행
            foreach (var card in matchedCard)
            {
                AnimationManager.Instance.PlayCardAnime(card.gameObject);
                Debug.Log($"[조커: {targetSuit}] 문양 일치 → 애니메이션 대상: {card.name}");
            }

            TextManager.Instance.UpdateText(2, holdManager.MultiplySum);

            ShowJokerRankText showJokerRankText = myJoker.GetComponent<ShowJokerRankText>();
            showJokerRankText.OnSettingRank(myJoker.currentData.baseData.multiple);

            return true;
        }

        // 그렇지 않다면 실패 효과음과 애니메이션 실행
        return false;
    }
}

