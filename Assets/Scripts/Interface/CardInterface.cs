using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 조커 카드 / 행성 카드 / 타로 카드 인터페이스


// 조커 능력 인터페이스
public interface IJokerEffect
{
    bool ApplyEffect(JokerEffectContext context);
}

// 나중에 다른 매니저를 만들어서 HoldManager말고 다른 매니저를 참조하게 하자
//
// -> 핸드, 버리기, 머니 등을 모두 관여할 수 있게 
//
// 조커 카드 판매 시 호출 

public interface IExitEffect
{
    // HoldManager : 직접적인 계산 과정을 담당

    // JokerCard : 조커카드의 정보, 애니메이션을 처리할 수 있다

    void ExitEffect(JokerCard jokerCard);
}


// 조커 효과 리셋
public interface IResettableEffect
{
    void ResetEffect();
}

// 조커 효과에 사용 될 컨텍스트들
public class JokerEffectContext
{
    public StateManager StateManager; // 머니, 핸드, 버리기, 칩, 배수 사용 가능

    public JokerCard MyJoker; // 조커 카드 접근 가능

    public MyJokerCard MyJokerCard; // 내 조커 카드 목록에 접근 가능 

    public string CurrentHandType; // 조커의 타입

    public List<Card> SelectedCards; // 현재 선택된 카드들 (계산중인)

    public List<string> HandTypes; // 족보 하위 타입 문자열 리스트
}

// |-----------------------

/// 마커 인터페이스 (상점 진입 조건으로 사용)
public interface IShopItem { }


// 상점 카드 인터페이스 (조커카드, 행성카드, 타로카드 가 구현) 
public interface IShopCard
{

    public string Name { get; }

    public int cost { get; }

    public Sprite Icon { get; }
}

// |-----------------------

/// 구매 가능 카드 (조커, 타로, 행성)
public interface IBuyCard
{
    void OnBuy(Transform parent, MyJokerCard list, ICardSelectionHandler handler);

}


/// 오브젝트 동적 생성 카드 (조커, 타로의 선택된 카드만)
public interface IInstantCard
{
    GameObject GetPrefab();
    void InstantiateCard(Transform parent);
}

/// 선택 가능 카드 (조커, 타로팩, 행성팩, 타로카드만)
public interface ISelectCard
{
    // 구매하기 버튼, 판매하기 버튼 활성화용
    void OnSelected();
    void OffSelected();

    bool CanBeSold { get; }  // 판매 가능한 상태인지
    bool IsInPlayerInventory { get; } // 내 카드 영역에 있는지
}

// 카드 선택을 처리할 핸들러 (Shop이 구현)
public interface ICardSelectionHandler
{
    // 매개변수로 카드 인터페이스를 받음
    void OnCardSelected(ISelectCard card);
    void OnCardDeselected(ISelectCard card);
}


/// 판매 가능한 카드 (조커, 타로)
public interface ISellCard
{
    void OnSell();
}
