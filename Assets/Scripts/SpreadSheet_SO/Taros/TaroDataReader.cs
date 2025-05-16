using System;
using System.Collections;
using System.Collections.Generic;
using GoogleSheetsToUnity;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public struct TaroData
{
    public string name;

    public string require;

    public string image;

    public TaroData(string name, string require, string image)
    {
        this.name = name;
        this.require = require;
        this.image = image;
    }
}


[CreateAssetMenu(fileName = "Reader", menuName = "Scriptable Object/TaroDataReader", order = int.MaxValue)]


public class TaroDataReader : TaroReaderBase
{
    [Header("스프레드시트에서 읽혀져 직렬화 된 오브젝트")]
    [SerializeField] public List<TaroData> DataList = new List<TaroData>();

    internal void UpdateStats(List<GSTU_Cell> list, int itemID)
    {
        string name = string.Empty;
        string require = string.Empty;
        string image = string.Empty;


        for (int i = 0; i < list.Count; i++)
        {
            switch (list[i].columnId)
            {
                case "TaroName":
                    {
                        name = list[i].value;
                        break;
                    }
                case "Suit":
                    {
                        require = list[i].value;
                        break;
                    }
                case "TaroImage":
                    {
                        image = list[i].value;
                        break;
                    }
            }
        }

        DataList.Add(new TaroData(name, require, image));
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TaroDataReader))]
public class TaroItemDataReaderEditor : Editor
{
    TaroDataReader data;

    void OnEnable()
    {
        data = (TaroDataReader)target;
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