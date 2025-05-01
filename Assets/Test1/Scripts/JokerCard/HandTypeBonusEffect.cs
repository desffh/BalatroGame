using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HandTypeBonusEffect : IJokerEffect
{
    private string targetType;   // 예: "트리플"
    private int bonus;
    private string category;

    public HandTypeBonusEffect(string type, int bonus, string category)
    {
        targetType = type;
        this.bonus = bonus;
        this.category = category;
    }

    public void ApplyEffect(List<Card> selectedCards, string currentHandType, HoldManager holdManager, string jokerCategory)
    {
        //if (jokerCategory != category) return;

        if (string.Equals(currentHandType, targetType, StringComparison.OrdinalIgnoreCase))
        {
            holdManager.MultiplySum += bonus;
            Debug.Log($"[조커: {targetType}] 족보 일치 → 배수 +{bonus}");
        }
    }
}
