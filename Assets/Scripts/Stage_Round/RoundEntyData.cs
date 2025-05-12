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


    // 카운트 리셋
    public void ResetCounts()
    {
        enty = 0;
        round = 0;
    }

    // 엔티 증가
    public void UpEnty()
    {

        ++enty;
        
    }

    // 라운드 증가
    public void UpRound()
    {

        ++round;
        
    }
}
