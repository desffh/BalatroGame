using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleChipPresenter : IMultiplyChipSetting
{
    private MultipleChipData data;
    private MultipleChipView view;

    public MultipleChipPresenter(MultipleChipData data, MultipleChipView view)
    {
        this.data = data;
        this.view = view;

        // �ʱ� �� UI �ݿ�
        view.SettingChip(data.PlusSum);
        view.SettingMultiply(data.MultiplySum);
    }

    // �������̽� ����
    public void SetMultiply(int value = 0) // ���
    {
        data.MultiplySum = value;
        view.SettingMultiply(data.MultiplySum);
    }

    public void SetPlus(int value = 0) // Ĩ
    {
       data.PlusSum = value;
       view.SettingChip(data.PlusSum);
    }
    
    public void Reset()
    {
        data.ResetCounts();

        view.SettingChip(data.PlusSum);
        view.SettingMultiply(data.MultiplySum);
    }
    
    public void AddMultiply(int value) // ���
    {
        data.MultiplySum += value;
        view.SettingMultiply(data.MultiplySum);
    }

    public void AddPlus(int value) // Ĩ
    {
        data.PlusSum += value;
        view.SettingChip(data.PlusSum);
    }

    public int GetChip() => data.PlusSum;
    public int GetMultiply() => data.MultiplySum;





}
