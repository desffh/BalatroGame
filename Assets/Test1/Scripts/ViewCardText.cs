using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 뷰 카드 팝업 텍스트 설정 담당
public class ViewCardText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;

    [SerializeField] TextMeshProUGUI infoText;

    public void TextUpdate(string name, int id)
    {
        string idColored = $"<color=#0000FF>+{id}칩</color>";

        nameText.text = name;
        infoText.text = idColored;
    }
}
