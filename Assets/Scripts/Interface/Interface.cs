using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

// 포커 족보 인터페이스
public interface IPokerHandle
{
    void PokerHandle(List<Card> cards, List<int> saveNum);
    string pokerName { get; }

    int plus {  get;}
    int multiple { get;}
}

// |------------------------------------------------------

// 추상 클래스
public abstract class IsStrightPlush
{
    // 스트레이트인지 확인 (ex 1 2 3 4 5)
    public bool isStraight(List<Card> cards)
    {
        if(cards.Count == 5)
        {
            for (int i = 1; i < cards.Count; i++)
            {
                // 현재카드와 바로 앞 카드의 숫자 차이가 1인지 확인
                // 하나라도 다르면 바로 false반환
                if (cards[i].itemdata.id != cards[i - 1].itemdata.id + 1)
                {
                    return false;
                }
            }
        }
        return true;
    }
    

    // 플러시인지 확인 (ex 다이아 5개)
    public bool isFlush(List<Card> cards)
    {
        if(cards.Count == 5)
        {
            // Suit타입을 저장할 변수
            string firstSuit = cards[0].itemdata.suit;
            for (int i = 0; i < cards.Count; i++)
            {
                // [0]번째와 hand 인덱스가 하나라도 다르면 false반환
                if (cards[i].itemdata.suit != firstSuit)
                {
                    return false;
                }
            }
        }
        return true;
    }

}

// |------------------------------------------------------


// 문양이 있다면 카드마다 배수 점수 추가
public interface IColorJoker
{
    void ColorJoker(List<Card> cards, List<Card> saveSuit);
}

// 족보가 있다면 배수 점수 추가
public interface IPokerJoker
{
    void PokerJoker(List<Card> cards, int saveMultiple);
}


// |------------------------------------------------------

// 게임 상태 인터페이스
public interface IGameState
{
    void Enter();
    void Exit();
}

// |------------------------------------------------------


// 팝업 텍스트 인터페이스 
public interface IPopupText
{
    string type { get; }

    void Initialize(string name, string info, int multiple, int cost);
}

// |------------------------------------------------------

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
}




// |------------------------------------------------------

// 오디오 등록용 인터페이스 - 서비스 로케이터
public interface IAudioService
{
    void PlaySFX(string clipName);

    void PlayBGM(string clipName, bool loop = true);

    void StopBGM();
}

// 효과음 피치 조절용 인터페이스 - 서비스 로케이터
public interface IAudioServicePitch
{ 

    void PlaySFXPitch(string clipName);
}

// |------------------------------------------------------

// MVP - 리셋용
public interface IReset
{
    void Reset();
}

// Hand - Delete 용
public interface IHandDeleteSetting : IReset
{
    void MinusHand();
    void MinusDelete();
    void PlusDelete();

    int GetHand();
    int GetDelete();


}

// Multiply - Chip 용
public interface IMultiplyChipSetting : IReset 
{
    void SetPlus(int value = 0);
    void SetMultiply(int value = 0);
    void AddPlus(int value);
    void AddMultiply(int value);

    int GetChip();
    int GetMultiply();
}

public interface IPlus
{
    void Add(int value);
}

// Money 용 -> 리셋, 추가, 제거
public interface IMoneySetting : IReset, IPlus
{
    void Remove(int value);

    int GetMoney();
}

public interface IRoundEntySetting : IReset
{
    void EntyAdd();

    void RoundAdd();

    int GetEnty();

    int GetRound();
}

// |------------------------------------------------------

// 보스 블라인드 디버프
public interface IBossDebuff
{
    void ApplyDebuff();
}
