using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDebuff : ISystemDebuff
{
    // �ڵ� Ƚ�� 1�� ����

    public void SystemDebuff()
    {
        StateManager.Instance.handDeleteSetting.SetHand(1);
    }
}
