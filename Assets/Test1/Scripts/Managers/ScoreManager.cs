using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    // ���� ��ü ����
    [SerializeField] private int totalScore;

    public int TotalScore
    {
        get { return totalScore; }

        set { 
            
            if(value == 0)
            {
                totalScore = 0;
            }

            totalScore += value; 
        }
    }


    public bool Calculation()
    { 
        if(TotalScore >= Round.Instance.CurrentScores)
        {
            Debug.Log("��ǥ ���� �޼�");

            // �������� �÷��� ����
            GameManager.Instance.PlayOff();
            return true;
        }
        else
        {
            Debug.Log("��ǥ ���� ���� - �� �÷����ϼ���");

            return false;
        }
    }
}
