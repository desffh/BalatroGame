using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 현재 엔티 / 라운드 뷰 업데이트 담당

public class RoundEntyView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI entyText;
    [SerializeField] private TextMeshProUGUI roundText;

    public void UpdateEnty(int value)
    {
        entyText.text = value.ToString();
        AnimationManager.Instance.CaltransformAnime(entyText);
    }

    public void UpdateRound(int value)
    {
        roundText.text = value.ToString();
        AnimationManager.Instance.CaltransformAnime(roundText);
    }
}
