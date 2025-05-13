using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ī�� ��ġ �� order�� ���� 
// 
// �ֱ� ī�尡 ���� ���� �������ǵ��� ����
public class Order : MonoBehaviour
{
    [SerializeField] Renderer CardRenderer;

    [SerializeField] Renderer EffectRenderer;      // ȿ�� �̹��� �߰�

    [SerializeField] Renderer DebuffRenderer;      // ȿ�� �̹��� �߰�


    int originOrder;

    // ī�� ���� (order) -> ī�尡 ������ �� ȣ��� �Լ�
    public void SetOriginOrder(int originOrder)
    {
        this.originOrder = originOrder;
        SetOrder(originOrder);
    }

    public void SetMostFrontOrder(bool isMostFront)
    {
        SetOrder(isMostFront ? 100 : originOrder);
    }

    // ī�尡 ������ ������ * 10
    public void SetOrder(int order)
    {
        int mulOrder = order * 10;

        if (CardRenderer != null)
            CardRenderer.sortingOrder = mulOrder;

        if (EffectRenderer != null)
            EffectRenderer.sortingOrder = mulOrder + 1; // �ո麸�� ��¦ ����

        if (DebuffRenderer != null)
            DebuffRenderer.sortingOrder = mulOrder + 2; // �ո麸�� ��¦ ����
    }
}
