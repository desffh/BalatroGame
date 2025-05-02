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

    //public string spriteSheetName;
    public string spriteNameToLoad;

    public PRS originPRS; // ī�� ������ġ�� ���� PRS Ŭ����

    // ��� �ؽ��ĸ� �� �־�� �迭
    //[SerializeField] Sprite[] sprites;

    //[SerializeField] string spriteSheetName;

    // SetUp �Լ��� ���� ī���� ����ü ������ �޾ƿͼ� ����
    public ItemData itemdata;

    public int ID => itemdata.id; // ���� ��ȣ�� ���� ���� �Ӽ� ����

    // ī�尡 ���ȴ��� Ȯ��
    [SerializeField] public bool checkCard = false;

    // |-----------------------------------------------

    public bool isInUse = false;



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
        this.itemdata = item;

        // ��������Ʈ �̸� ��������
        string spriteName = item.front;

        // ī�� �ո��� �������� SpriteRenderer
        spriteCards = transform.GetChild(0).GetComponent<SpriteRenderer>();

        // CardSprites���� ��������Ʈ ��������
        Sprite selectedSprite = CardSprites.Instance.Get(spriteName);

        if (selectedSprite != null)
        {
            spriteCards.sprite = selectedSprite;
        }
        else
        {
            Debug.LogWarning($"[Card] ��������Ʈ '{spriteName}'��(��) ã�� �� �����ϴ�.");
        }

        // ī�� �޸� ���� (�ɼ�)
        if (cardback != null)
        {
            spriteCards2 = transform.GetChild(1).GetComponent<SpriteRenderer>();
            cardBack.sprite = cardback;
        }

    }

    public Vector3 basePosition { get; private set; }
    public Vector3 baseScale { get; private set; }

    public void SaveInitialTransform()
    {
        basePosition = transform.position;
        baseScale = transform.localScale;
    }


    // ī�� ���� �ִϸ��̼�
    public void MoveTransform(PRS prs, bool useAnimation, float duration, System.Action onComplete = null)
    {
        if (useAnimation)
        {
            if (TryGetComponent<Collider2D>(out var collider))
                collider.enabled = false; // Ŭ�� �浹 ��ü ����

            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOMove(prs.pos, duration).SetEase(Ease.OutCubic));
            seq.Join(transform.DORotateQuaternion(prs.rot, duration).SetEase(Ease.OutCubic));
            seq.Join(transform.DOScale(prs.scale, duration).SetEase(Ease.OutCubic));

            seq.OnComplete(() =>
            {
                if (collider != null)
                    collider.enabled = true; // �ִϸ��̼� ���� �� Ŭ�� �����ϰ�
                onComplete?.Invoke();
            });
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
            onComplete?.Invoke();
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
        // ����Ʈ  �� á�ٸ�
        if (checkCard && PokerManager.Instance.cardData.SelectCards.Count <= 5)
        {
            // �� ��ũ��Ʈ�� �޸� Card�� �Ű������� ����
            //PokerManager.Instance.RemoveSuitIDdata(this);
            PokerManager.Instance.DeselectCard(this);

            AnimationManager.Instance.CardAnime(cardPrefabs);

            checkCard = false;
        }
        // ����Ʈ�� �� á�ٸ�
        else if (PokerManager.Instance.cardData.SelectCards.Count < 5)
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