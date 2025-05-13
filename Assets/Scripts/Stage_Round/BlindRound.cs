using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 개별 블라인드 정보 -> 컨텍스트 패턴

[System.Serializable]
public class BlindRound
{
    public string blindName;

    public int score;

    public int money;

    public Sprite blindImage;

    public Color blindColor;


    public bool isBoss; // 보스 블라인드인지 / 아닌지

    public IBossDebuff bossDebuff; // 전략 객체
}
