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

    [SerializeField] PlanetManager planetManager;

    [SerializeField] TaroManager taroManager;

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

        planetManager.SetupPlanetBuffer();

        planetManager.ShuffleBuffer();


        taroManager.SetupTaroBuffer();

        taroManager.ShuffleBuffer();

        PokerManager.Instance.ClearTaroEffects(); // 타로 카드 효과 초기화

        PokerManager.Instance.ResetAllUpgrades(); // 행성카드 효과 초기화


        jokerManager.MyJokerReset();

        ScoreManager.Instance.ResetMaxScore();

        RunSetting.Instance.ResetSetting(); // 런 정보 리셋
    }
}
