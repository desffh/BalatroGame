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

    [SerializeField] StageButton stageButton;

    public void GameResets()
    {
        ServiceLocator.Get<IAudioService>().PlayBGM("MainTheme-Title", true);
        
        // 엔티, 라운드 리셋
        StageManager.Instance.Reset();

        stageButton.ReSettings();
        
        StateManager.Instance.handDeleteSetting.Reset();

        StateManager.Instance.moneyViewSetting.Reset();
        
        // 조커 버퍼 셋팅
        jokerManager.SetupJokerBuffer();
        
        // 조커 셔플
        jokerManager.ShuffleBuffer();
        
        jokerManager.MyJokerReset();
        ScoreManager.Instance.ResetMaxScore();
    }
}
