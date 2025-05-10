using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 칩과 배수의 데이터 저장

[CreateAssetMenu(menuName = "Model/MultipleChip")]
public class MultipleChipData : ScriptableObject
{
    [SerializeField] private int plusSum = 0;

    [SerializeField] private int multiplySum = 0;

    public int PlusSum
    {
        get { return plusSum; }
        set { plusSum = value;}
    }

    public int MultiplySum
    {
        get { return multiplySum; }
        set { multiplySum = value; }
    }



    // 카운트 리셋
    public void ResetCounts()
    {
        plusSum = 0;
        multiplySum = 0;
    }

    // 칩 대입 -> 족보 일치 시 호출
    public void SettingChips(int chip)
    {
        plusSum = chip;

    }

    // 배수 대입 -> 족보 일치 시 호출
    public void SettingMultiply(int multi)
    {
        multiplySum = multi;

    }

    // 계산 시 칩 추가
    public void PlusPlusSum(int chip)
    {
        plusSum += chip;

    }

    // 계산 시 배수 추가
    public void PlusMultiple(int multiple)
    {
        multiplySum += multiple;
    }
}
