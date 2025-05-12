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

    

    [SerializeField] Money money;

    public void GameResets()
    {
        ServiceLocator.Get<IAudioService>().PlayBGM("MainTheme-Title", true);
        StageManager.Instance.Reset();

        StateManager.Instance.moneyViewSetting.Reset();
        jokerManager.SetupJokerBuffer();
        jokerManager.MyJokerReset();
        ScoreManager.Instance.ResetMaxScore();
    }
}
