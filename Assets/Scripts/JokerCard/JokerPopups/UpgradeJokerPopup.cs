using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeJokerPopup : MonoBehaviour, IPopupText
{
    [SerializeField] GameObject popup;

    [SerializeField] public TextMeshProUGUI jokerNameText;
    [SerializeField] public TextMeshProUGUI jokerInfoText;

    [SerializeField] public TextMeshProUGUI jokerMoneyText;

    public string type => "UpgradeChip";


    public void Initialize(string name, string info, int multiple, int cost)
    {
        if (jokerNameText == null) jokerNameText = FindTMPByName("NameText");
        if (jokerInfoText == null) jokerInfoText = FindTMPByName("InfoText");
        if (jokerMoneyText == null) jokerMoneyText = FindTMPByName("MoneyText");

        jokerNameText.text = $"<color=#0000FF>{name}</color>";

        jokerInfoText.fontSize = 26;

        jokerInfoText.text = $"핸드에 스트레이트가 있다면 칩 획득량 " +
            $"<color=#0000FF>+{15}</color>" + $" 개 증가 (기본 칩 <color=#008000>+{multiple}</color>)";

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
