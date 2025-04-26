using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumText : NumTextUpdater
{
    [SerializeField] TextMeshProUGUI numText;

    private void Awake()
    {
        numText = GetComponent<TextMeshProUGUI>();
    }
    public override void UpdateText(int text)
    {
        numText.text = text.ToString();
        AnimationManager.Instance.CaltransformAnime(numText);
    }
}
