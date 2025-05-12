using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public System.Action OnStageChanged; // �������� Ŭ���� ��

    [SerializeField] private bool hold;

    [SerializeField] ScreenPopupManager screenManager;

    [SerializeField] StageButton stageButton;

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
        
        StateManager.Instance.moneyViewSetting.Add(stageButton.blind.money);
    }


}
