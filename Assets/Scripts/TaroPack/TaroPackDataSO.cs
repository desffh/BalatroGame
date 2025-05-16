using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 타로 카드 팩 데이터 SO

[CreateAssetMenu(fileName = "TaroCardPackData", menuName = "CardPack/TaroPackData")]
public class TaroCardPackSO : ScriptableObject
{
    public string packName;

    public int cost;

    public int decCount;

    public int selectCount;

    public List<Sprite> planetPackImages;
}
