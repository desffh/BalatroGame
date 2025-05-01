using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ���� ���� �˾��� ��� �� ��� ���� ����
public class GameOverText : MonoBehaviour
{
    // ���� ���� �˾��� ��� �� �ؽ�Ʈ & ����ε� �̹��� 
    [SerializeField] TextMeshProUGUI besthandText;

    [SerializeField] TextMeshProUGUI numhandText;

    [SerializeField] TextMeshProUGUI deleteCardText;

    [SerializeField] TextMeshProUGUI buyCardText;

    [SerializeField] TextMeshProUGUI entyText;

    [SerializeField] TextMeshProUGUI roundText;

    [SerializeField] TextMeshProUGUI blindText;

    [SerializeField] Image blindImage;

    // ��Ƽ & ���� & ����ε� �̸�, �̹���|----------------

    public void EntyUpdate(int enty)
    {
        entyText.text = enty.ToString();
    }
    public void RoundUpdate(int round)
    {
        roundText.text = round.ToString();
    }

    public void BlindUpdate(string blind, Sprite blindimage)
    {
        blindText.text = blind.ToString();
        blindImage.sprite = blindimage;
    }

    public void BestHandUpdate(int besthand)
    {
        besthandText.text = besthand.ToString();
    }
}
