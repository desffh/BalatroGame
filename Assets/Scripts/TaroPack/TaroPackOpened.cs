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

    private int count; // 카드 갯수 

    [SerializeField] private GameObject cardStartPosition; //  초기 위치

    [SerializeField] GameObject taroCardPack; // 카드팩

    [SerializeField] Button continueButton; // 건너뛰기 버튼

    private TaroCardPack currentPack;

    private bool isReady = true; // 원래 위치로 다 돌아갔는지 확인

    [SerializeField] private GameObject transparentPanel;

    [SerializeField] private Button selectButton;

    [SerializeField] TextMeshProUGUI upgradeText;

    [SerializeField] private int selectedCount = 0; // 사용한 카드 수


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
        // 이벤트 구독
        cardPack.OnPackOpened += HandlePackOpened;
        Debug.Log("타로 카드 뽑기 이벤트 등록");
    }

    // 구독 해제 -> 상점이 닫힐 때
    public void Unregister(TaroCardPack cardPack)
    {
        cardPack.OnPackOpened -= HandlePackOpened;
        Debug.Log("타로 카드 뽑기 이벤트 해제");
    }


    private void HandlePackOpened(TaroCardPack pack)
    {
        if (!isReady)
        {
            Debug.LogWarning("아직 이전 카드 초기화 중! 팩 열기 중단.");
            return;
        }

        currentPack = pack;

        PackOpened(currentPack); // 창 열기 & 행성 카드 세팅

        Onpanel();
    }


    // |----

    private void PackOpened(TaroCardPack pack)
    {
        selectedCount = 0;

        count = pack.taroPackUI.decCount;

        panel.SetActive(true);

        // 카드팩 이미지 설정
        cardPackImage.sprite = pack.taroPackUI.packImage.sprite;

        // 티로카드 설정
        taroManager.ShuffleBuffer();

        for (int i = 0; i < count; i++)
        {
            // 뽑은 데이터
            var data = taroManager.PopData();

            currentCards.Add(data); // 현재 배치된 카드들의 데이터 추가

            // 카드에 주입
            taroCards[i].SetData(data);


            taroCards[i].GetComponent<Image>().sprite = data.image;

            int random = Random.Range(1, 15);

            taroCards[i].GetComponent<TaroCardPopup>().
                Initialize(data.baseData.name, data.baseData.require, random, 30);

            taroCards[i].OffRaycast(); // 활성화 되기 전에 막기

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


    // 카드를 클릭했을 때
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

    // 구매버튼 누르면 호출 (이벤트 등록)
    public void UpgradeTextSetting(TaroTotalData planetData)
    {
        Debug.Log($"[Upgrade] {planetData.baseData.require} 선택됨. 현재 선택 수: {selectedCount}");

        upgradeText.gameObject.SetActive(true);

        upgradeText.text = $"<color=#FF0000>{planetData.baseData.require}</color> 업그레이드 완료!";

        AnimationManager.Instance.ShowTextAnime(upgradeText);

        selectedCount++;

        if (selectedCount == currentPack.taroPackUI.selectCount)
        {
            Debug.Log("선택 완료! 카드팩 종료");
            EndPlanetPack(); // 팩 이벤트 종료 처리
        }
    }


    // |----

    // 카드팩을 누르면 실행 -> 이벤트 트리거 

    public void CardsMove()
    {
        int startIndex = (5 - count) / 2; // 중앙 정렬

        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-CardPack");


        // 트윈 중복 방지
        for (int i = 0; i < count; i++)
        {
            taroCards[i].transform.DOKill();
            taroCards[i].OffRaycast(); // 애니메이션 중 레이케스트 끄기 
        }

        continueButton.interactable = false; // 애니메이션 중 버튼 상호작용 막기

        Sequence seq = DOTween.Sequence();

        for (int i = 0; i < count; i++)
        {
            int index = i;

            // 순차적 사운드 삽입
            seq.AppendCallback(() =>
            {
                ServiceLocator.Get<IAudioService>().PlaySFX("Sound-SpawnCard");
            });

            seq.Append(
                taroCards[index].transform.DOMove(positions[startIndex + index].position, 0.7f).
                SetEase(Ease.OutExpo) // 감속 효과
            );
        }

        // 애니메이션 다 끝나고 레이케스트 키기
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

        Hide(); // 창 닫기
    }

    // |----


    // 이벤트 트리거 Enter
    public void OnEnterPack()
    {
        AnimationManager.Instance.OnEnterShopCard(taroCardPack);
    }

    // 이벤트 트리거 Exit
    public void OnExitPack()
    {
        AnimationManager.Instance.OnExitShopCard(taroCardPack);
    }

    // |----

    // 건너뛰기 버튼
    public void Hide()
    {
        // 1. 사라지는 애니메이션 실행 후 

        // 2. 데이터 반환
        PushData();

        // 3. 패널 비활성화
        panel.SetActive(false);
    }

    public void PushData()
    {
        isReady = false;

        foreach (var data in currentCards)
        {
            taroManager.PushData(data);
        }

        currentCards.Clear(); // 데이터 리스트 초기화

        foreach (var card in taroCards)
        {
            card.ResetCard(cardStartPosition.transform.position);
        }

        isReady = true;

        Debug.Log("타로카드 데이터 모두 반환할게요");
    }
}
