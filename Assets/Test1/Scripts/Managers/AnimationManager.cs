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

    public void PlayCardAnime(GameObject cardPrefabs)
    {
        Transform t = cardPrefabs.transform;

        // ���� Tween ����
        t.DOKill();

        // Sequence ����
        Sequence seq = DOTween.Sequence();

        // 1�ܰ�: ȸ�� (���� ������ ƨ��� ����)
        seq.Append(t.DORotate(new Vector3(t.eulerAngles.x, t.eulerAngles.y, t.eulerAngles.z + 3f), 0.1f));
        seq.Append(t.DORotate(new Vector3(t.eulerAngles.x, t.eulerAngles.y, t.eulerAngles.z), 0.1f));

        // ���ÿ� ������ ����
        seq.Join(t.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.5f));
        seq.Append(t.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.3f));

        // moveTween�� ����
        moveTween = seq;
    }


    // ��Ŀ �÷��� �� ī�� �ִϸ��̼�
    public void PlayJokerCardAnime(GameObject cardPrefabs)
    {
        cardPrefabs.transform.DOScale(new Vector3(1.05f, 1.05f, 1.05f), 0.3f).
            OnComplete(() => { cardPrefabs.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f); });
    }



    public void CardAnime(Transform cardTransform)
    {
        if (DOTween.IsTweening(cardTransform))
            DOTween.Kill(cardTransform); // ���� Ʈ�� ����

        cardTransform.DOMove(new Vector3(cardTransform.transform.position.x,
           cardTransform.transform.position.y - 0.5f,
           cardTransform.transform.position.z), 0.2f);
    }

    // ī�带 �ٽ� ������ �� ���ڸ� �ִϸ��̼�
    public void ReCardAnime(Transform cardTransform)
    {
        if (DOTween.IsTweening(cardTransform))
            DOTween.Kill(cardTransform); // ���� Ʈ�� ����

        cardTransform.DOMove(new Vector3(cardTransform.transform.position.x,
           cardTransform.transform.position.y + 0.5f,
           cardTransform.transform.position.z), 0.2f);
    }

    // ī�带 �� ������ �� �ִϸ��̼� 
    public void NoCardAnime(Transform cardTransform)
    {
        cardTransform.DOKill(); // ���� Tween ����

        cardTransform.DOScale(new Vector3(0.65f, 0.65f, 0.65f), 0.1f).
            OnComplete(() => { cardTransform.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.2f); });
    }
}
