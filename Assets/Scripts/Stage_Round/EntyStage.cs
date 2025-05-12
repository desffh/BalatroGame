using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ũ���ͺ� ������Ʈ ���� (��)
//
// ���� ��Ƽ. ����ε�3���� ������ ����
[CreateAssetMenu(fileName = "EntyStage", menuName = "Stage/EntyStage")]
public class EntyStage : ScriptableObject
{
    // 1��Ƽ ���ھ� 
    public int baseScore;

    // ���� ����ε� ���ھ�
    public int [] blindScore = new int[3];


    [Header("�Ϲ� ����ε� ���� (2��)")]
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
    
    // ���� ����ε� ���ھ� �迭 ����
    public void GetBlind()
    {
        for (int i = 0; i < blindScore.Length; i++)
        {
            blindScore[i] = baseScore + (i * baseScore / 2);
        }
    }
}
