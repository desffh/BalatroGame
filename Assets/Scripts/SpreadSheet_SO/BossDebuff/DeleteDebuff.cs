using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteDebuff : ISystemDebuff
{
    // ������ Ƚ�� 0���� ����

    public void SystemDebuff()
    {
        StateManager.Instance.handDeleteSetting.SetDelete(0);
    }
}
