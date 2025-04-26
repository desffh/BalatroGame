using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
