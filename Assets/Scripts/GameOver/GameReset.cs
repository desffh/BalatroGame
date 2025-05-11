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

    [SerializeField] Round round;

    [SerializeField] Money money;

    public void GameResets()
    {
        ServiceLocator.Get<IAudioService>().PlayBGM("MainTheme-Title", true);
        round.RoundReset();

        StateManager.Instance.moneyViewSetting.Reset();
        jokerManager.SetupJokerBuffer();
        jokerManager.MyJokerReset();
        ScoreManager.Instance.ResetMaxScore();
    }
}
