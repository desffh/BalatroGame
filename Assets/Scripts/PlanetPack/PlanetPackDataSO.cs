using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �༺ ī�� �� ������ SO

[CreateAssetMenu(fileName = "PlanetCardPackData", menuName = "CardPack/PlanetPackData")]
public class PlanetCardPackSO : ScriptableObject
{
    public string packName;

    public int cost;

    public List<Sprite> planetPackImages;
}
