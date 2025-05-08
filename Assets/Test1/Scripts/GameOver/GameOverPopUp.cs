using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���� �˾� Ȱ��ȭ / ��Ȱ��ȭ ���
public class GameOverPopUp : MonoBehaviour
{
    [SerializeField] GameObject GameOverPanel;


    private void Start()
    {
        GameOverPanel.SetActive(false);

    }

    // �ڵ尡 0�̰� ��ǥ ������ �޼����� ������ ��, ����
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
