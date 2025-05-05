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

    public bool ApplyEffect(List<Card> selectedCards, string currentHandType, HoldManager holdManager, string jokerCategory, JokerCard myJoker)
    {

        if (string.Equals(currentHandType, targetType, StringComparison.OrdinalIgnoreCase))
        {
            holdManager.MultiplySum += bonus;

            AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);

            TextManager.Instance.UpdateText(2, holdManager.MultiplySum);

            Debug.Log($"[조커: {targetType}] 족보 일치 → 배수 +{bonus}");
        
            ShowJokerRankText showJokerRankText = myJoker.GetComponent<ShowJokerRankText>();
            showJokerRankText.OnSettingRank(myJoker.currentData.baseData.multiple);

            return true;
        }

        return false;
    }
}
