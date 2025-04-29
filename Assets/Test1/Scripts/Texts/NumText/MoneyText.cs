using TMPro;
using UnityEngine;

public class MoneyText : NumTextUpdater
{
    [SerializeField] TextMeshProUGUI moneyText;

    [SerializeField] TextMeshProUGUI cashOutText;

    // |-----------------------------------------

    // ��ü �Ӵ� �ؽ�Ʈ ������Ʈ
    public override void UpdateText(int text)
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
