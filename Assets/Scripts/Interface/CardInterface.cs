using System;
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

// ��ư ��ġ�� ����
public interface IShopCard
{
    int cost { get; }
    RectTransform Transform { get; } // ī�� ��ġ

    event Action<IShopCard> OnClicked; // Ŭ�� �̺�Ʈ (��ưȰ��ȭ ��Ȱ��ȭ)
}


/// ���� ���� ī�� (��Ŀ)
public interface IBuyCard
{
    void OnBuy(Transform parent, MyJokerCard list, Action<IShopCard> onCreated);
}


public interface IBuyPlanetCard
{
    void OnBuy(Action<IShopCard> onCreated);
}


/// ������Ʈ ���� ���� ī�� (��Ŀ, Ÿ���� ���õ� ī�常)
public interface IInstantCard
{
    GameObject GetPrefab();
    void InstantiateCard(Transform parent);
}

/// ���� ���� ī�� (��Ŀ, Ÿ����, �༺��, Ÿ��ī�常)
public interface ICanBeSold
{
    bool CanBeSold { get; }  // �Ǹ� ������ ��������
    bool IsInPlayerInventory { get; } // �� ī�� ������ �ִ���
}



/// �Ǹ� ������ ī�� (��Ŀ, Ÿ��)
public interface ISellCard
{
    void OnSell();
}
