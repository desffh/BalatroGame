using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EvenOddBonusEffect : IJokerEffect
{
    private string targetSuit;  
    private int bonus;
    private string category; // 홀수 / 짝수

    public EvenOddBonusEffect(string suit, int bonus, string category)
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


        bool anyOdd = selectedCards.Any(card => card.itemdata.id % 2 != 0); // 하나라도 홀수면 true

        if (category == "Odd" && !anyOdd) // 홀수 조커인데 홀수가 하나도 없으면
            return false; // 실행하지 않음

        bool anyEven = selectedCards.Any(card => card.itemdata.id % 2 == 0);

        if (category == "Even" && !anyEven)
            return false;

        bool scoreApplied = false;

        foreach (var card in selectedCards)
        {
            if (category == "Odd" && card.itemdata.id % 2 != 0)
            {
                AnimationManager.Instance.PlayCardAnime(card.gameObject);
                Debug.Log($"[조커: {targetSuit}] 홀수 카드 애니메이션: {card.name}");

                if (!scoreApplied)
                {
                    AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);
                    ServiceLocator.Get<IAudioService>().PlaySFX("Sound-CheckCard");

                    stateManager.multiplyChipSetting.AddPlus(bonus);
                    myJoker.GetComponent<ShowJokerRankText>()?.OnSettingChip(myJoker.currentData.baseData.multiple);

                    scoreApplied = true;
                }
            }
            else if (category == "Even" && card.itemdata.id % 2 == 0)
            {
                AnimationManager.Instance.PlayCardAnime(card.gameObject);
                Debug.Log($"[조커: {targetSuit}] 짝수 카드 애니메이션: {card.name}");

                if (!scoreApplied)
                {
                    AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);
                    ServiceLocator.Get<IAudioService>().PlaySFX("Sound-CheckCard");

                    stateManager.multiplyChipSetting.AddMultiply(bonus);
                    myJoker.GetComponent<ShowJokerRankText>()?.OnSettingRank(myJoker.currentData.baseData.multiple);

                    scoreApplied = true;
                }
            }
        }


        return true;
    }
}
