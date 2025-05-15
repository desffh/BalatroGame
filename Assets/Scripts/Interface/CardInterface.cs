using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��Ŀ ī�� / �༺ ī�� / Ÿ�� ī�� �������̽�


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

    public List<string> HandTypes; // ���� ���� Ÿ�� ���ڿ� ����Ʈ
}

// |-----------------------

/// ��Ŀ �������̽� (���� ���� �������� ���)
public interface IShopItem { }


// ���� ī�� �������̽� (��Ŀī��, �༺ī��, Ÿ��ī�� �� ����) 
public interface IShopCard
{

    public string Name { get; }

    public int cost { get; }

    public Sprite Icon { get; }
}

// |-----------------------

/// ���� ���� ī�� (��Ŀ, Ÿ��, �༺)
public interface IBuyCard
{
    void OnBuy(Transform parent, MyJokerCard list, ICardSelectionHandler handler);

}


/// ������Ʈ ���� ���� ī�� (��Ŀ, Ÿ���� ���õ� ī�常)
public interface IInstantCard
{
    GameObject GetPrefab();
    void InstantiateCard(Transform parent);
}

/// ���� ���� ī�� (��Ŀ, Ÿ����, �༺��, Ÿ��ī�常)
public interface ISelectCard
{
    // �����ϱ� ��ư, �Ǹ��ϱ� ��ư Ȱ��ȭ��
    void OnSelected();
    void OffSelected();

    bool CanBeSold { get; }  // �Ǹ� ������ ��������
    bool IsInPlayerInventory { get; } // �� ī�� ������ �ִ���
}

// ī�� ������ ó���� �ڵ鷯 (Shop�� ����)
public interface ICardSelectionHandler
{
    // �Ű������� ī�� �������̽��� ����
    void OnCardSelected(ISelectCard card);
    void OnCardDeselected(ISelectCard card);
}


/// �Ǹ� ������ ī�� (��Ŀ, Ÿ��)
public interface ISellCard
{
    void OnSell();
}
