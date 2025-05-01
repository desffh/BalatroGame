using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class AnimationManager : Singleton<AnimationManager>
{

    protected override void Awake()
    {
        base.Awake();

        DOTween.SetTweensCapacity(1000, 200); // Tween ��, Sequence ��
    }

    // ���ھ�� �ؽ�Ʈ
    public void CaltransformAnime(TextMeshProUGUI scoreText)
    {
        scoreText.DOKill(); // ���� Tween ����

        // ���� ��Ʈ ũ�� ����
        float originalFontSize = scoreText.fontSize;

        // �۾� ũ�⸦ Ű��� �ִϸ��̼�
        DOTween.To(() => scoreText.fontSize, x => scoreText.fontSize = x, originalFontSize * 1.3f, 0.1f)
        .OnComplete(() =>
        {
            // �۾� ũ�⸦ �ٽ� ���� ũ��� ���̴� �ִϸ��̼�
            DOTween.To(() => scoreText.fontSize, x => scoreText.fontSize = x, originalFontSize, 0.1f);
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

    // �ڵ� �÷��� �� ī�� �̵� �ִϸ��̼�
    public void PlayCardAnime(GameObject cardPrefabs)
    {
        cardPrefabs.transform.DORotate(new Vector3(cardPrefabs.transform.position.x,
            cardPrefabs.transform.position.y, cardPrefabs.transform.position.z + 3f), 0.1f).
            OnComplete(() =>
            {
                cardPrefabs.transform.DORotate(new Vector3(cardPrefabs.transform.position.x,
            cardPrefabs.transform.position.y, cardPrefabs.transform.position.z), 0.1f);
            });

        cardPrefabs.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.5f).
            OnComplete(() => { cardPrefabs.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.3f); });
    }

    // ī�带 ������ �� �ִϸ��̼�
    public void CardAnime(Transform cardTransform)
    {
        cardTransform.DOKill(); // ���� Tween ����

        cardTransform.DOMove(new Vector3(cardTransform.transform.position.x,
           cardTransform.transform.position.y - 0.5f,
           cardTransform.transform.position.z), 0.2f);
    }

    // ī�带 �ٽ� ������ �� ���ڸ� �ִϸ��̼�
    public void ReCardAnime(Transform cardTransform)
    {
        cardTransform.DOKill(); // ���� Tween ����

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
