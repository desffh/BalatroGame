using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NormalJokerPopup : MonoBehaviour, IPopupText
{
    [SerializeField] GameObject popup;

    [SerializeField] public TextMeshProUGUI jokerNameText;
    [SerializeField] public TextMeshProUGUI jokerInfoText;

    [SerializeField] public TextMeshProUGUI jokerMoneyText;

    public string type => "Normal";

    private void Awake()
    {

    }

    public void Initialize(string name, string info, int multiple, int cost)
    {
        if (jokerNameText == null) jokerNameText = FindTMPByName("NameText");
        if (jokerInfoText == null) jokerInfoText = FindTMPByName("InfoText");
        if (jokerMoneyText == null) jokerMoneyText = FindTMPByName("MoneyText");

        jokerNameText.text = $"<color=#0000FF>{name}</color>";

        jokerInfoText.text = $"<color=#0000FF>+{multiple}</color>" + " ���";

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
