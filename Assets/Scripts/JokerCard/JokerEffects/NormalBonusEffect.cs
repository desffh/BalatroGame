using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

// 일반 조커 : 1. 요구 상관없이 4배수 카드 
public class NormalBonusEffect : IJokerEffect
{
    private string targetSuit;  // 일반 조커
    private int bonus;
    private string type; // 타입

    public NormalBonusEffect(string suit, int bonus, string category)
    {
        targetSuit = suit;
        this.bonus = bonus;
        this.type = category;
    }

    public bool ApplyEffect(JokerEffectContext context)
    {
        var stateManager = context.StateManager;
        var myJoker = context.MyJoker;

        stateManager.multiplyChipSetting.AddMultiply(bonus);

        AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);           
        


        ShowJokerRankText showJokerRankText = myJoker.GetComponent<ShowJokerRankText>();
        showJokerRankText.OnSettingRank(myJoker.currentData.baseData.multiple);

        return true;
    }
}
