using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{ 
    [SerializeField] private int totalScore; // 스테이지 전체 점수
    [SerializeField] private int maxScore;   // 가장 높은 점수 

    // 읽기 전용 프로퍼티 
    public int TotalScore => totalScore;
    public int MaxScore => maxScore;

    // |------------------------------------------------
    
    // 점수 리셋
    public void ResetTotalScore()
    {
        totalScore = 0;
    }

    // 점수 추가
    public void AddScore(int score)
    {
        totalScore += score;

        if(totalScore > maxScore)
        {
            maxScore = totalScore;
        }
    }

    // 스테이지 종료 체크 -> 내 점수가 더 크다면 true
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
