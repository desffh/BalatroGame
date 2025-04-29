using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


// 선택된 카드들이 저장된 selectCards 리스트를 관리

public class CardData : MonoBehaviour
{
    // 저장 된 카드 리스트
    private List<Card> selectCards = new List<Card>();

    // 리스트 읽기 전용 (일반 프로퍼티 사용 시 내부 데이터 수정이 가능해짐)
    public IReadOnlyList<Card> SelectCards => selectCards;

    // |------------------------------------------------

    // 카드 추가
    public void AddSelectCard(Card card)
    {
        if(!selectCards.Contains(card))
        {
            selectCards.Add(card);
            
            // 나중에 위치를 바꿔주자 -> CardData에 정렬이 있으면 SRP 위배
            selectCards = selectCards.OrderBy(x => x.itemdata.id).ToList();
        }
    }

    // 카드 제거
    public void RemoveSelectCard(Card card)
    {
        selectCards.Remove(card);
    }

    // 카드 리스트 초기화
    public void ClearSelectCard()
    {
        selectCards.Clear();
    }
}
