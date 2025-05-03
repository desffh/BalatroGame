using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class StageButton : MonoBehaviour
{

    [SerializeField] Button button;

    [SerializeField] Button nextEnty;

    [SerializeField] Canvas stagecanvas; // ��Ƽ3�� ������ ĵ����

    [SerializeField] GameObject Entycanvas; // ���â ĵ����

    [SerializeField] GameObject ShopPanel; // ���� ĵ����



    public static event System.Action OnShopCloseRequest;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnClick1()
    {
        SoundManager.Instance.ButtonClick();

        Stage1Click(0);
    }
    public void OnClick2()
    {
        SoundManager.Instance.ButtonClick();

        Stage1Click(1);
    }
    public void OnClick3()
    {      
        SoundManager.Instance.ButtonClick();

        SoundManager.Instance.OnBossStageStart();
        Stage1Click(2);
    }
    public void Stage1Click(int stage)
    {
        if(stage == 0)
        {
            Round.Instance.EntyUp();
            SoundManager.Instance.PlayGameBGM();
        }

        Round.Instance.RoundUp();

        // ���� �������� ��ǥ ���� ����
        Round.Instance.Score(stage);

        // ���� �������� ����ε� �̸� ���� 
        Round.Instance.BlindName = Round.Instance.blindNames[stage];
        
        // ���� �������� �Ӵ� ���� ���� 
        Round.Instance.Money = Round.Instance.moneys[stage];

        // ���� �������� �ؽ�Ʈ ����
        Round.Instance.ScoreTextSetting(stage);

        // ���� �������� ���� & ����ε� �̹��� ����
        Round.Instance.ImageSetting(stage);

        // �������� �÷��� ����
        GameManager.Instance.PlayOn();
        
        // �������� â ��Ȱ��ȭ
        stagecanvas.gameObject.SetActive(false);

        // �ʱ�ȭ
        CardManager.Instance.SetupNextStage();
    }



    // -------------------- ĳ�� �ƿ� ��ư -----------------------

    public void NextEntyOn() // ĳ�� �ƿ� ��ư�� ������ ���� ������
    {
        SoundManager.Instance.ButtonClick();

        Entycanvas.gameObject.SetActive(false);
        ShopPanel.gameObject.SetActive(true);
        SoundManager.Instance.OnShopStart();

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
        SoundManager.Instance.ButtonClick();
        SoundManager.Instance.ResumePreviousBGM();

        OnShopCloseRequest?.Invoke();

        ShopPanel.gameObject.SetActive(false);
        stagecanvas.gameObject.SetActive(true);

    }

    public void OnEntyCanvas()
    {
        stagecanvas.gameObject.SetActive(true);
    }


}
