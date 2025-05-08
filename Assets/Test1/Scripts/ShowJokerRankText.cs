using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 조커 점수 텍스트 설정 담당
public class ShowJokerRankText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI RankText;

    private void Start()
    {
        RankText.gameObject.SetActive(false);
    }


    public void OnSettingRank(int rank)
    {
        RankText.text = "+ " + rank.ToString() + "배수";

        OnRankText();
    }

    public void OnSettingChip(int chip)
    {
        RankText.text = "+ " + chip.ToString() + "칩";

        OnRankText();
    }

    public void OnRankText()
    {
        RankText.gameObject.SetActive(true);
        AnimationManager.Instance.ShowTextAnime(RankText);
    }
}
