using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New JokerData", menuName = "Scriptable Object/JokerData")]


public class JokerDataSO : ScriptableObject
{
    public string jokerName;
    public int cost;
    public int multiple;
    public string require;
}

