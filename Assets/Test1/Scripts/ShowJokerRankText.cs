using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowJokerRankText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI RankText;

    private void Awake()
    {

    }

    private void Start()
    {
        RankText.gameObject.SetActive(false);
    }


    public void OnSettingRank(int rank)
    {
        RankText.text = "+ " + rank.ToString() + "¹è¼ö";

        OnRankText();
    }

    public void OnRankText()
    {
        RankText.gameObject.SetActive(true);
        AnimationManager.Instance.ShowTextAnime(RankText);
    }
}
