using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUISet : MonoBehaviour
{
    [SerializeField] Image blindImage;

    [SerializeField] Image backgroundColor;

    [SerializeField] Image entynameColor;

    // |---------------------------------

    [SerializeField] TextMeshProUGUI entynameText;

    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] TextMeshProUGUI rewardText;


    private void Start()
    {
        scoreText.text = "";
        rewardText.text = "";
    }

    public void EntyTextSetting(string entyname, int score, string reward)
    {
        entynameText.text = entyname;
        scoreText.text = score.ToString();
        rewardText.text = reward;
    }

    public void EntyImageSetting(Image blind, string color, string namecolor)
    {
        blindImage.sprite = blind.sprite;
    }

} 
