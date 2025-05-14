using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


// ����ε� ���� ��ư�� ���� �� ����Ǵ� �Լ���

public class StageButton : MonoBehaviour
{
    [SerializeField] private Button[] blindButtons; // �� 3��

    [SerializeField] private TextMeshProUGUI [] scoreText; // ��ǥ���� �ؽ�Ʈ

    [SerializeField] private StageManager stageManager;

    [SerializeField] private ScoreUISet scoreUiSet;

    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private TextMeshProUGUI shoptext;

    [SerializeField] GameObject Entycanvas; // ���â ĵ����

    [SerializeField] private Canvas stagecanvas;

    [SerializeField] private GameObject ShopPanel;

    [SerializeField] Button nextEnty; // ���� ��Ƽ

    // ������ ���� �� ȣ��Ǵ� �븮��
    public static event System.Action OnShopCloseRequest;

    // �� ���尡 ���۵� �� ���� ȣ��Ǵ� �븮��
    //
    // -> ���帶�� �ʱ�ȭ�� �ʿ�� �ϴ� ��Ŀ�� �Լ� �ߵ�
    public static event System.Action OnRoundStart;

    // ----------- Ȱ��ȭ �� �г� ---------------------------

    [SerializeField] GameObject[] stagepanels;

    public BlindRound blind;


    [SerializeField] private Image bossBlindImage;

    [SerializeField] private TextMeshProUGUI bossInfoText;

    //---



    private void Start()
    {
        OnEntyCanvas();
        Typing(text);

        // ����ε� �гο� ù ���ھ� ����
        ScoreTextSetting(); 
        PanelSetting();

        // |---

        // ���� ����ε� �̹��� ����
       
        BossImageSetting();


        // ���� ����ε� �ؽ�Ʈ ����

        BossInfoSetting();
    }

    public void OnBlindClick0()
    {
        // ��Ƽ ����
        StageManager.Instance.EntyAdd();
        OnBlindClick(0);

    }
    public void OnBlindClick1() => OnBlindClick(1);
    
    
    // ���� ����ε�
    public void OnBlindClick2()
    {
        OnBlindClick(2);

    }

    public void OnBlindClick(int blindIndex)
    {
        // ���� ����
        StageManager.Instance.RoundAdd();

        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        blind = stageManager.GetBlindAtCurrentEnty(blindIndex);


        Debug.Log(blind.blindColor);

        scoreUiSet.EntyTextSetting(blind.blindName, blind.score, new string('$', blind.money));
        scoreUiSet.EntyImageSetting(blind.blindImage, blind.blindColor);

        if (blind.isBoss)
        {
            ServiceLocator.Get<IAudioService>().PlayBGM("BossTheme-BossEnty", true);
            
        }

        Panels(blindIndex);

        GameManager.Instance.PlayOn();
        stagecanvas.gameObject.SetActive(false);

        CardManager.Instance.SetupNextStage();

        // �� ���尡 ���۵� �� ���� ȣ��Ǵ� �븮��
        //
        // -> ���帶�� �ʱ�ȭ�� �ʿ�� �ϴ� ��Ŀ�� �Լ� �ߵ�
        OnRoundStart?.Invoke();
    }

    // ������ �г� Ȱ��ȭ
    public void Panels(int stage)
    {
        stagepanels[stage].SetActive(true);

        stagepanels[(stage + 1) % 3].SetActive(false);
    }

    // ������ �г� �ʱ� ����
    public void PanelSetting()
    {
        for (int i = 0; i < stagepanels.Length; i++)
        {
            stagepanels[i].SetActive(true);
        }

        stagepanels[0].SetActive(false);
    }

    public void OnEntyCanvas()
    {
        stagecanvas.gameObject.SetActive(true);
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-Enty");

        Typing(text);

        // ��Ƽ Ȱ��ȭ �� �� ���� �̹��� ����
        BossImageSetting();


        // ���� ����ε� �ؽ�Ʈ ����

        BossInfoSetting();
    }

    private void Typing(TextMeshProUGUI text)
    {
        AnimationManager.Instance.TMProText(text, 0.8f);

        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-BlindText");

    }

    // -------------------- ĳ�� �ƿ� ��ư -----------------------

    public void NextEntyOn() // ĳ�� �ƿ� ��ư�� ������ ���� ������
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        ServiceLocator.Get<IAudioService>().PlayBGM("ShopTheme-Shop", true);

        Entycanvas.gameObject.SetActive(false);
        ShopPanel.gameObject.SetActive(true);

        Typing(shoptext);

        // ��ǥ ���� �ٽ� ����
        stageManager.AdvanceBlind();
    }

    public void NextEntyOff()
    {
        nextEnty.interactable = false;
    }


    // ������ ���� ���� ��ư
    public void NextEnty()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        // ���� ���尡 ���� ����ε��� 
        if (stageManager.GetRound() % 3 == 0)
        {
            ServiceLocator.Get<IAudioService>().PlayBGM("MainTheme-Title", true);
            
            // ���� ��Ƽ �� �� 
            ScoreTextSetting();


        }


        OnShopCloseRequest?.Invoke();

        ShopPanel.gameObject.SetActive(false);

        // |---



        // ����ε� ĵ���� ���� 
        OnEntyCanvas();
    }

    // ��Ƽ�� ���� ����ε� �̹��� ����
    public void BossImageSetting()
    {
        BlindRound blindInfo =  stageManager.BossBlindInfo(2);

        bossBlindImage.sprite = blindInfo.blindImage;
    }

    public void BossInfoSetting()
    {
        BlindRound blindInfo = stageManager.BossBlindInfo(2);

        bossInfoText.text = blindInfo.blindInfo;
    }


    // ���� ���� �˾� ������Ʈ|-------------------------------------

    [SerializeField] GameOverText gameOverText;

    public void GameOverText()
    {
        gameOverText.EntyUpdate(StageManager.Instance.GetEnty());
        gameOverText.RoundUpdate(StageManager.Instance.GetRound());
        gameOverText.BlindUpdate(blind.blindName, blind.blindImage);
    }

    // ����, ��Ƽ Ƚ�� ���� |----------------------------------------------


    public void ScoreTextSetting()
    {
        EntyStage entys = stageManager.Getblind();

        for (int i = 0; i < entys.blindScore.Length; i++)
        {
            scoreText[i].text = entys.blindScore[i].ToString();
        }
    }

    public void ReSettings()
    {
        // ���ھ� ���� �ٽ��ϱ�
        ScoreTextSetting();

        // ������ �ʱ� ���� 
        PanelSetting();

        // ��Ƽ ĵ���� Ȱ��ȭ
        OnEntyCanvas();


    }
}
