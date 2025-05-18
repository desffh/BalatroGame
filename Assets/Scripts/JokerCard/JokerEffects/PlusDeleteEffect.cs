using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 일반 조커 : 버리기 횟수 추가 카드
public class PlusDeleteEffect : IJokerEffect, IExitEffect, IResettableEffect
{
    private string targetSuit;  // 일반 조커
    private int multiply;
    private string type; // 타입

    private bool isApplied = false; // 1회 적용 여부 플래그

    public PlusDeleteEffect(string suit, int bonus, string category)
    {
        targetSuit = suit;
        this.multiply = bonus;
        this.type = category;
    }

    public bool ApplyEffect(JokerEffectContext context)
    {
        var stateManager = context.StateManager;
        var myJoker = context.MyJoker;


        if (isApplied == true)
            return false; // 이미 적용되었으면 아무것도 하지 않음

        // 버리기 횟수 추가

        stateManager.handDeleteSetting.PlusDelete();

        AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);

        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-CheckCard");

        ShowJokerRankText showJokerRankText = myJoker.GetComponent<ShowJokerRankText>();

        showJokerRankText.OnSettingDelete(myJoker.currentData.baseData.multiple);

        isApplied = true; // 한 번만 적용되도록 플래그 설정

        return true;
    }

    // 조커 카드 판매 시 호출
    public void ExitEffect(JokerCard myJoker)
    {
        if(isApplied)
        {
            // 버리기 횟수 감소

            StateManager.Instance.handDeleteSetting.MinusDelete();
        }
    }

    // bool 변수 초기화 하여 다시 조커 로직 실행
    public void ResetEffect()
    {
        isApplied = false;
    }
}
