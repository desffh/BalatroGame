using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SuitBonusEffect : IJokerEffect
{
    private string targetSuit;  // ��: "��Ʈ"
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
            Debug.Log($"[��Ŀ: {targetSuit}] ���� ��ġ �� ��� +{bonus}");
        }
    }
}

