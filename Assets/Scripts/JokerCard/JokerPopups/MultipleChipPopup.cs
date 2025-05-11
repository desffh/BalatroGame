using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MultipleChipPopup : MonoBehaviour, IPopupText
{
    [SerializeField] GameObject popup;

    [SerializeField] public TextMeshProUGUI jokerNameText;
    [SerializeField] public TextMeshProUGUI jokerInfoText;

    [SerializeField] public TextMeshProUGUI jokerMoneyText;

    // 이 타입은 별로 상관없음 -> 나중에 없애자
    public string type => "MultipleChip";

    public void Initialize(string name, string info, int multiple, int cost)
    {

        if (jokerNameText == null) jokerNameText = FindTMPByName("NameText");
        if (jokerInfoText == null) jokerInfoText = FindTMPByName("InfoText");
        if (jokerMoneyText == null) jokerMoneyText = FindTMPByName("MoneyText");

        jokerNameText.text = $"<color=#0000FF>{name}</color>";

        jokerInfoText.fontSize = 26;

        if (name == "워키토키")
        {
            jokerInfoText.text = $"플레이 한 카드에 <color=#008000>{10}</color> 또는 <color=#008000>{4}</color> 포함 시 " +
                $"<color=#0000FF>+{10}</color> 칩 <color=#FF0000>+{multiple}</color>" + " 배수";
        }
        else if (name == "학자 조커")
        {
            jokerInfoText.text = $"플레이 한 카드에 <color=#008000>에이스</color> 포함 시 " +
                $"<color=#0000FF>+{20}</color> 칩 <color=#FF0000>+{multiple}</color>" + " 배수";
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
