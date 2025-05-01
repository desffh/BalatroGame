using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HandTypeBonusEffect : IJokerEffect
{
    private string targetType;   // ��: "Ʈ����"
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
            Debug.Log($"[��Ŀ: {targetType}] ���� ��ġ �� ��� +{bonus}");
        }
    }
}
