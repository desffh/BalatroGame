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


// Card�� �� ��ũ��Ʈ
public class Card : CardComponent
{
    private IObjectPool<Card> ManagedPool;

    // |------------------------------

    private Transform cardPrefabs;
     
    [SerializeField] SpriteRenderer card; // �ո�
    [SerializeField] SpriteRenderer cardBack; // �޸��� ����

    [SerializeField] Sprite cardback; // �޸� �̹���
    [SerializeField] Sprite cardFront;

    [SerializeField] SpriteRenderer spriteCards;
    [SerializeField] SpriteRenderer spriteCards2;

    [SerializeField] BoxCollider2D Collider2D;
    
    public string spriteSheetName;
    public string spriteNameToLoad;

    public PRS originPRS; // ī�� ������ġ�� ���� PRS Ŭ����

    // ��� �ؽ��ĸ� �� �־�� �迭
    Sprite[] sprites;

    // SetUp �Լ��� ���� ī���� ����ü ������ �޾ƿͼ� ����
    public ItemData itemdata;
    
    // ī�尡 ���ȴ��� Ȯ��
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
        // ī�� ��������Ʈ �̸� �޾ƿ�
        string spriteName = item.front;

        // ��� ��������Ʈ �迭�� �� �ֱ�
        sprites = Resources.LoadAll<Sprite>(spriteSheetName);

        if (sprites.Length > 0)
        {
            Sprite selctedSprite = sprites.FirstOrDefault(s => s.name == spriteName);

            if (selctedSprite != null)
            {
                spriteCards.sprite = selctedSprite;  // ������ ��������Ʈ�� ī�忡 ����
            }
        }

        if(cardback != null)
        {
            spriteCards2 = transform.GetChild(1).GetComponent<SpriteRenderer>();
            cardBack.sprite = cardback;
        }
    }

    // ī�� ���� �ִϸ��̼�
    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0, System.Action onComplete = null)
    {
        if (useDotween)
        {
            transform.DOMove(prs.pos, dotweenTime).SetDelay(0.2f);
            transform.DORotateQuaternion(prs.rot, dotweenTime).SetDelay(0.2f);
            transform.DOScale(prs.scale, dotweenTime).SetDelay(0.2f)
                .OnComplete(() => {
                    onComplete?.Invoke(); // ������ �ִϸ��̼��� ������ �ݹ� ����
                });
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
            onComplete?.Invoke(); // ��Ʈ�� �� ���� ���� �ٷ� ȣ��
        }
    }


    // |--------------------------------------------------------------


    // ���콺�� Ŭ���ϸ� CardIDdata ����Ʈ�� ī�� �ֱ� (�ִ�5��)
    public void OnMouseDown()
    {
        SoundManager.Instance.PlayCardClick();
        OnCardClicked();
    }

    public override void OnCardClicked()
    {
        // ����Ʈ�� �� á�ٸ�
        if (checkCard && PokerManager.Instance.cardData.SelectCards.Count <= 5)
        {
            // �� ��ũ��Ʈ�� �޸� Card�� �Ű������� ����
            //PokerManager.Instance.RemoveSuitIDdata(this);
            PokerManager.Instance.DeselectCard(this);

            AnimationManager.Instance.CardAnime(cardPrefabs);

            checkCard = false;
        }
        // ����Ʈ�� �� á�ٸ�
        else if(PokerManager.Instance.cardData.SelectCards.Count < 5)
        {
            //PokerManager.Instance.SaveSuitIDdata(this);
            
            PokerManager.Instance.SelectCard(this);

            AnimationManager.Instance.ReCardAnime(cardPrefabs);

            checkCard = true;
        }
        else
        {
            AnimationManager.Instance.NoCardAnime(cardPrefabs);
        }           
    }

    // |--------------------------------------------------------------

    // ��ġ�� ī�� �ݶ��̴� Ȱ��ȭ
    public override void OnCollider()
    {
        Collider2D.enabled = true;     
    }
    
    // ��ġ�� ī�� �ݶ��̴� ��Ȱ��ȭ
    public override void OffCollider()
    {

        Collider2D.enabled = false;
        
    }


    // |-----------------------------

    // �Ű������� ���� Card Ÿ���� pool�� ����
    public void SetManagedPool(IObjectPool<Card> pool)
    {
        ManagedPool = pool;
    }


}