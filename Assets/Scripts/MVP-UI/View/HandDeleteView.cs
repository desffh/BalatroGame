using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Hand & Delete�� �ؽ�Ʈ ������Ʈ�� ���
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
