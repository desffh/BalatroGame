using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipBonusEffect : IJokerEffect
{
    private string targetType;   // 예: "트리플"
    private int bonus; // 칩 배수
    private string category; // 조커 타입

    public ChipBonusEffect(string type, int bonus, string category)
    {
        targetType = type;
        this.bonus = bonus;
        this.category = category;
    }

    public bool ApplyEffect(List<Card> selectedCards, string currentHandType, StateManager stateManager, string jokerCategory, JokerCard myJoker)
    {
        if (string.Equals(currentHandType, targetType, StringComparison.OrdinalIgnoreCase))
        {
            stateManager.MultipleChip.PlusPlusSum(bonus);

            AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);

            TextManager.Instance.UpdateText(1, stateManager.MultipleChip.PLUSSum);

            Debug.Log($"[조커: {targetType}] 족보 일치 → 칩 +{bonus}");

            ShowJokerRankText showJokerRankText = myJoker.GetComponent<ShowJokerRankText>();
            showJokerRankText.OnSettingChip(myJoker.currentData.baseData.multiple);

            return true;
        }

        return false;
    }
}
