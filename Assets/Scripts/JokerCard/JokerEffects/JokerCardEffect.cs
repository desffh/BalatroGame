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

    private Sequence cachedSequence; // 외부에서 기다릴 수 있게 보관
    public JokerCardEffect(string suit, int bonus, string category)
    {
        targetSuit = suit;
        this.bonus = bonus;
        this.category = category;
    }

    public bool ApplyEffect(JokerEffectContext context)
    {
        // 조커가 자기 자신뿐이라면 본인만 실행
        var myjokerCards = context.MyJokerCard;
        var stateManager = context.StateManager;
        var myJoker = context.MyJoker;

        cachedSequence = DOTween.Sequence();

        foreach (var card in myjokerCards.myCards)
        {
            cachedSequence.AppendCallback(() =>
            {
                AnimationManager.Instance.PlayJokerCardAnime(card.gameObject);
                ServiceLocator.Get<IAudioService>().PlaySFX("Sound-CheckCard");
                stateManager.multiplyChipSetting.AddMultiply(bonus);

                var showJokerRankText = myJoker.GetComponent<ShowJokerRankText>();
                if (showJokerRankText != null)
                    showJokerRankText.OnSettingRank(myJoker.currentData.baseData.multiple);
            });

            cachedSequence.AppendInterval(1f);
        }

        return true;
    }

    // 조커 카드 애니메이션이 모두 종료될 때까지 대기
    public Sequence GetAnimationSequence()
    {
        return cachedSequence;
    }
}
