using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] TextManager textManager;

    // 현재 전체 점수
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
            Debug.Log("목표 점수 달성");

            // 스테이지 플레이 종료
            GameManager.Instance.PlayOff();
            return true;
        }
        else
        {
            Debug.Log("목표 점수 실패 - 더 플레이하세요");

            return false;
        }
    }
}
