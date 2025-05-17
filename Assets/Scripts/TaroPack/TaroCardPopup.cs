using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaroCardPopup : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI nameText;
    [SerializeField] public TextMeshProUGUI infoText;


    // �༺ ī�� �˾� ������ ����
    public void Initialize(string name, string suit, int random, int chip)
    {
        nameText.text = $"<color=#FF0000>{name}</color>"; // ������

        infoText.text = $"<color=#008000>{suit} {random}</color>�� Ĩ�� �����մϴ�. " +
            $"<color=#0000FF>Ĩ+{chip}</color> ";
    }
}
