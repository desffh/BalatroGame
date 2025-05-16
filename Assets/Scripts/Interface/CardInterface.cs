using System;
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

// 버튼 위치를 위함
public interface IShopCard
{
    int cost { get; }
    RectTransform Transform { get; } // 카드 위치

    event Action<IShopCard> OnClicked; // 클릭 이벤트 (버튼활성화 비활성화)
}


/// 구매 가능 카드 (조커)
public interface IBuyCard
{
    void OnBuy(Transform parent, MyJokerCard list, Action<IShopCard> onCreated);
}


public interface IBuyPlanetCard
{
    void OnBuy(Action<IShopCard> onCreated);
}


/// 오브젝트 동적 생성 카드 (조커, 타로의 선택된 카드만)
public interface IInstantCard
{
    GameObject GetPrefab();
    void InstantiateCard(Transform parent);
}

/// 선택 가능 카드 (조커, 타로팩, 행성팩, 타로카드만)
public interface ICanBeSold
{
    bool CanBeSold { get; }  // 판매 가능한 상태인지
    bool IsInPlayerInventory { get; } // 내 카드 영역에 있는지
}



/// 판매 가능한 카드 (조커, 타로)
public interface ISellCard
{
    void OnSell();
}
