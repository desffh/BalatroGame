using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{ 
    [SerializeField] private int totalScore; // �������� ��ü ����
    [SerializeField] private int maxScore;   // ���� ���� ���� 

    // �б� ���� ������Ƽ 
    public int TotalScore => totalScore;
    public int MaxScore => maxScore;

    // |------------------------------------------------
    
    // ���� ����
    public void ResetTotalScore()
    {
        totalScore = 0;
    }

    // ���� �߰�
    public void AddScore(int score)
    {
        totalScore += score;

        if(totalScore > maxScore)
        {
            maxScore = totalScore;
        }
    }

    // �������� ���� üũ -> �� ������ �� ũ�ٸ� true
    public bool CheckStageClear(int targetScore)
    {
        if (totalScore >= targetScore)
        {
            GameManager.Instance.PlayOff();

            return true;
        }

        return false;
    }
}
