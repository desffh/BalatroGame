using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDebuff : ISystemDebuff
{
    // 핸드 횟수 1로 변경

    public void SystemDebuff()
    {
        StateManager.Instance.handDeleteSetting.SetHand(1);
    }
}
