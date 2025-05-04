using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NormalBonusEffect : IJokerEffect
{
    private string targetSuit;  // �Ϲ� ��Ŀ
    private int bonus;
    private string category;

    public NormalBonusEffect(string suit, int bonus, string category)
    {
        targetSuit = suit;
        this.bonus = bonus;
        this.category = category;
    }

    public void ApplyEffect(List<Card> selectedCards, string currentHandType, HoldManager holdManager, string jokerCategory, JokerCard myJoker)
    {
        Debug.Log("���� Ȯ�� �ҰԿ�");


        holdManager.MultiplySum += bonus;

        AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);           
        
        TextManager.Instance.UpdateText(2, holdManager.MultiplySum);

        ShowJokerRankText showJokerRankText = myJoker.GetComponent<ShowJokerRankText>();
        showJokerRankText.OnSettingRank(myJoker.currentData.baseData.multiple);
    }
}
