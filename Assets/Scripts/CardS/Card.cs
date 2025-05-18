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


// Card�� �� ��ũ��Ʈ
public class Card : CardComponent
{
    // SetUp �Լ��� ���� ī���� ����ü ������ �޾ƿͼ� ����
    public ItemData itemdata;

    // ���� ��ȣ�� ���� ���� �Ӽ� ����
    public int ID => itemdata.inherenceID; 

    // |------------------------------

    private Transform cardPrefabs;

    // ī�� �ո� �̹��� 
    [SerializeField] Sprite cardFront;

    [SerializeField] SpriteRenderer spriteCards;

    [SerializeField] BoxCollider2D Collider2D;

    // ī�� ����� �̹���
    [SerializeField] GameObject debuffImage;

    // ī�� Ÿ�� �̹���
    [SerializeField] GameObject taroImage;

    public string spriteNameToLoad;

    public PRS originPRS; // ī�� ������ġ�� ���� PRS Ŭ����

    // ī�尡 ���ȴ��� Ȯ��
    [SerializeField] public bool checkCard = false;

    // |------------------------------

    public bool isInUse = false;

    [SerializeField] CardPopup cardPopup;

    public int bonusChipByTaro = 0; // Ÿ�η� ���� �߰��� Ĩ ��

    // ���� ������ ���� ID (������� Ÿ�� ����)
    public int EffectiveId => itemdata.id + bonusChipByTaro;


    // |------------------------------

    // ������Ʈ Ǯ��
    //
    // Card.ManagedPool : �� ī�� ��ü�� �ڱ� Ǯ�� �˾ƾ� �ڰ� �ݳ� ������ 
    //                    ��, �� ī�� ��ü�� ��� Ǯ���� �Դ��� ����ϴ� �뵵
    // |------------------------------

    private IObjectPool<Card> ManagedPool;

    // �Ű������� ���� Card Ÿ���� pool�� ManagedPool�� ����
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

        // ī�� �˾� ���� ����
        PopupText();
    }

    public void PopupText()
    {
        cardPopup.Initialize(itemdata.name, EffectiveId);
    }

    // |------------------------------


    public bool isAnimating = false;


    // ���콺�� ī�忡 ����� �� -> ȿ����, �˾� ����
    private void OnMouseEnter()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-EnterCard");
        cardPopup.MouseEnter();
        AnimationManager.Instance.OnEnterCard(gameObject);
    }

    // ���콺�� ī�带 ������ �� -> �˾� �ݱ�
    private void OnMouseExit()
    {
        cardPopup.MouseExit();
        AnimationManager.Instance.OnExitCard(gameObject);
    }

    // |------------------------------

    // ī�� ����, ��������Ʈ ���� (���ۿ��� ī�带 ���� �� ����)
    public void Setup(ItemData item)
    {
        bonusChipByTaro = 0;

        // ����ü�� itemdata ����
        this.itemdata = new ItemData()
        { 
            name = item.name,
            id = item.id,
            suit = item.suit,
            front = item.front,
            inherenceID = item.inherenceID,
        };

        // Ÿ�� ȿ���� ���� ����
        bonusChipByTaro = 0; // �׻� �ʱ�ȭ

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
            Debug.LogWarning($"ī�� ��������Ʈ ã�� �� ����");
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


    // ī�� ���� �ִϸ��̼�
    public void MoveTransform(PRS prs, bool useAnimation, float duration, System.Action onComplete = null)
    {

        if (moveTween != null && moveTween.IsActive())
        {
            moveTween.Kill();
            moveTween = null;
        }

        if (this == null || transform == null) return; // ���� ����


        if (useAnimation)
        {
            if (TryGetComponent<Collider2D>(out var collider))
                collider.enabled = false;

            // ���� Tween�� �ִٸ� �����ϰ� ����
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
                //collider.enabled = true; // �̰� �� ����>??????
                onComplete?.Invoke();
            });

            moveTween = seq; // Sequence�� Tween�̶� ���� ����
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

    // ���콺�� Ŭ���ϸ� CardIDdata ����Ʈ�� ī�� �ֱ� (�ִ�5��) & ����
    public void OnMouseDown()
    {
        // ���� ���콺 �����Ͱ� UI ��� ���� ���� ���, �Ʒ� �ڵ� ������ �ǳʶݴϴ�.
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return; // Ŭ�� ó������ ����
        }

        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ClickCard");

        OnCardClicked();
    }


    
    public override void OnCardClicked()
    {
        // �����ϰ� Tween ���� �˻�
        if (moveTween != null && moveTween.IsActive() && moveTween.IsPlaying())
            return;

        // ī�� ����
        if (checkCard && PokerManager.Instance.cardData.SelectCards.Count <= 5)
        {
            // �� ��ũ��Ʈ�� �޸� Card�� �Ű������� ����
            PokerManager.Instance.DeselectCard(this);

            AnimationManager.Instance.CardAnime(cardPrefabs);

            checkCard = false;
        }
        // ī�� �߰�
        else if (PokerManager.Instance.cardData.SelectCards.Count < 5)
        {
            PokerManager.Instance.SelectCard(this);

            AnimationManager.Instance.ReCardAnime(cardPrefabs);

            checkCard = true;
        }
        else // ī�� �� �� 
        {
            AnimationManager.Instance.NoCardAnime(cardPrefabs);

        }

    }

    // |------------------------------

    // ī�� �ݶ��̴� Ȱ��ȭ
    public override void OnCollider()
    {
        Collider2D.enabled = true;
    }

    // ī�� �ݶ��̴� ��Ȱ��ȭ
    public override void OffCollider()
    {

        Collider2D.enabled = false;

    }

    // |-----------------------------


    // Ʈ�� ������ ���� DOKill
    public void KillTween()
    {
        if (moveTween != null && moveTween.IsActive())
        {
            moveTween.Kill();
            moveTween = null;
        }
    }

    // |-----------------------------

    // ����� �̹��� Ȱ��ȭ 

    public void OndebuffImage()
    {
        debuffImage.SetActive(true);
    }

    // ����� �̹��� ��Ȱ��ȭ
    public void OffdebuffImage()
    {
        debuffImage.SetActive(false);
    }

    // Ÿ�� �̹��� Ȱ��ȭ
    public void OnTaroImage()
    {
        if (taroImage != null)
            taroImage.SetActive(true);
    }

    // Ÿ�� �̹��� ��Ȱ��ȭ
    public void OffTaroImage()
    {
        if (taroImage != null)
            taroImage.SetActive(false);
    }
}