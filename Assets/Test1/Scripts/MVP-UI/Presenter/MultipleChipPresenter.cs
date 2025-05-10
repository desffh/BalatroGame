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

        // 초기 값 UI 반영
        view.SettingChip(data.PlusSum);
        view.SettingMultiply(data.MultiplySum);
    }

    // 인터페이스 정의
    public void SetMultiply(int value = 0) // 배수
    {
        data.MultiplySum = value;
        view.SettingMultiply(data.MultiplySum);
    }

    public void SetPlus(int value = 0) // 칩
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
    
    public void AddMultiply(int value) // 배수
    {
        data.MultiplySum += value;
        view.SettingMultiply(data.MultiplySum);
    }

    public void AddPlus(int value) // 칩
    {
        data.PlusSum += value;
        view.SettingChip(data.PlusSum);
    }

    public int GetChip() => data.PlusSum;
    public int GetMultiply() => data.MultiplySum;





}
