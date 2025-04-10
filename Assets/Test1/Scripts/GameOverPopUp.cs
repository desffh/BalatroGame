using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPopUp : MonoBehaviour
{
    [SerializeField] GameObject GameOverPanel;

    [SerializeField] GameObject RedPanel;


    private void Awake()
    {
        //GameOverPanel.GetComponent<GameObject>(); // ���� ��ũ��Ʈ�� ���� ��ü ���� (������ �Ҵ絵 ����)
    }

    private void Start()
    {
        GameOverPanel.SetActive(false);
        RedPanel.SetActive(false);
    }

    // �ڵ尡 0�̰� ����Ǿ�����,
    public void GameOver()
    {
        GameOverPanel.SetActive(true);
        RedPanel.SetActive(true );
        Time.timeScale = 1.0f;
    }
}
