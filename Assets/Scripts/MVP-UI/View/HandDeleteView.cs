using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Hand & Delete의 텍스트 업데이트만 담당
public class HandDeleteView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI handText;
    [SerializeField] private TextMeshProUGUI deleteText;

    public void UpdateHand(int value)
    {
        handText.text = value.ToString();
        AnimationManager.Instance.CaltransformAnime(handText);
    }

    public void UpdateDelete(int value)
    {
        deleteText.text = value.ToString();
        AnimationManager.Instance.CaltransformAnime(deleteText);
    }
}
