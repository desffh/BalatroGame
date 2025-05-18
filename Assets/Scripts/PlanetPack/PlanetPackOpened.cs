using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// ī������ ���� ��, â ���� & �༺ī���� ������ ���� & �ִϸ��̼� ����

public class PlanetPackOpened : MonoBehaviour
{
    [SerializeField] private PlanetManager planetManager;

    [SerializeField] private Image cardPackImage;

    [SerializeField] private PlanetCard [] planetCards;

    private List<PlanetTotalData> currentCards = new(); // ��� ��ġ�� ī�� ����Ʈ

    [SerializeField] private GameObject panel; // â
     
    [SerializeField] private Transform [] positions; // ������ ��ġ �迭

    private int count; // ī�� ���� 

    [SerializeField] private GameObject cardStartPosition; // �༺ī�� �ʱ� ��ġ

    [SerializeField] GameObject planetCardPack; // ī����

    [SerializeField] Button continueButton; // �ǳʶٱ� ��ư

    private PlanetCardPack currentPack;

    private bool isReady = true; // ���� ��ġ�� �� ���ư����� Ȯ��

    [SerializeField] private GameObject transparentPanel;

    [SerializeField] private Button selectButton;

    [SerializeField] TextMeshProUGUI upgradeText;

    [SerializeField] private int selectedCount = 0; // ����� ī�� ��


    private void Start()
    {
        for (int i = 0; i < planetCards.Length; i++)
        {
            planetCards[i].gameObject.SetActive(false);
        }

        upgradeText.gameObject.SetActive(false);
    }

    public void Register(PlanetCardPack cardPack)
    {
        // �̺�Ʈ ����
        cardPack.OnPackOpened += HandlePackOpened;
        Debug.Log("�༺ ī�� �̱� �̺�Ʈ ���");
    }

    // ���� ���� -> ������ ���� ��
    public void Unregister(PlanetCardPack cardPack)
    {
        cardPack.OnPackOpened -= HandlePackOpened;
        Debug.Log("�༺ ī�� �̱� �̺�Ʈ ����");
    }


    private void HandlePackOpened(PlanetCardPack pack)
    {
        if (!isReady)
        {
            Debug.LogWarning("���� ���� ī�� �ʱ�ȭ ��! �� ���� �ߴ�.");
            return;
        }

        currentPack = pack;

        PackOpened(currentPack); // â ���� & �༺ ī�� ����

        Onpanel();
    }


    private void PackOpened(PlanetCardPack pack)
    {
        currentCards.Clear();

        selectedCount = 0;

        count = pack.planetPackUI.decCount;

        panel.SetActive(true);

        // ī���� �̹��� ����
        cardPackImage.sprite = pack.planetPackUI.packImage.sprite;

        // �༺ī�� ����
        planetManager.ShuffleBuffer();

        for (int i = 0; i < count; i++)
        {
            // ���� �༺ ������
            var data = planetManager.PopData();
            
            currentCards.Add(data); // ���� ��ġ�� �༺ ī����� ������ �߰�

            // ī�忡 ����
            planetCards[i].SetData(data);


            planetCards[i].GetComponent<Image>().sprite = data.image;

            planetCards[i].GetComponent<PlanetCardPopup>().
                Initialize(data.baseData.name, data.baseData.require, data.baseData.chip, data.baseData.multiple);

            planetCards[i].OffRaycast(); // Ȱ��ȭ �Ǳ� ���� ����

            planetCards[i].gameObject.SetActive(true);

        }

        planetCardPack.GetComponent<Image>().raycastTarget = true;


        Debug.Log("[PlanetPack] ī���� â ���µ�");
    }

    public void Onpanel()
    {
        foreach (var card in planetCards)
        {
            card.OnCardSelected -= HandleCardSelected;
            card.OnCardSelected += HandleCardSelected;

            card.OnSelectButtonClick -= UpgradeTextSetting;
            card.OnSelectButtonClick += UpgradeTextSetting;
        }
    }


    // ī�带 Ŭ������ ��
    private void HandleCardSelected(PlanetCard card)
    {
        transparentPanel.SetActive(true);

        selectButton.transform.position = card.transform.position;

        selectButton.gameObject.SetActive(true);

        selectButton.onClick.RemoveAllListeners();
        
        selectButton.onClick.AddListener(() =>
        {
            card.OnButtonClick();

            OnTransparentPanelClick();

        });
    }
    public void OnTransparentPanelClick()
    {
        transparentPanel.SetActive(false);
        selectButton.gameObject.SetActive(false);
    }

    // ���Ź�ư ������ ȣ�� (�̺�Ʈ ���)
    public void UpgradeTextSetting(PlanetTotalData planetData)
    {
        Debug.Log($"[Upgrade] {planetData.baseData.require} ���õ�. ���� ���� ��: {selectedCount}");

        upgradeText.gameObject.SetActive(true);

        upgradeText.text = $"<color=#FF0000>{planetData.baseData.require}</color> ���׷��̵� �Ϸ�!";

        AnimationManager.Instance.ShowTextAnime(upgradeText);

        selectedCount++;

        if (selectedCount == currentPack.planetPackUI.selectCount)
        {
            Debug.Log("���� �Ϸ�! ī���� ����");
            EndPlanetPack(); // �� �̺�Ʈ ���� ó��
        }
    }


    // Dotween�� Sequence�� ����Ͽ� ���� �̵�

    // ī������ ������ ���� -> �̺�Ʈ Ʈ���� 

    public void CardsMove()
    {
        int startIndex = (5 - count) / 2; // �߾� ����

        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-CardPack");


        // Ʈ�� �ߺ� ����
        for (int i = 0; i < count; i++)
        {
            planetCards[i].transform.DOKill();
            planetCards[i].OffRaycast(); // �ִϸ��̼� �� �����ɽ�Ʈ ���� 
        }

        continueButton.interactable = false; // �ִϸ��̼� �� ��ư ��ȣ�ۿ� ����
        
        Sequence seq = DOTween.Sequence();

        for (int i = 0; i < count; i++)
        {
            int index = i;

            // ������ ���� ����
            seq.AppendCallback(() =>
            {
                ServiceLocator.Get<IAudioService>().PlaySFX("Sound-SpawnCard");
            });

            seq.Append(
                planetCards[index].transform.DOMove(positions[startIndex + index].position, 0.7f).
                SetEase(Ease.OutExpo) // ���� ȿ��
            );
        }

        // �ִϸ��̼� �� ������ �����ɽ�Ʈ Ű��
        seq.OnComplete(() =>
        {
            for (int i = 0; i < count; i++)
            {
                planetCards[i].OnRaycast();
            }
            planetCardPack.GetComponent<Image>().raycastTarget = false;

            continueButton.interactable = true;
        });
    }

    private void EndPlanetPack()
    {
        StartCoroutine(EndPackWithDelay());
    }




    // �̺�Ʈ Ʈ���� Enter
    public void OnEnterPack()
    {
        AnimationManager.Instance.OnEnterShopCard(planetCardPack);
    }

    // �̺�Ʈ Ʈ���� Exit
    public void OnExitPack()
    {
        AnimationManager.Instance.OnExitShopCard(planetCardPack);
    }



    private IEnumerator EndPackWithDelay()
    {
        yield return new WaitForSeconds(1.0f);

        Hide(); // â �ݱ�
    }







    // �ǳʶٱ� ��ư
    public void Hide()
    {
        // 1. ������� �ִϸ��̼� ���� �� 

        // 2. ������ ��ȯ
        PushData();

        // 3. �г� ��Ȱ��ȭ
        panel.SetActive(false); 
    }

    public void PushData()
    {
        isReady = false;

        foreach (var data in currentCards)
        {
            planetManager.PushData(data);
        }

        currentCards.Clear(); // ������ ����Ʈ �ʱ�ȭ

        foreach (var card in planetCards)
        {
            card.ResetCard(cardStartPosition.transform.position);
        }

        isReady = true;

        Debug.Log("�༺ī�� ������ ��� ��ȯ�ҰԿ�");
    }
}
