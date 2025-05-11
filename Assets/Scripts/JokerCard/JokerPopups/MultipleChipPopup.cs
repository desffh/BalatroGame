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

    // �� Ÿ���� ���� ������� -> ���߿� ������
    public string type => "MultipleChip";

    public void Initialize(string name, string info, int multiple, int cost)
    {

        if (jokerNameText == null) jokerNameText = FindTMPByName("NameText");
        if (jokerInfoText == null) jokerInfoText = FindTMPByName("InfoText");
        if (jokerMoneyText == null) jokerMoneyText = FindTMPByName("MoneyText");

        jokerNameText.text = $"<color=#0000FF>{name}</color>";

        jokerInfoText.fontSize = 26;

        if (name == "��Ű��Ű")
        {
            jokerInfoText.text = $"�÷��� �� ī�忡 <color=#008000>{10}</color> �Ǵ� <color=#008000>{4}</color> ���� �� " +
                $"<color=#0000FF>+{10}</color> Ĩ <color=#FF0000>+{multiple}</color>" + " ���";
        }
        else if (name == "���� ��Ŀ")
        {
            jokerInfoText.text = $"�÷��� �� ī�忡 <color=#008000>���̽�</color> ���� �� " +
                $"<color=#0000FF>+{20}</color> Ĩ <color=#FF0000>+{multiple}</color>" + " ���";
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
