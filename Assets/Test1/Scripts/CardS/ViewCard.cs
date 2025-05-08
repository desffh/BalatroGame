using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// View용 카드 스크립트 

public class ViewCard : MonoBehaviour
{
    [SerializeField] private Image cardImage; // 카드 이미지 (UI)

    [SerializeField] public int cardID { get; private set; }

    [SerializeField] public string suit;

    [SerializeField] public int rank;

    private CanvasGroup canvasGroup;

    [SerializeField] ViewCardText viewCardText;

    private void OnEnable()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        popupTextSetting();
    }

    // |-----------------------------------------------------

    // 뷰 카드 이미지 & 데이터 설정
    public void Setup(ItemData data)
    {
        cardID = data.inherenceID;

        suit = data.name;

        rank = data.id;

        // 카드 이름에 해당하는 스프라이트를 CardSprites에서 가져오기
        string spriteName = data.front;
        Sprite selectedSprite = CardSprites.Instance.Get(spriteName);

        if (selectedSprite != null)
        {
            cardImage.sprite = selectedSprite;
        }
        else
        {
            Debug.LogWarning($"[ViewCard] 스프라이트 '{spriteName}'를 찾지 못했습니다.");
        }
    }

    // 뷰 카드 보이기
    public void Show()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
                Debug.LogWarning($"[ViewCard] Show() 시 CanvasGroup이 없어 자동 추가됨: {name}");
            }
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    // 뷰 카드 숨기기
    public void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    // |-------------------------------------------------

    // 팝업 텍스트 셋팅
    public void popupTextSetting()
    {
        viewCardText.TextUpdate(suit, rank);
    }

    public void EnterViewCard()
    {
        AnimationManager.Instance.OnEnterViewCard(gameObject);
    }

    public void ExitViewCard()
    {
        AnimationManager.Instance.OnExitViewCard(gameObject);
    }
}
