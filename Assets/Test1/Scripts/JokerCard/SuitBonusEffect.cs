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

    public void ApplyEffect(List<Card> selectedCards, string currentHandType, HoldManager holdManager, string jokerCategory)
    {
        //if (jokerCategory != category) return;

        if (selectedCards.Any(card => card.itemdata.suit == targetSuit))
        {
            holdManager.MultiplySum += bonus;
            Debug.Log($"[조커: {targetSuit}] 문양 일치 → 배수 +{bonus}");
        }
    }
}

