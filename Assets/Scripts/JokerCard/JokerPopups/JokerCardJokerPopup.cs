using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JokerCardJokerPopup : MonoBehaviour, IPopupText
{
    [SerializeField] GameObject popup;

    [SerializeField] public TextMeshProUGUI jokerNameText;
    [SerializeField] public TextMeshProUGUI jokerInfoText;

    [SerializeField] public TextMeshProUGUI jokerMoneyText;

    public string type => "Jokers";


    public void Initialize(string name, string info, int multiple, int cost)
    {
        if (jokerNameText == null) jokerNameText = FindTMPByName("NameText");
        if (jokerInfoText == null) jokerInfoText = FindTMPByName("InfoText");
        if (jokerMoneyText == null) jokerMoneyText = FindTMPByName("MoneyText");

        jokerNameText.text = $"<color=#0000FF>{name}</color>";

        jokerInfoText.fontSize = 26;

        jokerInfoText.text = $"보유중인 조커 카드 마다 " +
            $"<color=#FF0000>+{multiple}</color>" + " 배수";

        jokerMoneyText.text = $"<color=#FFA500>${cost}</color>";
    }

    // 자식 중 이름으로 TMP 찾는 메서드
    private TextMeshProUGUI FindTMPByName(string targetName)
    {
        foreach (var tmp in GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            if (tmp.name == targetName)
                return tmp;
        }

        return null;
    }
}
