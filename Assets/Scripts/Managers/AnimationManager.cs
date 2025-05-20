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

        DOTween.SetTweensCapacity(1000, 200); // Tween 수, Sequence 수
    }

    // 스코어들 텍스트
    public void CaltransformAnime(TextMeshProUGUI scoreText)
    {
       // 맨 처음 폰트 사이즈만 저장
        if(defaultFontsize == 0f)
        {
            defaultFontsize = scoreText.fontSize;
        }

        scoreText.DOKill(); // 이전 Tween 제거

        // 글씨 크기를 키우는 애니메이션
        DOTween.To(() => scoreText.fontSize, x => scoreText.fontSize = x, defaultFontsize * 1.3f, 0.1f)
        .OnComplete(() =>
        {
            // 글씨 크기를 다시 원래 크기로 줄이는 애니메이션
            DOTween.To(() => scoreText.fontSize, x => scoreText.fontSize = x, defaultFontsize, 0.1f);
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

    public void ShowTextAnime(TextMeshProUGUI scoreText)
    {
        scoreText.DOKill();

        // 원래 폰트 크기 저장
        float originalFontSize = scoreText.fontSize;

        // 글씨 크기를 키우는 애니메이션
        DOTween.To(() => scoreText.fontSize, x => scoreText.fontSize = x, originalFontSize * 1.35f, 0.3f)
        .OnComplete(() =>
        {
            // 글씨 크기를 다시 원래 크기로 줄이는 애니메이션
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

       // // 기존 Tween 제거
       // if (moveTween != null && moveTween.IsActive())
       // {
       //     moveTween.Kill();
       // }
        
        t.DOKill();

        //Sequence 생성
        Sequence seq = DOTween.Sequence();

        // 1단계: 회전 (작은 각도로 튕기는 느낌)
        seq.Append(t.DORotate(new Vector3(t.eulerAngles.x, t.eulerAngles.y, t.eulerAngles.z + 3f), 0.1f));
        seq.Append(t.DORotate(new Vector3(t.eulerAngles.x, t.eulerAngles.y, t.eulerAngles.z), 0.1f));

        // 동시에 스케일 변경
        seq.Join(t.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.5f));
        seq.Append(t.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.3f));

        // 자동으로 죽이지 않도록 설정 (중요!)
        seq.SetAutoKill(false);

        // Play (명시적으로!)
        seq.Play();

        // moveTween에 저장
        moveTween = seq;

        return seq;
    }

    // 카드에 마우스가 들어왔을 때
    public void OnEnterCard(GameObject cardPrefabs)
    {
        if (cardPrefabs.GetComponent<Card>().isAnimating) return;
        Transform t = cardPrefabs.transform;
        t.DOKill();

        t.DOScale(new Vector3(0.75f, 0.75f, 0.75f), 0.05f);
    }


    // 카드에서 마우스를 뗏을 때
    public void OnExitCard(GameObject cardPrefabs)
    {
        if (cardPrefabs.GetComponent<Card>().isAnimating) return;

        Transform t = cardPrefabs.transform;

        t.DOKill();

        t.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.05f);
    }

    // 조커 플레이 시 카드 애니메이션
    public void PlayJokerCardAnime(GameObject cardPrefabs)
    {
        cardPrefabs.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.3f).
            OnComplete(() => { cardPrefabs.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f); });
    }

    // 카드를 눌렀을 때 올라가는 애니메이션
    public void CardAnime(Transform cardTransform)
    {

        if (DOTween.IsTweening(cardTransform))
            DOTween.Kill(cardTransform); // 기존 트윈 제거

        cardTransform.GetComponent<Card>().isAnimating = true;

        // 초기 크기 설정
        cardTransform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        
        // Sequence 생성
        Sequence seq = DOTween.Sequence();

        seq.Append(cardTransform.DOMove(new Vector3(cardTransform.transform.position.x,
           cardTransform.transform.position.y - 0.5f,
           cardTransform.transform.position.z), 0.2f).
           OnComplete(() => { cardTransform.GetComponent<Card>().isAnimating = false; }));
    }

    // 카드를 다시 눌렀을 때 제자리 애니메이션
    public void ReCardAnime(Transform cardTransform)
    {
        if (DOTween.IsTweening(cardTransform))
            DOTween.Kill(cardTransform); // 기존 트윈 제거
        
        cardTransform.GetComponent<Card>().isAnimating = true;

        // 초기 크기 설정
        cardTransform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

        // Sequence 생성
        Sequence seq = DOTween.Sequence();

        seq.Append(cardTransform.DOMove(new Vector3(cardTransform.transform.position.x,
           cardTransform.transform.position.y + 0.5f,
           cardTransform.transform.position.z), 0.2f).
           OnComplete(() => { cardTransform.GetComponent<Card>().isAnimating = false; }));
    }

    // 카드를 다 눌렀을 때 애니메이션 
    public void NoCardAnime(Transform cardTransform)
    {
        cardTransform.DOKill(); // 기존 Tween 제거

        cardTransform.GetComponent<Card>().isAnimating = true;

        // Sequence 생성
        Sequence seq = DOTween.Sequence();

        seq.Append(cardTransform.DOScale(new Vector3(0.65f, 0.65f, 0.65f), 0.1f).
            OnComplete(() => { cardTransform.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.2f); }).
            OnComplete(() => { cardTransform.GetComponent<Card>().isAnimating = false; }));
    }

    // 타이핑 모션
    public void TMProText(TextMeshProUGUI text, float duration)
    {
        text.maxVisibleCharacters = 0;

        DOTween.To(x => text.maxVisibleCharacters = (int)x, 0f, text.text.Length, duration);
    }

    // 뷰 카드 
    public void OnEnterViewCard(GameObject cardPrefabs)
    {
        cardPrefabs.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f);
    }
    public void OnExitViewCard(GameObject cardPrefabs)
    {
        cardPrefabs.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
    }

    // |-------------------

    // 조커에 마우스가 들어왔을 때
    public void OnEnterShopCard(GameObject cardPrefabs)
    {
        cardPrefabs.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f);
    }

    // 조커에 마우스가 나갔을 때
    public void OnExitShopCard(GameObject cardPrefabs)
    {
        cardPrefabs.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
    }

    // |-------------------

}
