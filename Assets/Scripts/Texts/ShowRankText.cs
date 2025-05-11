using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 숫자 카드 '점수 텍스트' 설정 담당
public class ShowRankText : MonoBehaviour
{
    [SerializeField] TextMeshPro RankText;

    private void Awake()
    {

    }

    private void Start()
    {
        RankText.gameObject.SetActive(false);
    }


    public void OnSettingRank(int rank)
    {
        RankText.text = "+" + rank.ToString();

        OnRankText();
    }
    
    public void OnRankText()
    {
        RankText.gameObject.SetActive(true);
        AnimationManager.Instance.ShowTextAnime(RankText);
    }
}
