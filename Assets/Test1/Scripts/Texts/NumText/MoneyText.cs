using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyText : NumTextUpdater
{
    [SerializeField] TextMeshProUGUI moneyText;

    [SerializeField] TextMeshProUGUI cashOutText;
    private void Awake()
    {

    }

    public override void UpdateText(int text)
    {
        moneyText.text = "$" + text.ToString();
        AnimationManager.Instance.CaltransformAnime(moneyText);
    }
    public void UpdatecashText(int text)
    {
        cashOutText.text = "$" + text.ToString();
    }
}
