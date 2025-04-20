using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorJokerPopup : MonoBehaviour, IPopupText
{
    [SerializeField] GameObject popup;

    [SerializeField] public TextMeshProUGUI jokerNameText;
    [SerializeField] public TextMeshProUGUI jokerInfoText;

    public void Initialize(string name, string info, int multiple)
    {      
        jokerNameText.text = $"<color=#0000FF>{name}</color>";
        jokerInfoText.text = $"<color=#0000FF>{info}</color>" + "를 사용하여 득점 시  +" +
            $"<color=#0000FF>{multiple}</color>" + " 배수";
    }

}
