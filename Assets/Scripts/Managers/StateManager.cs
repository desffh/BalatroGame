using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

// 게임에 사용되는 핸드, 버리기 / 머니 / 칩, 배수 를 가져와서 호출하는 담당
public class StateManager : Singleton<StateManager>
{
    [Header("Model")] // 모델 -> 에디터에서 SO 할당
    [SerializeField] private HandDeleteData handDeleteData;
    [SerializeField] private MultipleChipData multipleChipData;
    [SerializeField] private Money moneyData;

    [Header("View")] // 뷰 -> 에디터에서 UI 할당
    [SerializeField] private HandDeleteView handDeleteView;
    [SerializeField] private MultipleChipView MultipleChipView;
    [SerializeField] private MoneyView moneyView;


    // 프레젠터
    private HandDeletePresenter handDeletePresenter;
    private MultipleChipPresenter MultipleChipPresenter;
    private MoneyPresenter MoneyPresenter;


    // 인터페이스
    public IHandDeleteSetting handDeleteSetting => handDeletePresenter;
    public IMultiplyChipSetting multiplyChipSetting => MultipleChipPresenter;

    public IMoneySetting moneyViewSetting => MoneyPresenter;

    // 외부에서 인터페이스로 접근해 호출 가능
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
