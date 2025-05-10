using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

// ���ӿ� ���Ǵ� �ڵ�, ������ / �Ӵ� / Ĩ, ��� �� �����ͼ� ȣ���ϴ� ���
public class StateManager : Singleton<StateManager>
{
    [Header("Model")] // �� -> �����Ϳ��� SO �Ҵ�
    [SerializeField] private HandDeleteData handDeleteData;
    [SerializeField] private MultipleChipData multipleChipData;
    [SerializeField] private Money moneyData;

    [Header("View")] // �� -> �����Ϳ��� UI �Ҵ�
    [SerializeField] private HandDeleteView handDeleteView;
    [SerializeField] private MultipleChipView MultipleChipView;
    [SerializeField] private MoneyView moneyView;


    // ��������
    private HandDeletePresenter handDeletePresenter;
    private MultipleChipPresenter MultipleChipPresenter;
    private MoneyPresenter MoneyPresenter;


    // �������̽�
    public IHandDeleteSetting handDeleteSetting => handDeletePresenter;
    public IMultiplyChipSetting multiplyChipSetting => MultipleChipPresenter;

    public IMoneySetting moneyViewSetting => MoneyPresenter;

    // �ܺο��� �������̽��� ������ ȣ�� ����
    // |-------------------------------

    protected override void Awake()
    {
        base.Awake();

        handDeletePresenter = new HandDeletePresenter(handDeleteData, handDeleteView);
        MultipleChipPresenter = new MultipleChipPresenter(multipleChipData, MultipleChipView);
        MoneyPresenter = new MoneyPresenter(moneyData, moneyView);
    }

    private void Start()
    {
        handDeleteSetting.Reset();
        multiplyChipSetting.Reset();
        moneyViewSetting.Reset();
    }

}
