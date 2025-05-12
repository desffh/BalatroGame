using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스크립터블 오브젝트 생성 (모델)
//
// 현재 엔티. 블라인드3개의 점수만 셋팅
[CreateAssetMenu(fileName = "EntyStage", menuName = "Stage/EntyStage")]
public class EntyStage : ScriptableObject
{
    // 1엔티 스코어 
    public int baseScore;

    // 하위 블라인드 스코어
    public int [] blindScore = new int[3];


    [Header("일반 블라인드 정보 (2개)")]
    public string[] normalBlindNames = new string[2];

    public Sprite[] normalBlindImages = new Sprite[2];

    public int[] normalMoneys = new int[3];

    public Color[] normalColors = new Color[3];


    // blindIndex: 0 1 2
    public int GetBlindScore(int blindIndex)
    {

        blindScore[blindIndex] = baseScore + (blindIndex * baseScore / 2);

        return blindScore[blindIndex];
    }
    
    // 하위 블라인드 스코어 배열 셋팅
    public void GetBlind()
    {
        for (int i = 0; i < blindScore.Length; i++)
        {
            blindScore[i] = baseScore + (i * baseScore / 2);
        }
    }
}
