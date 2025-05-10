using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Money의 텍스트 업데이트 담당

public class MoneyView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyText;

    [SerializeField] TextMeshProUGUI cashOutText;

    // |-----------------------------------------

    // 전체 머니 텍스트 업데이트
    public void UpdateText(int text)
    {
        moneyText.text = "$" + text.ToString();
        AnimationManager.Instance.CaltransformAnime(moneyText);
    }

    // 획득한 머니 텍스트 업데이트 (cashOutText)
    public void UpdatecashOutText(int text)
    {
        cashOutText.text = "$" + text.ToString();
    }
}
