using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 추상화 조커 : 조커카드 갯수마다 3배수 (최대 5장)
public class JokerCardEffect : IJokerEffect
{
    private string targetSuit;  // 문양
    private int bonus; // 배수 
    private string category; // 타입

    public JokerCardEffect(string suit, int bonus, string category)
    {
        targetSuit = suit;
        this.bonus = bonus;
        this.category = category;
    }

    public bool ApplyEffect(JokerEffectContext context)
    {
        // 조커 카드가 한장 뿐이라면 자기 자신 애니메이션만 실행
        
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
            seq.AppendInterval(1f); // 카드당 딜레이
        }

        return true;
    }
}
