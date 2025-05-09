using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public bool ApplyEffect(List<Card> selectedCards, string currentHandType, StateManager stateManager, string jokerCategory, JokerCard myJoker)
    {
        stateManager.MultipleChip.PlusMultiple(bonus);

        AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);           
        
        TextManager.Instance.UpdateText(2, stateManager.MultipleChip.MULTIPLYSum);

        ShowJokerRankText showJokerRankText = myJoker.GetComponent<ShowJokerRankText>();
        showJokerRankText.OnSettingRank(myJoker.currentData.baseData.multiple);

        return true;
    }
}
