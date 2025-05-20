using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class AnimationManager : Singleton<AnimationManager>
{
    private float defaultFontsize = 45f;

    protected override void Awake()
    {
        base.Awake();

        DOTween.SetTweensCapacity(1000, 200); // Tween ��, Sequence ��
    }

    // ���ھ�� �ؽ�Ʈ
    public void CaltransformAnime(TextMeshProUGUI scoreText)
    {
       // �� ó�� ��Ʈ ����� ����
        if(defaultFontsize == 0f)
        {
            defaultFontsize = scoreText.fontSize;
        }

        scoreText.DOKill(); // ���� Tween ����

        // �۾� ũ�⸦ Ű��� �ִϸ��̼�
        DOTween.To(() => scoreText.fontSize, x => scoreText.fontSize = x, defaultFontsize * 1.3f, 0.1f)
        .OnComplete(() =>
        {
            // �۾� ũ�⸦ �ٽ� ���� ũ��� ���̴� �ִϸ��̼�
            DOTween.To(() => scoreText.fontSize, x => scoreText.fontSize = x, defaultFontsize, 0.1f);
        });
    }

    // ī�� ���� ��� �� �ؽ�Ʈ �ִϸ��̼�
    public void ShowTextAnime(TextMeshPro scoreText)
    {
        scoreText.DOKill();

        // ���� ��Ʈ ũ�� ����
        float originalFontSize = scoreText.fontSize;

        // �۾� ũ�⸦ Ű��� �ִϸ��̼�
        DOTween.To(() => scoreText.fontSize, x => scoreText.fontSize = x, originalFontSize * 1.3f, 0.2f)
        .OnComplete(() =>
        {
            // �۾� ũ�⸦ �ٽ� ���� ũ��� ���̴� �ִϸ��̼�
            DOTween.To(() => scoreText.fontSize, x => scoreText.fontSize = x, originalFontSize, 0.2f)
            .OnComplete(() =>
            {
                scoreText.gameObject.SetActive(false);
            });
        });
    }

    public void ShowTextAnime(TextMeshProUGUI scoreText)
    {
        scoreText.DOKill();

        // ���� ��Ʈ ũ�� ����
        float originalFontSize = scoreText.fontSize;

        // �۾� ũ�⸦ Ű��� �ִϸ��̼�
        DOTween.To(() => scoreText.fontSize, x => scoreText.fontSize = x, originalFontSize * 1.35f, 0.3f)
        .OnComplete(() =>
        {
            // �۾� ũ�⸦ �ٽ� ���� ũ��� ���̴� �ִϸ��̼�
            DOTween.To(() => scoreText.fontSize, x => scoreText.fontSize = x, originalFontSize, 0.3f)
            .OnComplete(() =>
            {
                scoreText.gameObject.SetActive(false);
            });
        });
    }
    public Tween moveTween;

    public Tween PlayCardAnime(GameObject cardPrefabs)
    {
        Transform t = cardPrefabs.transform;

       // // ���� Tween ����
       // if (moveTween != null && moveTween.IsActive())
       // {
       //     moveTween.Kill();
       // }
        
        t.DOKill();

        //Sequence ����
        Sequence seq = DOTween.Sequence();

        // 1�ܰ�: ȸ�� (���� ������ ƨ��� ����)
        seq.Append(t.DORotate(new Vector3(t.eulerAngles.x, t.eulerAngles.y, t.eulerAngles.z + 3f), 0.1f));
        seq.Append(t.DORotate(new Vector3(t.eulerAngles.x, t.eulerAngles.y, t.eulerAngles.z), 0.1f));

        // ���ÿ� ������ ����
        seq.Join(t.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.5f));
        seq.Append(t.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.3f));

        // �ڵ����� ������ �ʵ��� ���� (�߿�!)
        seq.SetAutoKill(false);

        // Play (���������!)
        seq.Play();

        // moveTween�� ����
        moveTween = seq;

        return seq;
    }

    // ī�忡 ���콺�� ������ ��
    public void OnEnterCard(GameObject cardPrefabs)
    {
        if (cardPrefabs.GetComponent<Card>().isAnimating) return;
        Transform t = cardPrefabs.transform;
        t.DOKill();

        t.DOScale(new Vector3(0.75f, 0.75f, 0.75f), 0.05f);
    }


    // ī�忡�� ���콺�� ���� ��
    public void OnExitCard(GameObject cardPrefabs)
    {
        if (cardPrefabs.GetComponent<Card>().isAnimating) return;

        Transform t = cardPrefabs.transform;

        t.DOKill();

        t.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.05f);
    }

    // ��Ŀ �÷��� �� ī�� �ִϸ��̼�
    public void PlayJokerCardAnime(GameObject cardPrefabs)
    {
        cardPrefabs.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.3f).
            OnComplete(() => { cardPrefabs.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f); });
    }

    // ī�带 ������ �� �ö󰡴� �ִϸ��̼�
    public void CardAnime(Transform cardTransform)
    {

        if (DOTween.IsTweening(cardTransform))
            DOTween.Kill(cardTransform); // ���� Ʈ�� ����

        cardTransform.GetComponent<Card>().isAnimating = true;

        // �ʱ� ũ�� ����
        cardTransform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        
        // Sequence ����
        Sequence seq = DOTween.Sequence();

        seq.Append(cardTransform.DOMove(new Vector3(cardTransform.transform.position.x,
           cardTransform.transform.position.y - 0.5f,
           cardTransform.transform.position.z), 0.2f).
           OnComplete(() => { cardTransform.GetComponent<Card>().isAnimating = false; }));
    }

    // ī�带 �ٽ� ������ �� ���ڸ� �ִϸ��̼�
    public void ReCardAnime(Transform cardTransform)
    {
        if (DOTween.IsTweening(cardTransform))
            DOTween.Kill(cardTransform); // ���� Ʈ�� ����
        
        cardTransform.GetComponent<Card>().isAnimating = true;

        // �ʱ� ũ�� ����
        cardTransform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

        // Sequence ����
        Sequence seq = DOTween.Sequence();

        seq.Append(cardTransform.DOMove(new Vector3(cardTransform.transform.position.x,
           cardTransform.transform.position.y + 0.5f,
           cardTransform.transform.position.z), 0.2f).
           OnComplete(() => { cardTransform.GetComponent<Card>().isAnimating = false; }));
    }

    // ī�带 �� ������ �� �ִϸ��̼� 
    public void NoCardAnime(Transform cardTransform)
    {
        cardTransform.DOKill(); // ���� Tween ����

        cardTransform.GetComponent<Card>().isAnimating = true;

        // Sequence ����
        Sequence seq = DOTween.Sequence();

        seq.Append(cardTransform.DOScale(new Vector3(0.65f, 0.65f, 0.65f), 0.1f).
            OnComplete(() => { cardTransform.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.2f); }).
            OnComplete(() => { cardTransform.GetComponent<Card>().isAnimating = false; }));
    }

    // Ÿ���� ���
    public void TMProText(TextMeshProUGUI text, float duration)
    {
        text.maxVisibleCharacters = 0;

        DOTween.To(x => text.maxVisibleCharacters = (int)x, 0f, text.text.Length, duration);
    }

    // �� ī�� 
    public void OnEnterViewCard(GameObject cardPrefabs)
    {
        cardPrefabs.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f);
    }
    public void OnExitViewCard(GameObject cardPrefabs)
    {
        cardPrefabs.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
    }

    // |-------------------

    // ��Ŀ�� ���콺�� ������ ��
    public void OnEnterShopCard(GameObject cardPrefabs)
    {
        cardPrefabs.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f);
    }

    // ��Ŀ�� ���콺�� ������ ��
    public void OnExitShopCard(GameObject cardPrefabs)
    {
        cardPrefabs.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
    }

    // |-------------------

}
