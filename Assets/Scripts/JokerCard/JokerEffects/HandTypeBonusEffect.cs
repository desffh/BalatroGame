using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HandTypeBonusEffect : IJokerEffect
{
    private string targetType;   // ����
    private int bonus; // ���
    private string category; // Ÿ��

    public HandTypeBonusEffect(string type, int bonus, string category)
    {
        targetType = type;
        this.bonus = bonus;
        this.category = category;
    }

    public bool ApplyEffect(JokerEffectContext context)
    {
        var stateManager = context.StateManager;
        var myJoker = context.MyJoker;
        var jokerCategory = context.CurrentHandType;


        // string.Equals(a, b, StringComparison.OrdinalIgnoreCase) ��ҹ��� ���о��� ���ڿ� ����

        if (string.Equals(jokerCategory, targetType, StringComparison.OrdinalIgnoreCase))
        {
            stateManager.multiplyChipSetting.AddMultiply(bonus);

            AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);

            Debug.Log($"[��Ŀ: {targetType}] ���� ��ġ �� ��� +{bonus}");
        
            ShowJokerRankText showJokerRankText = myJoker.GetComponent<ShowJokerRankText>();
            showJokerRankText.OnSettingRank(myJoker.currentData.baseData.multiple);

            return true;
        }

        return false;
    }
}
