using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EvenOddJokerPopup : MonoBehaviour, IPopupText
{
    [SerializeField] GameObject popup;

    [SerializeField] public TextMeshProUGUI jokerNameText;
    [SerializeField] public TextMeshProUGUI jokerInfoText;

    [SerializeField] public TextMeshProUGUI jokerMoneyText;

    // �� Ÿ���� ���� ������� -> ���߿� ������
    public string type => "Even";

    public void Initialize(string name, string info, int multiple, int cost)
    {

        if (jokerNameText == null) jokerNameText = FindTMPByName("NameText");
        if (jokerInfoText == null) jokerInfoText = FindTMPByName("InfoText");
        if (jokerMoneyText == null) jokerMoneyText = FindTMPByName("MoneyText");

        jokerNameText.text = $"<color=#0000FF>{name}</color>";

        jokerInfoText.fontSize = 26;

        if (name == "¦�� ��Ŀ")
        {
            jokerInfoText.text = "�÷��� �� ī�忡 <color=#008000>¦��</color> ���� �� " +
                $"<color=#FF0000>+{multiple}</color>" + " ���";
        }
        else if(name == "Ȧ�� ��Ŀ")
        {
            jokerInfoText.text = "�÷��� �� ī�忡 <color=#008000>Ȧ��</color> ���� �� " +
                $"<color=#0000FF>+{multiple}</color>" + " Ĩ";
        }


        jokerMoneyText.text = $"<color=#FFA500>${cost}</color>";

    }

    // �ڽ� �� �̸����� TMP ã�� �޼���
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
