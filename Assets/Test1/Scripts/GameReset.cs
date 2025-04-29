using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReset : MonoBehaviour
{
    [SerializeField] Round round;

    [SerializeField] Money money;

    public void GameResets()
    {
        SoundManager.Instance.ButtonClick();

        round.RoundReset();
        money.ReSetTotalMoney();
        money.MoneyUpdate();
    }
}
