using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ����ε� ���� -> ���ؽ�Ʈ ����

[System.Serializable]
public class BlindRound
{
    public string blindName;

    public int score;

    public int money;

    public Sprite blindImage;

    public Color blindColor;


    public bool isBoss; // ���� ����ε����� / �ƴ���

    public IBossDebuff bossDebuff; // ���� ��ü
}
