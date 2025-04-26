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

    // �ڵ尡 0�̰� ����Ǿ�����,
    public void GameOver()
    {
        SoundManager.Instance.StageFail();

        GameOverPanel.SetActive(true);

        Time.timeScale = 1.0f;
    }

    public void GameOverExit()
    {

        GameOverPanel.SetActive(false);
    }
}
