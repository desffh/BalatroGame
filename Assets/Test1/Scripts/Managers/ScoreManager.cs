using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{ 
    [SerializeField] private int totalScore; // �������� ��ü ����
    [SerializeField] private int maxScore;   // ���� ���� ���� 

    [SerializeField] GameOverText bestHand; // ����Ʈ �ڵ� ������ ���� ����

    // �б� ���� ������Ƽ 
    public int TotalScore => totalScore;
    public int MaxScore => maxScore;

    // |------------------------------------------------
    
    // ���� ����
    public void ResetTotalScore()
    {
        totalScore = 0;
    }

    public void ResetMaxScore()
    {
        maxScore = 0;
    }

    // ���� �߰�
    public void AddScore(int score)
    {
        totalScore += score;

        // ��� ���� ����� �� ���� ��� ����
        if(score > maxScore)
        {
            maxScore = score;
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

    // ���� ���� �� ȣ��
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
