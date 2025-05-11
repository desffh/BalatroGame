using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoDeleteJokerPopup : MonoBehaviour, IPopupText
{
    [SerializeField] GameObject popup;

    [SerializeField] public TextMeshProUGUI jokerNameText;
    [SerializeField] public TextMeshProUGUI jokerInfoText;

    [SerializeField] public TextMeshProUGUI jokerMoneyText;

    public string type => "NoDelete";

    public void Initialize(string name, string info, int multiple, int cost)
    {

        if (jokerNameText == null) jokerNameText = FindTMPByName("NameText");
        if (jokerInfoText == null) jokerInfoText = FindTMPByName("InfoText");
        if (jokerMoneyText == null) jokerMoneyText = FindTMPByName("MoneyText");

        jokerNameText.text = $"<color=#0000FF>{name}</color>";

        jokerInfoText.fontSize = 26;

        if (name == "깃발")
        {
            jokerInfoText.text = "남은 버리기 당 " +
                $"<color=#0000FF>+{multiple}</color>" + " 칩";
        }
        else if (name == "신비의 정점")
        {
            jokerInfoText.text = $"버리기가 <color=#008000>{0}</color>번 남았을 경우 " +
                $"<color=#FF0000>+{multiple}</color>" + " 배수";
        }


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
