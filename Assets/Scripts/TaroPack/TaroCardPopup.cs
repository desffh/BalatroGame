using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaroCardPopup : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI nameText;
    [SerializeField] public TextMeshProUGUI infoText;


    // 행성 카드 팝업 데이터 세팅
    public void Initialize(string name, string suit, int random, int chip)
    {
        nameText.text = $"<color=#FF0000>{name}</color>"; // 빨강색

        infoText.text = $"<color=#008000>{suit} {random}</color>의 칩이 증가합니다. " +
            $"<color=#0000FF>칩+{chip}</color> ";
    }
}
