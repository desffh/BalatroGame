using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyText : NumTextUpdater
{
    [SerializeField] TextMeshProUGUI moneyText;

    private void Awake()
    {

    }

    public override void UpdateText(int text)
    {
        moneyText.text = "$" + text.ToString();
    }
}
