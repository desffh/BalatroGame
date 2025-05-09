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

    public bool ApplyEffect(List<Card> selectedCards, string currentHandType, StateManager stateManager, string jokerCategory, JokerCard myJoker)
    {
        if (isApplied == true)
            return false; // �̹� ����Ǿ����� �ƹ��͵� ���� ����

        // ������ Ƚ�� �߰�
        stateManager.UpCountDelete();

        AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);

        TextManager.Instance.UpdateText(4, stateManager.HandDelete.Delete);

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
            StateManager.Instance.DeCountDelete();

            TextManager.Instance.UpdateText(4, StateManager.Instance.HandDelete.Delete);
        }
    }

    // bool ���� �ʱ�ȭ �Ͽ� �ٽ� ��Ŀ ���� ����
    public void ResetEffect()
    {
        isApplied = false;
    }
}
