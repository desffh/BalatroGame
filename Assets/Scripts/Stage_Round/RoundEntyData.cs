using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[CreateAssetMenu(menuName = "Model/RoundEntyData")]
public class RoundEntyData : ScriptableObject
{
    [SerializeField] private int enty = 0;
    [SerializeField] private int round = 0;

    public int Enty => enty;
    public int Round => round;


    // ī��Ʈ ����
    public void ResetCounts()
    {
        enty = 0;
        round = 0;
    }

    // ��Ƽ ����
    public void UpEnty()
    {

        ++enty;
        
    }

    // ���� ����
    public void UpRound()
    {

        ++round;
        
    }
}
