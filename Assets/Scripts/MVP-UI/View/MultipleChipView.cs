using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Plus & Multiply�� �ؽ�Ʈ ������Ʈ�� ���

public class MultipleChipView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI plusText;
    [SerializeField] private TextMeshProUGUI multiplyText;

    public void SettingChip(int chip = 0)
    {
        plusText.text = chip.ToString();
        AnimationManager.Instance.CaltransformAnime(plusText);
    }

    public void SettingMultiply(int multiple = 0)
    {
        multiplyText.text = multiple.ToString();
        AnimationManager.Instance.CaltransformAnime(multiplyText);
    }
}
