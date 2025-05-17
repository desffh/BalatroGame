using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaroPackOpened : MonoBehaviour
{
    [SerializeField] private TaroManager taroManager;

    [SerializeField] private Image cardPackImage;

    [SerializeField] private TaroCard[] taroCards;

    private List<TaroTotalData> currentCards = new();

    [SerializeField] private GameObject panel;

    [SerializeField] private Transform[] positions;

    private int count; // ī�� ���� 

    [SerializeField] private GameObject cardStartPosition; //  �ʱ� ��ġ

    [SerializeField] GameObject taroCardPack; // ī����

    [SerializeField] Button continueButton; // �ǳʶٱ� ��ư

    private TaroCardPack currentPack;

    private bool isReady = true; // ���� ��ġ�� �� ���ư����� Ȯ��

    [SerializeField] private GameObject transparentPanel;

    [SerializeField] private Button selectButton;

    [SerializeField] TextMeshProUGUI upgradeText;

    [SerializeField] private int selectedCount = 0; // ����� ī�� ��


    // |----

    private void Start()
    {
        for (int i = 0; i < taroCards.Length; i++)
        {
            taroCards[i].gameObject.SetActive(false);
        }

        upgradeText.gameObject.SetActive(false);
    }

    public void Register(TaroCardPack cardPack)
    {
        // �̺�Ʈ ����
        cardPack.OnPackOpened += HandlePackOpened;
        Debug.Log("Ÿ�� ī�� �̱� �̺�Ʈ ���");
    }

    // ���� ���� -> ������ ���� ��
    public void Unregister(TaroCardPack cardPack)
    {
        cardPack.OnPackOpened -= HandlePackOpened;
        Debug.Log("Ÿ�� ī�� �̱� �̺�Ʈ ����");
    }


    private void HandlePackOpened(TaroCardPack pack)
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


    // |----

    private void PackOpened(TaroCardPack pack)
    {
        selectedCount = 0;

        count = pack.taroPackUI.decCount;

        panel.SetActive(true);

        // ī���� �̹��� ����
        cardPackImage.sprite = pack.taroPackUI.packImage.sprite;

        // Ƽ��ī�� ����
        taroManager.ShuffleBuffer();

        for (int i = 0; i < count; i++)
        {
            // ���� ������
            var data = taroManager.PopData();

            currentCards.Add(data); // ���� ��ġ�� ī����� ������ �߰�

            // ī�忡 ����
            taroCards[i].SetData(data);


            taroCards[i].GetComponent<Image>().sprite = data.image;

            int random = Random.Range(1, 15);

            taroCards[i].GetComponent<TaroCardPopup>().
                Initialize(data.baseData.name, data.baseData.require, random, 30);

            taroCards[i].OffRaycast(); // Ȱ��ȭ �Ǳ� ���� ����

            taroCards[i].gameObject.SetActive(true);

        }

        taroCardPack.GetComponent<Image>().raycastTarget = true;

    }

    public void Onpanel()
    {
        foreach (var card in taroCards)
        {
            card.OnCardSelected -= HandleCardSelected;
            card.OnCardSelected += HandleCardSelected;

            card.OnSelectButtonClick -= UpgradeTextSetting;
            card.OnSelectButtonClick += UpgradeTextSetting;
        }
    }


    // |----


    // ī�带 Ŭ������ ��
    private void HandleCardSelected(TaroCard card)
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


    // |----

    // ���Ź�ư ������ ȣ�� (�̺�Ʈ ���)
    public void UpgradeTextSetting(TaroTotalData planetData)
    {
        Debug.Log($"[Upgrade] {planetData.baseData.require} ���õ�. ���� ���� ��: {selectedCount}");

        upgradeText.gameObject.SetActive(true);

        upgradeText.text = $"<color=#FF0000>{planetData.baseData.require}</color> ���׷��̵� �Ϸ�!";

        AnimationManager.Instance.ShowTextAnime(upgradeText);

        selectedCount++;

        if (selectedCount == currentPack.taroPackUI.selectCount)
        {
            Debug.Log("���� �Ϸ�! ī���� ����");
            EndPlanetPack(); // �� �̺�Ʈ ���� ó��
        }
    }


    // |----

    // ī������ ������ ���� -> �̺�Ʈ Ʈ���� 

    public void CardsMove()
    {
        int startIndex = (5 - count) / 2; // �߾� ����

        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-CardPack");


        // Ʈ�� �ߺ� ����
        for (int i = 0; i < count; i++)
        {
            taroCards[i].transform.DOKill();
            taroCards[i].OffRaycast(); // �ִϸ��̼� �� �����ɽ�Ʈ ���� 
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
                taroCards[index].transform.DOMove(positions[startIndex + index].position, 0.7f).
                SetEase(Ease.OutExpo) // ���� ȿ��
            );
        }

        // �ִϸ��̼� �� ������ �����ɽ�Ʈ Ű��
        seq.OnComplete(() =>
        {
            for (int i = 0; i < count; i++)
            {
                taroCards[i].OnRaycast();
            }
            taroCardPack.GetComponent<Image>().raycastTarget = false;

            continueButton.interactable = true;
        });
    }

    private void EndPlanetPack()
    {
        StartCoroutine(EndPackWithDelay());
    }

    private IEnumerator EndPackWithDelay()
    {
        yield return new WaitForSeconds(1.0f);

        Hide(); // â �ݱ�
    }

    // |----


    // �̺�Ʈ Ʈ���� Enter
    public void OnEnterPack()
    {
        AnimationManager.Instance.OnEnterShopCard(taroCardPack);
    }

    // �̺�Ʈ Ʈ���� Exit
    public void OnExitPack()
    {
        AnimationManager.Instance.OnExitShopCard(taroCardPack);
    }

    // |----

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
            taroManager.PushData(data);
        }

        currentCards.Clear(); // ������ ����Ʈ �ʱ�ȭ

        foreach (var card in taroCards)
        {
            card.ResetCard(cardStartPosition.transform.position);
        }

        isReady = true;

        Debug.Log("Ÿ��ī�� ������ ��� ��ȯ�ҰԿ�");
    }
}
