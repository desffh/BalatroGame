using UnityEngine;
using GoogleSheetsToUnity;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public struct JokerData
{
    public string name;
    public int cost;
    public int multiple;
    public string require;
    public JokerData(string name, int cost, int multiple, string require)
    {
        this.name = name;
        this.cost = cost;
        this.multiple = multiple;
        this.require = require;
    }
}

[CreateAssetMenu(fileName = "Reader", menuName = "Scriptable Object/JokerDataReader", order = int.MaxValue)]


// 구조체 타입으로 데이터를 받아오고 JokerData구조체 타입의 리스트에 저장한다
public class JokerDataReader : JokerReaderBase
{
    [Header("스프레드시트에서 읽혀져 직렬화 된 오브젝트")]
    [SerializeField] public List<JokerData> DataList = new List<JokerData>();

    internal void UpdateStats(List<GSTU_Cell> list, int itemID)
    {
        string name = string.Empty;
        int cost = 0;
        int muliple = 0;
        string require = string.Empty;


        for (int i = 0; i < list.Count; i++)
        {
            switch (list[i].columnId)
            {
                case "JokerName":
                    {
                        name = list[i].value;
                        break;
                    }
                case "Cost":
                    {

                        if (!int.TryParse(list[i].value, out cost))
                        {
                            Debug.LogError($"잘못된 id 값: {list[i].value}");
                        }

                        break;
                    }
                case "Multiply":
                    {

                        if (!int.TryParse(list[i].value, out muliple))
                        {
                            Debug.LogError($"잘못된 id 값: {list[i].value}");
                        }

                        break;
                    }
                case "Require":
                    {
                        require = list[i].value;
                        break;
                    }
            }
        }

        DataList.Add(new JokerData(name, cost, muliple, require));
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(JokerDataReader))]
public class JokerItemDataReaderEditor : Editor
{
    JokerDataReader data;

    void OnEnable()
    {
        data = (JokerDataReader)target;
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
