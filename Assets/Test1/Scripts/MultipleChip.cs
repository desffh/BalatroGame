using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 칩과 배수의 카운팅을 담당
public class MultipleChip : MonoBehaviour
{
    [SerializeField] int PlusSum;

    [SerializeField] int MultiplySum;

    public int PLUSSum => PlusSum;

    public int MULTIPLYSum => MultiplySum;

    public void Start()
    {
        PlusSum = 0;
        MultiplySum = 0;
    }


    public void SettingChip(int  chip = 0)
    {
        PlusSum = chip;
        TextManager.Instance.UpdateText(1, PlusSum);
    }

    public void SettingMultiple(int multiple = 0)
    {
        MultiplySum = multiple;
        TextManager.Instance.UpdateText(2, MultiplySum);

    }

    // |--------------------------------

    public void PlusPlusSum(int chip)
    {
        PlusSum += chip;
        TextManager.Instance.UpdateText(1, PlusSum);
    }

    public void PlusMultiple(int multiple)
    {
        MultiplySum += multiple;
        TextManager.Instance.UpdateText(2, MultiplySum);
    }

    // |--------------------------------

}
