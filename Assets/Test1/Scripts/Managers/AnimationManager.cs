using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class AnimationManager : Singleton<AnimationManager>
{
    // 스코어들 텍스트
    public void CaltransformAnime(TextMeshProUGUI scoreText)
    {
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
    
    // 핸드 플레이 시 카드 이동
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

    // 카드를 눌렀을 때
    public void CardAnime(Transform cardTransform)
    {
        cardTransform.DOMove(new Vector3(cardTransform.transform.position.x,
           cardTransform.transform.position.y - 0.5f,
           cardTransform.transform.position.z), 0.2f);
    }

    // 카드를 눌렀을 때 제자리
    public void ReCardAnime(Transform cardTransform)
    {
        cardTransform.DOMove(new Vector3(cardTransform.transform.position.x,
           cardTransform.transform.position.y + 0.5f,
           cardTransform.transform.position.z), 0.2f);
    }

    public void NoCardAnime(Transform cardTransform)
    {
        cardTransform.DOScale(new Vector3(0.65f, 0.65f, 0.65f), 0.1f).
            OnComplete(() => { cardTransform.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.2f); });
    }
}
