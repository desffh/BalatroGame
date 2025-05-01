using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 게임 오버 팝업에 들어 갈 모든 정보 갱신
public class GameOverText : MonoBehaviour
{
    // 게임 종료 팝업에 들어 갈 텍스트 & 블라인드 이미지 
    [SerializeField] TextMeshProUGUI besthandText;

    [SerializeField] TextMeshProUGUI numhandText;

    [SerializeField] TextMeshProUGUI deleteCardText;

    [SerializeField] TextMeshProUGUI buyCardText;

    [SerializeField] TextMeshProUGUI entyText;

    [SerializeField] TextMeshProUGUI roundText;

    [SerializeField] TextMeshProUGUI blindText;

    [SerializeField] Image blindImage;

    // 엔티 & 라운드 & 블라인드 이름, 이미지|----------------

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
