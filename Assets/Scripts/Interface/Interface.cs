using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

// ��Ŀ ���� �������̽�
public interface IPokerHandle
{
    void PokerHandle(List<Card> cards, List<int> saveNum);
    string pokerName { get; }

    int plus {  get;}
    int multiple { get;}
}

// |------------------------------------------------------

// �߻� Ŭ����
public abstract class IsStrightPlush
{
    // ��Ʈ����Ʈ���� Ȯ�� (ex 1 2 3 4 5)
    public bool isStraight(List<Card> cards)
    {
        if(cards.Count == 5)
        {
            for (int i = 1; i < cards.Count; i++)
            {
                // ����ī��� �ٷ� �� ī���� ���� ���̰� 1���� Ȯ��
                // �ϳ��� �ٸ��� �ٷ� false��ȯ
                if (cards[i].itemdata.id != cards[i - 1].itemdata.id + 1)
                {
                    return false;
                }
            }
        }
        return true;
    }
    

    // �÷������� Ȯ�� (ex ���̾� 5��)
    public bool isFlush(List<Card> cards)
    {
        if(cards.Count == 5)
        {
            // SuitŸ���� ������ ����
            string firstSuit = cards[0].itemdata.suit;
            for (int i = 0; i < cards.Count; i++)
            {
                // [0]��°�� hand �ε����� �ϳ��� �ٸ��� false��ȯ
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


// ������ �ִٸ� ī�帶�� ��� ���� �߰�
public interface IColorJoker
{
    void ColorJoker(List<Card> cards, List<Card> saveSuit);
}

// ������ �ִٸ� ��� ���� �߰�
public interface IPokerJoker
{
    void PokerJoker(List<Card> cards, int saveMultiple);
}


// |------------------------------------------------------

// ���� ���� �������̽�
public interface IGameState
{
    void Enter();
    void Exit();
}

// |------------------------------------------------------


// �˾� �ؽ�Ʈ �������̽� 
public interface IPopupText
{
    string type { get; }

    void Initialize(string name, string info, int multiple, int cost);
}

// |------------------------------------------------------

// ��Ŀ �ɷ� �������̽�
public interface IJokerEffect
{
    bool ApplyEffect(JokerEffectContext context);
}

// ���߿� �ٸ� �Ŵ����� ���� HoldManager���� �ٸ� �Ŵ����� �����ϰ� ����
//
// -> �ڵ�, ������, �Ӵ� ���� ��� ������ �� �ְ� 
//
// ��Ŀ ī�� �Ǹ� �� ȣ�� 

public interface IExitEffect
{
    // HoldManager : �������� ��� ������ ���

    // JokerCard : ��Ŀī���� ����, �ִϸ��̼��� ó���� �� �ִ�

    void ExitEffect(JokerCard jokerCard);
}


// ��Ŀ ȿ�� ����
public interface IResettableEffect
{
    void ResetEffect();
}

// ��Ŀ ȿ���� ��� �� ���ؽ�Ʈ��
public class JokerEffectContext
{
    public StateManager StateManager; // �Ӵ�, �ڵ�, ������, Ĩ, ��� ��� ����

    public JokerCard MyJoker; // ��Ŀ ī�� ���� ����

    public MyJokerCard MyJokerCard; // �� ��Ŀ ī�� ��Ͽ� ���� ���� 

    public string CurrentHandType; // ��Ŀ�� Ÿ��

    public List<Card> SelectedCards; // ���� ���õ� ī��� (�������)
}




// |------------------------------------------------------

// ����� ��Ͽ� �������̽� - ���� ��������
public interface IAudioService
{
    void PlaySFX(string clipName);

    void PlayBGM(string clipName, bool loop = true);

    void StopBGM();
}

// ȿ���� ��ġ ������ �������̽� - ���� ��������
public interface IAudioServicePitch
{ 

    void PlaySFXPitch(string clipName);
}

// |------------------------------------------------------

// MVP - ���¿�
public interface IReset
{
    void Reset();
}

// Hand - Delete ��
public interface IHandDeleteSetting : IReset
{
    void MinusHand();
    void MinusDelete();
    void PlusDelete();

    int GetHand();
    int GetDelete();


}

// Multiply - Chip ��
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

// Money �� -> ����, �߰�, ����
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

// ���� ����ε� �����
public interface IBossDebuff
{
    void ApplyDebuff();
}
