using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �Ϲ� ��Ŀ : ������ Ƚ�� �߰� ī��
public class PlusDeleteEffect : IJokerEffect, IExitEffect, IResettableEffect
{
    private string targetSuit;  // �Ϲ� ��Ŀ
    private int multiply;
    private string type; // Ÿ��

    private bool isApplied = false; // 1ȸ ���� ���� �÷���

    public PlusDeleteEffect(string suit, int bonus, string category)
    {
        targetSuit = suit;
        this.multiply = bonus;
        this.type = category;
    }

    public bool ApplyEffect(JokerEffectContext context)
    {
        var stateManager = context.StateManager;
        var myJoker = context.MyJoker;


        if (isApplied == true)
            return false; // �̹� ����Ǿ����� �ƹ��͵� ���� ����

        // ������ Ƚ�� �߰�

        stateManager.handDeleteSetting.PlusDelete();

        AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);

        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-CheckCard");

        ShowJokerRankText showJokerRankText = myJoker.GetComponent<ShowJokerRankText>();

        showJokerRankText.OnSettingDelete(myJoker.currentData.baseData.multiple);

        isApplied = true; // �� ���� ����ǵ��� �÷��� ����

        return true;
    }

    // ��Ŀ ī�� �Ǹ� �� ȣ��
    public void ExitEffect(JokerCard myJoker)
    {
        if(isApplied)
        {
            // ������ Ƚ�� ����

            StateManager.Instance.handDeleteSetting.MinusDelete();
        }
    }

    // bool ���� �ʱ�ȭ �Ͽ� �ٽ� ��Ŀ ���� ����
    public void ResetEffect()
    {
        isApplied = false;
    }
}
