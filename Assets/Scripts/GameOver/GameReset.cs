using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���� ���
//
// 1. ���� ���� - ���ο� ��
// 2. �ɼ� �г� - ���ο� �� 
public class GameReset : MonoBehaviour
{
    [SerializeField] JokerManager jokerManager;

    [SerializeField] PlanetManager planetManager;

    [SerializeField] TaroManager taroManager;

    [SerializeField] StageButton stageButton;

    public void GameResets()
    {
        ServiceLocator.Get<IAudioService>().PlayBGM("MainTheme-Title", true);
        
        // ��Ƽ, ���� ����
        StageManager.Instance.Reset();

        stageButton.ReSettings();
        
        StateManager.Instance.handDeleteSetting.Reset();

        StateManager.Instance.moneyViewSetting.Reset();
        
        // ��Ŀ ���� ����
        jokerManager.SetupJokerBuffer();
        
        // ��Ŀ ����
        jokerManager.ShuffleBuffer();

        planetManager.SetupPlanetBuffer();

        planetManager.ShuffleBuffer();


        taroManager.SetupTaroBuffer();

        taroManager.ShuffleBuffer();

        PokerManager.Instance.ClearTaroEffects(); // Ÿ�� ī�� ȿ�� �ʱ�ȭ

        PokerManager.Instance.ResetAllUpgrades(); // �༺ī�� ȿ�� �ʱ�ȭ


        jokerManager.MyJokerReset();

        ScoreManager.Instance.ResetMaxScore();

        RunSetting.Instance.ResetSetting(); // �� ���� ����
    }
}
