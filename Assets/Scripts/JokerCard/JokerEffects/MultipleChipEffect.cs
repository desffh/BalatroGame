using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultipleChipEffect : IJokerEffect
{
    private string targetSuit; // 문양
    private int bonus; // 배수 
    private string category; // 타입

    public MultipleChipEffect(string suit, int bonus, string category)
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

        // 10이나 4가 하나도 없으면 false
        bool noTenFour = selectedCards.Any(card => card.itemdata.id == 10 || card.itemdata.id == 4);


        // 워키토키 조커인데 카드에 10이나 4가 하나도 없을 경우 
        if (myJoker.data.name == "워키토키" && !noTenFour)
        {
            return false;
        }

        // 14가 하나라도 없으면 false
        bool noAce = selectedCards.Any(card => card.itemdata.id == 14);

        // 학자 조커인데 에이스(14)가 하나도 없을 경우
        if (myJoker.data.name == "학자 조커" && !noAce)
        {
            return false;
        }

        AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);

        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-CheckCard");


        bool scoreApplied = false;

        foreach (var card in selectedCards)
        {
            if (myJoker.data.name == "워키토키" && card.itemdata.id == 10 || card.itemdata.id == 4)
            {
                AnimationManager.Instance.PlayCardAnime(card.gameObject);

                int chip1 = 10;

                if(!scoreApplied)
                {
                    stateManager.multiplyChipSetting.AddMultiply(bonus); // 4배수
                    stateManager.multiplyChipSetting.AddPlus(chip1); // 10칩

                    myJoker.GetComponent<ShowJokerRankText>()?.OnSettingMultiplyChip( chip1, myJoker.data.multiple);

                    scoreApplied = true;
                }
            }

            else if (myJoker.data.name == "학자 조커" && card.itemdata.id == 14)
            {
                AnimationManager.Instance.PlayCardAnime(card.gameObject);

                int chip2 = 20;
                
                if(!scoreApplied)
                {
                    stateManager.multiplyChipSetting.AddMultiply(bonus); // 4배수
                    stateManager.multiplyChipSetting.AddPlus(chip2); // 20칩

                    myJoker.GetComponent<ShowJokerRankText>()?.OnSettingMultiplyChip(chip2, myJoker.data.multiple);

                    scoreApplied = true;
                }
            }

        }
        return true;
    }
}
