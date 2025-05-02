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

    public void ApplyEffect(List<Card> selectedCards, string currentHandType, HoldManager holdManager, string jokerCategory, JokerCard myJoker)
    {
        Debug.Log("문양 확인 할게요");

        var matchedCard = selectedCards.FirstOrDefault(card => card.itemdata.suit == targetSuit);

        if (matchedCard != null)
        {
            holdManager.MultiplySum += bonus;

            AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);

            AnimationManager.Instance.PlayCardAnime(matchedCard.gameObject); // ← 여기에 전달

            TextManager.Instance.UpdateText(2, holdManager.MultiplySum);

            Debug.Log($"[조커: {targetSuit}] 문양 일치 → 배수 +{bonus}, 애니메이션 대상: {matchedCard.name}");
        }

    }
}

