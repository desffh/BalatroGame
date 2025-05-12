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


    };


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
