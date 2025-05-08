using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임 리셋 담당
//
// 1. 게임 오버 - 새로운 런
// 2. 옵션 패널 - 새로운 런 
public class GameReset : MonoBehaviour
{
    [SerializeField] JokerManager jokerManager;

    [SerializeField] Round round;

    [SerializeField] Money money;

    public void GameResets()
    {
        ServiceLocator.Get<IAudioService>().PlayBGM("MainTheme-Title", true);
        round.RoundReset();
        money.ReSetTotalMoney();
        money.MoneyUpdate();
        jokerManager.SetupJokerBuffer();
        jokerManager.MyJokerReset();
        ScoreManager.Instance.ResetMaxScore();
    }
}
