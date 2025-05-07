using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PanelHandler : MonoBehaviour
{
    // clearPanel 애니메이션 완료된 후 호출
    public System.Action onPanelShowComplete;

    void Awake()
    {
        DOTween.Init();

    }

    public void Show()
    {
        gameObject.SetActive(true);

        // DOTween 함수를 차례대로 수행하게 해줍니다.
        var seq = DOTween.Sequence();

        // DOScale 의 첫 번째 파라미터는 목표 Scale 값, 두 번째는 시간입니다.
        seq.Append(transform.DOScale(1.1f, 0.3f));
        seq.Append(transform.DOScale(1f, 0.1f));

        seq.Play();
    }

    public void Hide()
    {
        var seq = DOTween.Sequence();

        seq.Append(transform.DOScale(1.1f, 0.1f));
        seq.Append(transform.DOScale(0.2f, 0.3f));

        // OnComplete 는 seq 에 설정한 애니메이션의 플레이가 완료되면
        // { } 안에 있는 코드가 수행된다는 의미입니다.
        // 여기서는 닫기 애니메이션이 완료된 후 객체를 비활성화 합니다.
        seq.Play().OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void EndPanelShow()
    {
        transform.position = new Vector3(960f, -540f, 0f);

        gameObject.SetActive(true);

        var seq = DOTween.Sequence();

        seq.Append(transform.DOMoveY(540f, 0.7f));
        
        // 애니메이션이 끝났을 때 호출될 콜백
        seq.OnComplete(() =>
        {
            if (onPanelShowComplete != null)
                onPanelShowComplete.Invoke();
        });

        seq.Play();
    }
}