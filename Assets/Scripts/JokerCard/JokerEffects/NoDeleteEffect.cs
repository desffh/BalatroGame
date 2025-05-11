using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NoDeleteEffect : IJokerEffect
{
    private string targetSuit; // 문양
    private int bonus; // 배수 
    private string category; // 타입

    public NoDeleteEffect(string suit, int bonus, string category)
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


        // 깃발조커인데 남은 버리기가 0일 경우
        if (myJoker.data.name == "깃발" && stateManager.handDeleteSetting.GetDelete() == 0)
        {
            return false;
        }

        // 신비의 정점 조커인데 남은 버리기가 0이 아닐 경우
        if (myJoker.data.name == "신비의 정점" && stateManager.handDeleteSetting.GetDelete() != 0)
        {
            return false;
        }

        AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);
        
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-CheckCard");

        if(myJoker.data.name == "깃발")
        {
            int currentDelete = stateManager.handDeleteSetting.GetDelete();

            stateManager.multiplyChipSetting.AddPlus(bonus * currentDelete);

            myJoker.GetComponent<ShowJokerRankText>()?.OnSettingChip(bonus * currentDelete);
        }

        else if(myJoker.data.name == "신비의 정점")
        {
            stateManager.multiplyChipSetting.AddMultiply(bonus);

            myJoker.GetComponent<ShowJokerRankText>()?.OnSettingRank(myJoker.data.multiple);
        }

        return true;
    }
}
