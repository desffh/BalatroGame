using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임 오버 팝업 활성화 / 비활성화 담당
public class GameOverPopUp : MonoBehaviour
{
    [SerializeField] GameObject GameOverPanel;


    private void Start()
    {
        GameOverPanel.SetActive(false);

    }

    // 핸드가 0이고 목표 점수를 달성하지 못했을 때, 종료
    public void GameOver()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-Fail");

        GameOverPanel.SetActive(true);

        Time.timeScale = 1.0f;
    }

    public void GameOverExit()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        GameOverPanel.SetActive(false);
    }
}
