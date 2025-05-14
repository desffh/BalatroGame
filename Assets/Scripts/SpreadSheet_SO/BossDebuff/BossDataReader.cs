using UnityEngine;
using GoogleSheetsToUnity;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public struct BossData
{
    public string blindname;
    
    public string debuffname;
    
    public int money;

    public string require;
    
    public int requireNum;
    
    public string blindImage;

    public string blindInfo;

    public BossData(string blindname, string debuff, int money, string require, int requirenum, string blindImage, string info)
    {
        this.blindname = blindname;
        this.debuffname = debuff;
        this.money = money;
        this.require = require;
        this.requireNum = requirenum;
        this.blindImage = blindImage;
        this.blindInfo = info;
    }
}

// ��ũ���ͺ� ������Ʈ �����
[CreateAssetMenu(fileName = "Reader", menuName = "Scriptable Object/BossDataReader", order = int.MaxValue)]


// ����ü Ÿ������ �����͸� �޾ƿ��� JokerData����ü Ÿ���� ����Ʈ�� �����Ѵ�
public class BossDataReader : JokerReaderBase
{
    [Header("���������Ʈ���� ������ ����ȭ �� ������Ʈ")]
    [SerializeField] public List<BossData> DataList = new List<BossData>();

    internal void UpdateStats(List<GSTU_Cell> list, int itemID)
    {
        string blindname = string.Empty;

        string debuffname = string.Empty;

        int money = 0;  

        string require = string.Empty;

        int requireNum = 0;

        string blindImage = string.Empty;

        string info = string.Empty;

        for (int i = 0; i < list.Count; i++)
        {
            switch (list[i].columnId)
            {
                case "BlindName":
                    {
                        blindname = list[i].value;
                        break;
                    }

                case "DebuffName":
                    {
                        debuffname = list[i].value;
                        break;
                    }
                case "Money":
                    {
                        if (!int.TryParse(list[i].value, out money))
                        {
                            Debug.LogError($"�߸��� money ��: {list[i].value}");
                        }
                        break;
                    }
                case "RequireString":
                    {
                        require = list[i].value;
                        break;
                    }
                case "RequireNum":
                    {
                        if (!int.TryParse(list[i].value, out requireNum))
                        {
                            Debug.LogError($"�߸��� requireNum ��: {list[i].value}");
                        }
                        break;
                    }
                case "BlindImage":
                    {
                        blindImage = list[i].value;
                        break;
                    }
                case "BlindInfo":
                    {
                        info = list[i].value;
                        break;
                    }
            }
        }

        DataList.Add(new BossData(blindname, debuffname, money, require, requireNum, blindImage, info));
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(BossDataReader))]
public class BossDataReaderEditor : Editor
{
    BossDataReader data;

    void OnEnable()
    {
        data = (BossDataReader)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("\n\n�������� ��Ʈ �о����");

        if (GUILayout.Button("������ �б�(API ȣ��)"))
        {
            data.DataList.Clear();
            UpdateStats(UpdateMethodOne);
        }
    }

    void UpdateStats(UnityAction<GstuSpreadSheet> callback, bool mergedCells = false)
    {
        Debug.Log("�������� ��Ʈ ������ ��û ����");

        SpreadsheetManager.Read(new GSTU_Search(data.associatedSheet, data.associatedWorksheet), callback, mergedCells);
    }

    void UpdateMethodOne(GstuSpreadSheet ss)
    {
        for (int i = data.START_ROW_LENGTH; i <= data.END_ROW_LENGTH; ++i)
        {
            data.UpdateStats(ss.rows[i], i);
        }

        EditorUtility.SetDirty(target);
    }
}
#endif
