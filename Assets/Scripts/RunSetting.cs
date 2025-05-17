using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class PokerUI
{
    public TextMeshProUGUI chipText;
    public TextMeshProUGUI multiText;
    public TextMeshProUGUI levelText;

    [HideInInspector]
    public int currentLevel = 1;
}

public class RunSetting : Singleton<RunSetting>
{
    // 족보 이름은 고정된 키로 사용
    [Header("Poker UI Group")]
    [SerializeField] private PokerUI highCardUI;
    [SerializeField] private PokerUI onePairUI;
    [SerializeField] private PokerUI twoPairUI;
    [SerializeField] private PokerUI tripleUI;
    [SerializeField] private PokerUI straightUI;
    [SerializeField] private PokerUI flushUI;
    [SerializeField] private PokerUI fullHouseUI;
    [SerializeField] private PokerUI fourCardUI;
    [SerializeField] private PokerUI straightFlushUI;

    // 초기 점수 세팅 (딕셔너리로)
    private readonly Dictionary<string, (int chip, int multi)> baseValues = new()
    {
        { "하이 카드", (5, 1) },
        { "페어", (10, 2) },
        { "투 페어", (20, 2) },
        { "트리플", (30, 3) },
        { "스트레이트", (30, 4) },
        { "플러시", (35, 4) },
        { "풀 하우스", (40, 4) },
        { "포 카드", (60, 7) },
        { "스트레이트 플러시", (100, 8) }
    };

    private Dictionary<string, PokerUI> uiMap;

    protected override void Awake()
    {
        base.Awake();

        // 이름으로 UI를 묶는 맵 구성
        uiMap = new Dictionary<string, PokerUI>
        {
            { "하이 카드", highCardUI },
            { "페어", onePairUI },
            { "투 페어", twoPairUI },
            { "트리플", tripleUI },
            { "스트레이트", straightUI },
            { "플러시", flushUI },
            { "풀 하우스", fullHouseUI },
            { "포 카드", fourCardUI },
            { "스트레이트 플러시", straightFlushUI },
        };
    }

    private void Start()
    {
        foreach (var kv in baseValues)
        {
            SetPokerUI(kv.Key, kv.Value.chip, kv.Value.multi);
        }
    }

    public void SetPokerUI(string pokerName, int chip, int multi)
    {
        if (!uiMap.ContainsKey(pokerName)) return;

        var ui = uiMap[pokerName];
        ui.chipText.text = $"{chip}";
        ui.multiText.text = $"{multi}";
        ui.levelText.text = $"Lv. {ui.currentLevel}";
    }

    public void UpgradePokerUI(string pokerName, int addChip, int addMulti)
    {
        if (!uiMap.ContainsKey(pokerName)) return;

        var ui = uiMap[pokerName];
        
        // 현재 값
        int currentChip = int.Parse(ui.chipText.text);      
        int currentMulti = int.Parse(ui.multiText.text);    


        // 증가 적용
        int newChip = currentChip + addChip;
        int newMulti = currentMulti + addMulti;
        ui.currentLevel++;

        // 텍스트 갱신
        ui.chipText.text = $"{newChip}";
        ui.multiText.text = $"{newMulti}";
        ui.levelText.text = $"Lv. {ui.currentLevel}";

    }

    public void ResetSetting()
    {
        foreach (var kv in baseValues)
        {
            var ui = uiMap[kv.Key];
            ui.currentLevel = 1;
            SetPokerUI(kv.Key, kv.Value.chip, kv.Value.multi);
        }
    }
}
