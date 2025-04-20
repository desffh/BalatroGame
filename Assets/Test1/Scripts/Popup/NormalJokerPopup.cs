using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NormalJokerPopup : MonoBehaviour, IPopupText
{
    [SerializeField] GameObject popup;

    [SerializeField] public TextMeshProUGUI jokerNameText;
    [SerializeField] public TextMeshProUGUI jokerInfoText;

    public void Initialize(string name, string info, int multiple)
    {
        jokerNameText.text = $"<color=#0000FF>{name}</color>";

        jokerInfoText.text = $"<color=#0000FF>+{multiple}</color>" + " ¹è¼ö";
    }
}
