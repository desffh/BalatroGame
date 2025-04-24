using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StageButton : MonoBehaviour
{

    [SerializeField] Button button;

    [SerializeField] Button nextEnty;

    [SerializeField] Canvas stagecanvas; // ��Ƽ3�� ������ ĵ����

    [SerializeField] Canvas Entycanvas; // ���â ĵ����

    [SerializeField] GameObject ShopPanel; // ���� ĵ����

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnClick1()
    {
        Stage1Click(0);
    }
    public void OnClick2()
    {
        Stage1Click(1);
    }
    public void OnClick3()
    {
        Stage1Click(2);
    }
    public void Stage1Click(int stage)
    {
        if(stage == 0)
        {
            Round.Instance.EntyUp();
        }

        Round.Instance.RoundUp();

        // ���� �������� ��ǥ ���� ����
        Round.Instance.Score(stage);

        Round.Instance.ScoreTextSetting();

        // �������� �÷��� ����
        GameManager.Instance.PlayOn();
        
        // �������� â ��Ȱ��ȭ
        stagecanvas.gameObject.SetActive(false);

        // �ʱ�ȭ
        KardManager.Instance.SetupNextStage();
    }


    // -------------------- ĳ�� �ƿ� ��ư -----------------------

    public void NextEntyOn() // ĳ�� �ƿ� ��ư�� ������ ���� ������
    {
        Entycanvas.gameObject.SetActive(false);
        ShopPanel.gameObject.SetActive(true);

        if(Round.Instance.CurrentScores == Round.Instance.stages[2])
        {
            Round.Instance.isStage();
            Round.Instance.ScoreSetting();
        }

    }

    public void NextEntyOff()
    {
        nextEnty.interactable = false;
    }


    // ������ ���� ���� ��ư
    public void NextEnty()
    {
        ShopPanel.gameObject.SetActive(false);
        stagecanvas.gameObject.SetActive(true);
    }
}
