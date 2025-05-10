using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ĩ�� ����� ������ ����

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



    // ī��Ʈ ����
    public void ResetCounts()
    {
        plusSum = 0;
        multiplySum = 0;
    }

    // Ĩ ���� -> ���� ��ġ �� ȣ��
    public void SettingChips(int chip)
    {
        plusSum = chip;

    }

    // ��� ���� -> ���� ��ġ �� ȣ��
    public void SettingMultiply(int multi)
    {
        multiplySum = multi;

    }

    // ��� �� Ĩ �߰�
    public void PlusPlusSum(int chip)
    {
        plusSum += chip;

    }

    // ��� �� ��� �߰�
    public void PlusMultiple(int multiple)
    {
        multiplySum += multiple;
    }
}
