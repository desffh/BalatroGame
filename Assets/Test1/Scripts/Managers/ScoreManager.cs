using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{ 
    [SerializeField] private int totalScore; // 스테이지 전체 점수
    [SerializeField] private int maxScore;   // 가장 높은 점수 

    [SerializeField] GameOverText bestHand; // 베스트 핸드 갱신을 위한 참조

    // 읽기 전용 프로퍼티 
    public int TotalScore => totalScore;
    public int MaxScore => maxScore;

    // |------------------------------------------------
    
    // 점수 리셋
    public void ResetTotalScore()
    {
        totalScore = 0;
    }

    public void ResetMaxScore()
    {
        maxScore = 0;
    }

    // 점수 추가
    public void AddScore(int score)
    {
        totalScore += score;

        // 방금 들어온 계산이 더 높을 경우 갱신
        if(score > maxScore)
        {
            maxScore = score;
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

    // 게임 오버 시 호출
    public void BestHand()
    {
        bestHand.BestHandUpdate(MaxScore);
    }


    [SerializeField] private TextMeshProUGUI scoreText;

    public void AnimateScore(int from, int to, float duration = 1f)
    {
        DOTween.To(() => from, x => {
            from = x;
            scoreText.text = from.ToString();
        }, to, duration).SetEase(Ease.OutQuad);
    }
}
