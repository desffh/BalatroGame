using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// ��Ŀ ���� �ؽ�Ʈ ���� ���
public class ShowJokerRankText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI RankText;

    private void Start()
    {
        RankText.gameObject.SetActive(false);
    }


    public void OnSettingRank(int rank)
    {
        RankText.text = "+ " + rank.ToString() + "���";

        OnRankText();
    }

    public void OnSettingChip(int chip)
    {
        RankText.text = "+ " + chip.ToString() + "Ĩ";

        OnRankText();
    }

    public void OnSettingDelete(int delete)
    {
        RankText.text = "+ " + delete.ToString() + "������";
        OnRankText();
    }

    public void OnSettingMultiplyChip(int chip, int multiply)
    {
        RankText.text = "+ " + chip.ToString() + "Ĩ " + "+ " + multiply.ToString() + "���";
    }

    // �ؽ�Ʈ Ȱ��ȭ & �ִϸ��̼�
    public void OnRankText()
    {
        RankText.gameObject.SetActive(true);
        AnimationManager.Instance.ShowTextAnime(RankText);
    }
}
