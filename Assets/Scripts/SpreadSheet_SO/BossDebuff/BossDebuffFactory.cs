using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossDebuffType
{
    Hook,
    Club,
    Goad,
    Water,
    Needle,
    Head,
    Tooth
}


public static class BossDebuffFactory
{
    private static readonly Dictionary<BossDebuffType, Func<BossData, IBossDebuff>> debuffMap
        = new()
    {
            { BossDebuffType.Club, data => new ClubDebuff() },

    };

    // BossData�� ������� ����� ����
    public static IBossDebuff Create(BossData data)
    {
        var type = data.ToDebuffType();

        if (type.HasValue && debuffMap.TryGetValue(type.Value, out var creator))
        {
            return creator.Invoke(data);
        }

        Debug.LogWarning($"[BossDebuffFactory] ����� ���� ����: {data.debuffname}");
        return null;
    }
}



// bossDebuff string Ÿ�� -> enum ������ Ÿ������ ��ȯ
public static class BossDataExtensions
{
    public static BossDebuffType? ToDebuffType(this BossData data)
    {
        if (Enum.TryParse<BossDebuffType>(data.debuffname, true, out var type))
            return type;

        Debug.LogWarning($"[BossData] �� �� ���� ����� Ÿ��: {data.debuffname}");
        return null;
    }
}
