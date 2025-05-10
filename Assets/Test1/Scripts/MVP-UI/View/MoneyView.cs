using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Money�� �ؽ�Ʈ ������Ʈ ���

public class MoneyView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyText;

    [SerializeField] TextMeshProUGUI cashOutText;

    // |-----------------------------------------

    // ��ü �Ӵ� �ؽ�Ʈ ������Ʈ
    public void UpdateText(int text)
    {
        moneyText.text = "$" + text.ToString();
        AnimationManager.Instance.CaltransformAnime(moneyText);
    }

    // ȹ���� �Ӵ� �ؽ�Ʈ ������Ʈ (cashOutText)
    public void UpdatecashOutText(int text)
    {
        cashOutText.text = "$" + text.ToString();
    }
}
