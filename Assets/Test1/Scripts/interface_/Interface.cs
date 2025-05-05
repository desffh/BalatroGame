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
    bool ApplyEffect(List<Card> selectedCards, string currentHandType, HoldManager holdManager, string jokerCategory, JokerCard myJoker);
}

// |------------------------------------------------------

public interface IAudioService
{
    void PlaySFX(string clipName);

    void PlayBGM(string clipName, bool loop = true);

    void StopBGM();
}

public interface IAudioServicePitch
{ 

    void PlaySFXPitch(string clipName);
}
