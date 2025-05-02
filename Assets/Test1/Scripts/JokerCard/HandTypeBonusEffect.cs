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

    public void ApplyEffect(List<Card> selectedCards, string currentHandType, HoldManager holdManager, string jokerCategory, JokerCard myJoker)
    {
        if (jokerCategory != category) return;

        if (string.Equals(currentHandType, targetType, StringComparison.OrdinalIgnoreCase))
        {
            holdManager.MultiplySum += bonus;

            AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);

            TextManager.Instance.UpdateText(2, holdManager.MultiplySum);

            Debug.Log($"[��Ŀ: {targetType}] ���� ��ġ �� ��� +{bonus}");
        }
    }
}
