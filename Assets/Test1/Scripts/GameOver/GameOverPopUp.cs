using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPopUp : MonoBehaviour
{
    [SerializeField] GameObject GameOverPanel;


    private void Start()
    {
        GameOverPanel.SetActive(false);

    }

    // 핸드가 0이고 종료되었을때,
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
