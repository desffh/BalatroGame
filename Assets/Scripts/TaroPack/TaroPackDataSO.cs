using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ÿ�� ī�� �� ������ SO

[CreateAssetMenu(fileName = "TaroCardPackData", menuName = "CardPack/TaroPackData")]
public class TaroCardPackSO : ScriptableObject
{
    public string packName;

    public int cost;

    public int decCount;

    public int selectCount;

    public List<Sprite> planetPackImages;
}
