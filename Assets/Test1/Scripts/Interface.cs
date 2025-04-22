using System.Collections.Generic;
using TMPro;

// ��Ŀ ���� �������̽�
public interface IPokerHandle
{
    void PokerHandle(List<Card> cards, List<int> saveNum);
    string pokerName { get; }

    int plus {  get;}
    int multiple { get;}
}


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


public interface IPokerJoker
{
    void PokerJoker(List<Card> cards, int saveMultiple);
}


// |------------------------------------------------------

public interface IPopupText
{
    void Initialize(string name, string info, int multiple, int cost);
}

// |------------------------------------------------------
