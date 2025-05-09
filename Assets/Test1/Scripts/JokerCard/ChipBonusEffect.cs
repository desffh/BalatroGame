using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipBonusEffect : IJokerEffect
{
    private string targetType;   // ��: "Ʈ����"
    private int bonus; // Ĩ ���
    private string category; // ��Ŀ Ÿ��

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

            Debug.Log($"[��Ŀ: {targetType}] ���� ��ġ �� Ĩ +{bonus}");

            ShowJokerRankText showJokerRankText = myJoker.GetComponent<ShowJokerRankText>();
            showJokerRankText.OnSettingChip(myJoker.currentData.baseData.multiple);

            return true;
        }

        return false;
    }
}
