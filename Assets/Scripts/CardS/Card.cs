using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using UnityEngine.U2D;
using System.Linq;
using Newtonsoft.Json.Bson;
using UnityEngine.Events;
using System;
using TMPro;

using UnityEngine.Pool;
using UnityEngine.EventSystems;


// Card에 들어갈 스크립트
public class Card : CardComponent
{
    // SetUp 함수로 뽑은 카드의 구조체 정보를 받아와서 저장
    public ItemData itemdata;

    // 고유 번호로 쓰기 쉽게 속성 제공
    public int ID => itemdata.inherenceID; 

    // |------------------------------

    private Transform cardPrefabs;

    // 카드 앞면 이미지 
    [SerializeField] Sprite cardFront;

    [SerializeField] SpriteRenderer spriteCards;

    [SerializeField] BoxCollider2D Collider2D;

    // 카드 디버프 이미지
    [SerializeField] GameObject debuffImage;

    // 카드 타로 이미지
    [SerializeField] GameObject taroImage;

    public string spriteNameToLoad;

    public PRS originPRS; // 카드 원본위치를 담은 PRS 클래스

    // 카드가 눌렸는지 확인
    [SerializeField] public bool checkCard = false;

    // |------------------------------

    public bool isInUse = false;

    [SerializeField] CardPopup cardPopup;

    public int bonusChipByTaro = 0; // 타로로 인해 추가된 칩 수

    // 실제 점수에 사용될 ID (디버프나 타로 포함)
    public int EffectiveId => itemdata.id + bonusChipByTaro;


    // |------------------------------

    // 오브젝트 풀링
    //
    // Card.ManagedPool : 각 카드 객체가 자기 풀을 알아야 자가 반납 가능함 
    //                    즉, 이 카드 객체가 어느 풀에서 왔는지 기억하는 용도
    // |------------------------------

    private IObjectPool<Card> ManagedPool;

    // 매개변수로 받은 Card 타입의 pool을 ManagedPool에 저장
    public void SetManagedPool(IObjectPool<Card> pool)
    {
        ManagedPool = pool;
    }

    // |------------------------------

    private void Awake()
    {
        Collider2D = GetComponent<BoxCollider2D>();

        cardPrefabs = GetComponent<Transform>();

    }

    private void Start()
    {
        Collider2D.enabled = true;

        cardPopup.gameObject.SetActive(false);

        // 카드 팝업 정보 세팅
        PopupText();
    }

    public void PopupText()
    {
        cardPopup.Initialize(itemdata.name, EffectiveId);
    }

    // |------------------------------


    public bool isAnimating = false;


    // 마우스가 카드에 닿았을 때 -> 효과음, 팝업 띄우기
    private void OnMouseEnter()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-EnterCard");
        cardPopup.MouseEnter();
        AnimationManager.Instance.OnEnterCard(gameObject);
    }

    // 마우스가 카드를 떠났을 때 -> 팝업 닫기
    private void OnMouseExit()
    {
        cardPopup.MouseExit();
        AnimationManager.Instance.OnExitCard(gameObject);
    }

    // |------------------------------

    // 카드 정보, 스프라이트 셋팅 (버퍼에서 카드를 뽑을 때 마다)
    public void Setup(ItemData item)
    {
        bonusChipByTaro = 0;

        // 구조체에 itemdata 저장
        this.itemdata = new ItemData()
        { 
            name = item.name,
            id = item.id,
            suit = item.suit,
            front = item.front,
            inherenceID = item.inherenceID,
        };

        // 타로 효과는 따로 저장
        bonusChipByTaro = 0; // 항상 초기화

        // 스프라이트 이름 가져오기
        string spriteName = item.front;

        // 카드 앞면을 렌더링할 SpriteRenderer
        spriteCards = transform.GetChild(0).GetComponent<SpriteRenderer>();

        // CardSprites에서 스프라이트 꺼내오기
        Sprite selectedSprite = CardSprites.Instance.Get(spriteName);

        if (selectedSprite != null)
        {
            spriteCards.sprite = selectedSprite;
        }
        else
        {
            Debug.LogWarning($"카드 스프라이트 찾을 수 없음");
        }
    }
    // |------------------------------

    public Vector3 basePosition { get; private set; }
    public Vector3 baseScale { get; private set; }

    public void SaveInitialTransform()
    {
        basePosition = transform.position;
        baseScale = transform.localScale;
    }

    // |------------------------------

    private Tween moveTween;


    // 카드 정렬 애니메이션
    public void MoveTransform(PRS prs, bool useAnimation, float duration, System.Action onComplete = null)
    {

        if (moveTween != null && moveTween.IsActive())
        {
            moveTween.Kill();
            moveTween = null;
        }

        if (this == null || transform == null) return; // 예외 방지


        if (useAnimation)
        {
            if (TryGetComponent<Collider2D>(out var collider))
                collider.enabled = false;

            // 기존 Tween이 있다면 안전하게 제거
            if (moveTween != null && moveTween.IsActive())
            {
                moveTween.Kill();
                moveTween = null;
            }

            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOMove(prs.pos, duration).SetEase(Ease.OutCubic));
            seq.Join(transform.DORotateQuaternion(prs.rot, duration).SetEase(Ease.OutCubic));
            seq.Join(transform.DOScale(prs.scale, duration).SetEase(Ease.OutCubic));

            seq.OnComplete(() =>
            {
                //collider.enabled = true; // 이거 왜 있죠>??????
                onComplete?.Invoke();
            });

            moveTween = seq; // Sequence도 Tween이라 저장 가능
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
            onComplete?.Invoke();
        }
    }

    // |------------------------------

    // 마우스로 클릭하면 CardIDdata 리스트에 카드 넣기 (최대5개) & 사운드
    public void OnMouseDown()
    {
        // 현재 마우스 포인터가 UI 요소 위에 있을 경우, 아래 코드 실행을 건너뜁니다.
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return; // 클릭 처리하지 않음
        }

        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ClickCard");

        OnCardClicked();
    }


    
    public override void OnCardClicked()
    {
        // 안전하게 Tween 상태 검사
        if (moveTween != null && moveTween.IsActive() && moveTween.IsPlaying())
            return;

        // 카드 제거
        if (checkCard && PokerManager.Instance.cardData.SelectCards.Count <= 5)
        {
            // 이 스크립트가 달린 Card를 매개변수로 전달
            PokerManager.Instance.DeselectCard(this);

            AnimationManager.Instance.CardAnime(cardPrefabs);

            checkCard = false;
        }
        // 카드 추가
        else if (PokerManager.Instance.cardData.SelectCards.Count < 5)
        {
            PokerManager.Instance.SelectCard(this);

            AnimationManager.Instance.ReCardAnime(cardPrefabs);

            checkCard = true;
        }
        else // 카드 꽉 참 
        {
            AnimationManager.Instance.NoCardAnime(cardPrefabs);

        }

    }

    // |------------------------------

    // 카드 콜라이더 활성화
    public override void OnCollider()
    {
        Collider2D.enabled = true;
    }

    // 카드 콜라이더 비활성화
    public override void OffCollider()
    {

        Collider2D.enabled = false;

    }

    // |-----------------------------


    // 트윈 방지를 위한 DOKill
    public void KillTween()
    {
        if (moveTween != null && moveTween.IsActive())
        {
            moveTween.Kill();
            moveTween = null;
        }
    }

    // |-----------------------------

    // 디버프 이미지 활성화 

    public void OndebuffImage()
    {
        debuffImage.SetActive(true);
    }

    // 디버프 이미지 비활성화
    public void OffdebuffImage()
    {
        debuffImage.SetActive(false);
    }

    // 타로 이미지 활성화
    public void OnTaroImage()
    {
        if (taroImage != null)
            taroImage.SetActive(true);
    }

    // 타로 이미지 비활성화
    public void OffTaroImage()
    {
        if (taroImage != null)
            taroImage.SetActive(false);
    }
}