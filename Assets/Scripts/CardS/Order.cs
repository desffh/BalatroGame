using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카드 배치 시 order값 조정 
// 
// 최근 카드가 제일 위에 렌더링되도록 설정
public class Order : MonoBehaviour
{
    [SerializeField] Renderer CardRenderer;

    [SerializeField] Renderer EffectRenderer;      // 효과 이미지 추가

    [SerializeField] Renderer DebuffRenderer;      // 효과 이미지 추가


    int originOrder;

    // 카드 정렬 (order) -> 카드가 생성될 때 호출될 함수
    public void SetOriginOrder(int originOrder)
    {
        this.originOrder = originOrder;
        SetOrder(originOrder);
    }

    public void SetMostFrontOrder(bool isMostFront)
    {
        SetOrder(isMostFront ? 100 : originOrder);
    }

    // 카드가 생성될 때마다 * 10
    public void SetOrder(int order)
    {
        int mulOrder = order * 10;

        if (CardRenderer != null)
            CardRenderer.sortingOrder = mulOrder;

        if (EffectRenderer != null)
            EffectRenderer.sortingOrder = mulOrder + 1; // 앞면보다 살짝 위에

        if (DebuffRenderer != null)
            DebuffRenderer.sortingOrder = mulOrder + 2; // 앞면보다 살짝 위에
    }
}
