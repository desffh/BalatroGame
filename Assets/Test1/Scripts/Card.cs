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


// Card에 들어갈 스크립트
public class Card : CardComponent
{
    private IObjectPool<Card> ManagedPool;

    // |------------------------------

    private Transform cardPrefabs;
     
    [SerializeField] SpriteRenderer card; // 앞면
    [SerializeField] SpriteRenderer cardBack; // 뒷면은 통일

    [SerializeField] Sprite cardback; // 뒷면 이미지
    [SerializeField] Sprite cardFront;

    [SerializeField] SpriteRenderer spriteCards;
    [SerializeField] SpriteRenderer spriteCards2;

    [SerializeField] BoxCollider2D Collider2D;
    
    public string spriteSheetName;
    public string spriteNameToLoad;

    public PRS originPRS; // 카드 원본위치를 담은 PRS 클래스

    // 모든 텍스쳐를 다 넣어둘 배열
    Sprite[] sprites;

    // SetUp 함수로 뽑은 카드의 구조체 정보를 받아와서 저장
    public ItemData itemdata;
    
    // 카드가 눌렸는지 확인
    [SerializeField] public bool checkCard = false;

    // |-----------------------------------------------

    [SerializeField] CardPopup cardPopup;

    private void Awake()
    {
        Collider2D = GetComponent<BoxCollider2D>();

        cardPrefabs = GetComponent<Transform>();

    }

    private void Start()
    {
        Collider2D.enabled = true;

        cardPopup.gameObject.SetActive(false);

        PopupText();
    }

    public void PopupText()
    {
        cardPopup.Initialize(itemdata.name, itemdata.id);
    }

    private void OnMouseEnter()
    {
        SoundManager.Instance.PlayCardEnter();
        cardPopup.MouseEnter();
    }

    private void OnMouseExit()
    {
        cardPopup.MouseExit();
    }
    public void Setup(ItemData item)
    {
        spriteCards = transform.GetChild(0).GetComponent<SpriteRenderer>();

        this.itemdata = item;
        // 카드 스프라이트 이름 받아옴
        string spriteName = item.front;

        // 모든 스프라이트 배열에 다 넣기
        sprites = Resources.LoadAll<Sprite>(spriteSheetName);

        if (sprites.Length > 0)
        {
            Sprite selctedSprite = sprites.FirstOrDefault(s => s.name == spriteName);

            if (selctedSprite != null)
            {
                spriteCards.sprite = selctedSprite;  // 선택한 스프라이트를 카드에 적용
            }
        }

        if(cardback != null)
        {
            spriteCards2 = transform.GetChild(1).GetComponent<SpriteRenderer>();
            cardBack.sprite = cardback;
        }
    }

    // 카드 정렬 애니메이션
    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0, System.Action onComplete = null)
    {
        if (useDotween)
        {
            transform.DOMove(prs.pos, dotweenTime).SetDelay(0.2f);
            transform.DORotateQuaternion(prs.rot, dotweenTime).SetDelay(0.2f);
            transform.DOScale(prs.scale, dotweenTime).SetDelay(0.2f)
                .OnComplete(() => {
                    onComplete?.Invoke(); // 스케일 애니메이션이 끝나고 콜백 실행
                });
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
            onComplete?.Invoke(); // 도트윈 안 썼을 때도 바로 호출
        }
    }


    // |--------------------------------------------------------------


    // 마우스로 클릭하면 CardIDdata 리스트에 카드 넣기 (최대5개)
    public void OnMouseDown()
    {
        SoundManager.Instance.PlayCardClick();
        OnMouse();
    }

    public override void OnMouse()
    {
        // 리스트가 꽉 찼다면
        if (checkCard && PokerManager.Instance.CardIDdata.Count <= 5)
        {
            // 이 스크립트가 달린 Card를 매개변수로 전달
            PokerManager.Instance.RemoveSuitIDdata(this);

            AnimationManager.Instance.CardAnime(cardPrefabs);

            checkCard = false;
        }
        // 리스트가 덜 찼다면
        else if(PokerManager.Instance.CardIDdata.Count < 5)
        {
            PokerManager.Instance.SaveSuitIDdata(this);
            
            AnimationManager.Instance.ReCardAnime(cardPrefabs);

            checkCard = true;
        }
        else
        {
            AnimationManager.Instance.NoCardAnime(cardPrefabs);
        }   
             
        // 족보 확인
        if (PokerManager.Instance.CardIDdata.Count >= 0)
        {
            //PokerManager.Instance.Hand();
            //PokerManager.Instance.getHandType();
        }
        
    }

    // |--------------------------------------------------------------

    // 배치된 카드 콜라이더 비활성화
    public override void OffCollider()
    {

        Collider2D.enabled = false;
        
    }

    // 배치된 카드 콜라이더 활성화
    // -> 나중에 본인 카드만 끄도록 바꾸자 
    public override void OnCollider()
    {
        Collider2D.enabled = true;     
    }

    // |-----------------------------

    // 매개변수로 받은 Card 타입의 pool을 저장
    public void SetManagedPool(IObjectPool<Card> pool)
    {
        ManagedPool = pool;
    }


}