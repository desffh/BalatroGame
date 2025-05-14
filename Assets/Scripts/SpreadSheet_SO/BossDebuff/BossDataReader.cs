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

// 스크립터블 오브젝트 만들기
[CreateAssetMenu(fileName = "Reader", menuName = "Scriptable Object/BossDataReader", order = int.MaxValue)]


// 구조체 타입으로 데이터를 받아오고 JokerData구조체 타입의 리스트에 저장한다
public class BossDataReader : JokerReaderBase
{
    [Header("스프레드시트에서 읽혀져 직렬화 된 오브젝트")]
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
                            Debug.LogError($"잘못된 money 값: {list[i].value}");
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
                            Debug.LogError($"잘못된 requireNum 값: {list[i].value}");
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

        GUILayout.Label("\n\n스프레드 시트 읽어오기");

        if (GUILayout.Button("데이터 읽기(API 호출)"))
        {
            data.DataList.Clear();
            UpdateStats(UpdateMethodOne);
        }
    }

    void UpdateStats(UnityAction<GstuSpreadSheet> callback, bool mergedCells = false)
    {
        Debug.Log("스프레드 시트 데이터 요청 시작");

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
