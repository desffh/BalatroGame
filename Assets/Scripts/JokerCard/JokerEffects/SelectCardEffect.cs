using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 반쪽짜리 조커 : SelectedCards가 3개 이하라면 20배수
public class SelectCardEffect : IJokerEffect
{
    private string targetSuit;  // 문양
    private int bonus; // 배수 
    private string category; // 타입

    public SelectCardEffect(string suit, int bonus, string category)
    {
        targetSuit = suit;
        this.bonus = bonus;
        this.category = category;
    }

    public bool ApplyEffect(JokerEffectContext context)
    {
        var selectedCards = context.SelectedCards;

        var stateManager = context.StateManager;
        
        var myJoker = context.MyJoker;

        if(selectedCards.Count > 3)
        {
            return false;
        }

        AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);

        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-CheckCard");

        stateManager.multiplyChipSetting.AddMultiply(bonus);

        ShowJokerRankText showJokerRankText = myJoker.GetComponent<ShowJokerRankText>();
        showJokerRankText.OnSettingRank(myJoker.currentData.baseData.multiple);

        return true;
    }
}
