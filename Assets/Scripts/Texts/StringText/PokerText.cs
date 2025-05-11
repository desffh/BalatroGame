using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class PokerText : StringTextUpdater
{
    [SerializeField] TextMeshProUGUI stringText;

    private void Awake()
    {
        stringText = GetComponent<TextMeshProUGUI>();
    }


    public override void UpdateText(string text = "")
    {
        stringText.text = text;
        AnimationManager.Instance.CaltransformAnime(stringText);
    }
}
