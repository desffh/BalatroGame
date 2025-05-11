using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �߻�ȭ ��Ŀ : ��Ŀī�� �������� 3��� (�ִ� 5��)
public class JokerCardEffect : IJokerEffect
{
    private string targetSuit;  // ����
    private int bonus; // ��� 
    private string category; // Ÿ��

    public JokerCardEffect(string suit, int bonus, string category)
    {
        targetSuit = suit;
        this.bonus = bonus;
        this.category = category;
    }

    public bool ApplyEffect(JokerEffectContext context)
    {
        // ��Ŀ ī�尡 ���� ���̶�� �ڱ� �ڽ� �ִϸ��̼Ǹ� ����
        
        var myjokerCards = context.MyJokerCard;

        var stateManager = context.StateManager;

        var myJoker = context.MyJoker;

        Sequence seq = DOTween.Sequence();

        foreach (var card in myjokerCards.myCards)
        {
            seq.AppendCallback(() =>
            {
                AnimationManager.Instance.PlayJokerCardAnime(card.gameObject);

                ServiceLocator.Get<IAudioService>().PlaySFX("Sound-CheckCard");
                
                stateManager.multiplyChipSetting.AddMultiply(bonus);

                var showJokerRankText = myJoker.GetComponent<ShowJokerRankText>();

                if (showJokerRankText != null)
                    showJokerRankText.OnSettingRank(myJoker.currentData.baseData.multiple);
            });
            seq.AppendInterval(1f); // ī��� ������
        }

        return true;
    }
}
