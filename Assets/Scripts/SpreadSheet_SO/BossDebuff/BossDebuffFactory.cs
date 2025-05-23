using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossDebuffType
{
    Goad,
    Head,
    Water,
    Needle,
    Window,
    Club,
    Tooth,
    Eye
}


public static class BossDebuffFactory
{
    private static readonly Dictionary<BossDebuffType, Func<BossData, IBossDebuff>> debuffMap
        = new()
    {
            { BossDebuffType.Club, data => new ClubDebuff() },
            { BossDebuffType.Head, data => new HeartDebuff() },
            { BossDebuffType.Goad, data => new SpadeDebuff() },
            { BossDebuffType.Window, data => new DiaDebuff() },
            { BossDebuffType.Water, data => new DeleteDebuff() },
            { BossDebuffType.Needle, data => new HandDebuff() },
            { BossDebuffType.Tooth, data => new EvenDebuff() },
            { BossDebuffType.Eye, data => new OddDebuff() },


    };

    // BossData를 기반으로 디버프 생성
    public static IBossDebuff Create(BossData data)
    {
        var type = data.ToDebuffType();

        if (type.HasValue && debuffMap.TryGetValue(type.Value, out var creator))
        {
            return creator.Invoke(data);
        }

        Debug.LogWarning($"[BossDebuffFactory] 디버프 생성 실패: {data.debuffname}");
        return null;
    }
}



// bossDebuff string 타입 -> enum 열거형 타입으로 변환
public static class BossDataExtensions
{
    public static BossDebuffType? ToDebuffType(this BossData data)
    {
        if (Enum.TryParse<BossDebuffType>(data.debuffname, true, out var type))
            return type;

        Debug.LogWarning($"[BossData] 알 수 없는 디버프 타입: {data.debuffname}");
        return null;
    }
}
