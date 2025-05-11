using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ����¥�� ��Ŀ : SelectedCards�� 3�� ���϶�� 20���
public class SelectCardEffect : IJokerEffect
{
    private string targetSuit;  // ����
    private int bonus; // ��� 
    private string category; // Ÿ��

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
