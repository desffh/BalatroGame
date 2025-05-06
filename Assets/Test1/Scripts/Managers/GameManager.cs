using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public System.Action OnStageChanged; // �������� Ŭ���� ��

    [SerializeField] private bool hold;

    [SerializeField] ScreenPopupManager screenManager;

    protected override void Awake()
    {
        base.Awake();
        OnStageChanged += CardManager.Instance.SetupNextStage; // ���⼭ �̺�Ʈ ���
    }

    // |-�������� �÷��� ���� (��������1)-----------------------------------------------|

    [SerializeField] bool playState;

    [SerializeField]
    public bool PlayState
    {
        get { return playState; }
    }

    [SerializeField] Money money;

    public void PlayOn()
    {
        //Debug.Log("PlayOn ȣ��");

        //playState = true;

    }

    public void PlayOff()
    {
        //Debug.Log("PlayOff ȣ��");

        //playState = false;

        StartCoroutine(screenManager.OnClearPanel());

        // ����!!!!!!!! ��� �˾��� �߰� ����� �� �Ӵ� �߰��� 
        money.AddMoney(Round.Instance.Money);
        money.MoneyUpdate();
    }


}
