using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

// �Ϲ� ��Ŀ : 1. �䱸 ������� 4��� ī�� 
public class NormalBonusEffect : IJokerEffect
{
    private string targetSuit;  // �Ϲ� ��Ŀ
    private int bonus;
    private string type; // Ÿ��

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
