using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteDebuff : ISystemDebuff
{
    // 버리기 횟수 0으로 변경

    public void SystemDebuff()
    {
        StateManager.Instance.handDeleteSetting.SetDelete(0);
    }
}
