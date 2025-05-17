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
    // ���� �̸��� ������ Ű�� ���
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

    // �ʱ� ���� ���� (��ųʸ���)
    private readonly Dictionary<string, (int chip, int multi)> baseValues = new()
    {
        { "���� ī��", (5, 1) },
        { "���", (10, 2) },
        { "�� ���", (20, 2) },
        { "Ʈ����", (30, 3) },
        { "��Ʈ����Ʈ", (30, 4) },
        { "�÷���", (35, 4) },
        { "Ǯ �Ͽ콺", (40, 4) },
        { "�� ī��", (60, 7) },
        { "��Ʈ����Ʈ �÷���", (100, 8) }
    };

    private Dictionary<string, PokerUI> uiMap;

    protected override void Awake()
    {
        base.Awake();

        // �̸����� UI�� ���� �� ����
        uiMap = new Dictionary<string, PokerUI>
        {
            { "���� ī��", highCardUI },
            { "���", onePairUI },
            { "�� ���", twoPairUI },
            { "Ʈ����", tripleUI },
            { "��Ʈ����Ʈ", straightUI },
            { "�÷���", flushUI },
            { "Ǯ �Ͽ콺", fullHouseUI },
            { "�� ī��", fourCardUI },
            { "��Ʈ����Ʈ �÷���", straightFlushUI },
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
        
        // ���� ��
        int currentChip = int.Parse(ui.chipText.text);      
        int currentMulti = int.Parse(ui.multiText.text);    


        // ���� ����
        int newChip = currentChip + addChip;
        int newMulti = currentMulti + addMulti;
        ui.currentLevel++;

        // �ؽ�Ʈ ����
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
