using TMPro;
using UnityEngine;

public class MoneyText : NumTextUpdater
{
    [SerializeField] TextMeshProUGUI moneyText;

    [SerializeField] TextMeshProUGUI cashOutText;

    // |-----------------------------------------

    // 전체 머니 텍스트 업데이트
    public override void UpdateText(int text)
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
