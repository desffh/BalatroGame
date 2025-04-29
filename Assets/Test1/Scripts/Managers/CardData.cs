using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���õ� ī����� ����� selectCards ����Ʈ�� ����

public class CardData : MonoBehaviour
{
    // ���� �� ī�� ����Ʈ
    private List<Card> selectCards = new List<Card>();

    // ����Ʈ �б� ���� (�Ϲ� ������Ƽ ��� �� ���� ������ ������ ��������)
    public IReadOnlyList<Card> SelectCards => selectCards;

    // |------------------------------------------------

    // ī�� �߰�
    public void AddSelectCard(Card card)
    {
        if(!selectCards.Contains(card))
        {
            selectCards.Add(card);
        }
    }

    // ī�� ����
    public void RemoveSelectCard(Card card)
    {
        selectCards.Remove(card);
    }

    // ī�� ����Ʈ �ʱ�ȭ
    public void ClearSelectCard()
    {
        selectCards.Clear();
    }
}
