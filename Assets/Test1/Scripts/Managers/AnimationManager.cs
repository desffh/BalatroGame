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

        DOTween.SetTweensCapacity(1000, 200); // Tween 수, Sequence 수
    }

    // 스코어들 텍스트
    public void CaltransformAnime(TextMeshProUGUI scoreText)
    {
        scoreText.DOKill(); // 이전 Tween 제거

        // 원래 폰트 크기 저장
        float originalFontSize = scoreText.fontSize;

        // 글씨 크기를 키우는 애니메이션
        DOTween.To(() => scoreText.fontSize, x => scoreText.fontSize = x, originalFontSize * 1.3f, 0.1f)
        .OnComplete(() =>
        {
            // 글씨 크기를 다시 원래 크기로 줄이는 애니메이션
            DOTween.To(() => scoreText.fontSize, x => scoreText.fontSize = x, originalFontSize, 0.1f);
        });
    }

    // 카드 점수 계산 시 텍스트 애니메이션
    public void ShowTextAnime(TextMeshPro scoreText)
    {
        scoreText.DOKill();

        // 원래 폰트 크기 저장
        float originalFontSize = scoreText.fontSize;

        // 글씨 크기를 키우는 애니메이션
        DOTween.To(() => scoreText.fontSize, x => scoreText.fontSize = x, originalFontSize * 1.3f, 0.2f)
        .OnComplete(() =>
        {
            // 글씨 크기를 다시 원래 크기로 줄이는 애니메이션
            DOTween.To(() => scoreText.fontSize, x => scoreText.fontSize = x, originalFontSize, 0.2f)
            .OnComplete(() =>
            {
                scoreText.gameObject.SetActive(false);
            });
        });
    }

    // 핸드 플레이 시 카드 이동 애니메이션
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

    // 카드를 눌렀을 때 애니메이션
    public void CardAnime(Transform cardTransform)
    {
        cardTransform.DOKill(); // 기존 Tween 제거

        cardTransform.DOMove(new Vector3(cardTransform.transform.position.x,
           cardTransform.transform.position.y - 0.5f,
           cardTransform.transform.position.z), 0.2f);
    }

    // 카드를 다시 눌렀을 때 제자리 애니메이션
    public void ReCardAnime(Transform cardTransform)
    {
        cardTransform.DOKill(); // 기존 Tween 제거

        cardTransform.DOMove(new Vector3(cardTransform.transform.position.x,
           cardTransform.transform.position.y + 0.5f,
           cardTransform.transform.position.z), 0.2f);
    }

    // 카드를 다 눌렀을 때 애니메이션 
    public void NoCardAnime(Transform cardTransform)
    {
        cardTransform.DOKill(); // 기존 Tween 제거

        cardTransform.DOScale(new Vector3(0.65f, 0.65f, 0.65f), 0.1f).
            OnComplete(() => { cardTransform.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.2f); });
    }
}
