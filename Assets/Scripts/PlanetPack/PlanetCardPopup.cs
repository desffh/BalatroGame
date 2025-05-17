using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlanetCardPopup : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI nameText;
    [SerializeField] public TextMeshProUGUI infoText;


    // �༺ ī�� �˾� ������ ����
    public void Initialize(string name, string require, int chip, int multiple)
    {
        nameText.text = $"<color=#FF0000>{name}</color>"; // ������

        infoText.text = $"<color=#008000>{require}</color> ������ �����մϴ�. " +
            $"<color=#0000FF>Ĩ+{chip}</color> " +
            $"<color=#FF0000>���+{multiple}</color>";
    }
}
