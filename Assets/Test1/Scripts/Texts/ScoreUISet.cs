using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// TotalUIPanel > ScoreSet 내부에 있는 스크립트 -> 나중에 위치 수정

// 엔티 ScoreUISet, ClearPanel 에 사용되기 때문

public class ScoreUISet : MonoBehaviour
{
    [SerializeField] Image blindImage;

    [SerializeField] Image backgroundColor;

    [SerializeField] Image entynameColor;

    // |---------------------------------

    [SerializeField] TextMeshProUGUI entynameText;

    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] TextMeshProUGUI rewardText;

    // |---------------------------------

    [SerializeField] Image clearblindImage;

    [SerializeField] TextMeshProUGUI clearscoreText;

    [SerializeField] TextMeshProUGUI clearrewardText;

    private void Start()
    {
        if(scoreText != null)
        {
            scoreText.text = "";
        }

        if (rewardText != null)
        {
            rewardText.text = "";
        }
    }

    public void EntyTextSetting(string entyname, int score, string reward)
    {
        entynameText.text = entyname;
        scoreText.text = score.ToString();
        rewardText.text = reward;

        clearscoreText.text = score.ToString();
        clearrewardText.text = reward;
    }
    
    public void EntyImageSetting(Sprite blind, Color color)
    {
        blindImage.sprite = blind;
        clearblindImage.sprite = blind;

        backgroundColor.color = color;

        color.g = 0.5f;

        entynameColor.color = color;

        // 캔버스를 강제로 갱신
        Canvas.ForceUpdateCanvases();
    }

} 
