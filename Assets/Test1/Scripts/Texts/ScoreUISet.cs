using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// TotalUIPanel > ScoreSet ���ο� �ִ� ��ũ��Ʈ -> ���߿� ��ġ ����

// ��Ƽ ScoreUISet, ClearPanel �� ���Ǳ� ����

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

        // ĵ������ ������ ����
        Canvas.ForceUpdateCanvases();
    }

} 
