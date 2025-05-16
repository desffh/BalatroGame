using System;
using System.Collections;
using System.Collections.Generic;
using GoogleSheetsToUnity;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public struct PlanetData
{
    public string name;
    public string require;
    public int chip;
    public int multiple;
    public string image;

    public PlanetData(string name, string require, int chip, int multiple, string image)
    {
        this.name = name;
        this.require = require;
        this.multiple = multiple;
        this.chip = chip;
        this.image = image;
    }
}

[CreateAssetMenu(fileName = "Reader", menuName = "Scriptable Object/PlanetDataReader", order = int.MaxValue)]


public class PlanetDataReader : PlanetReaderBase
{
    [Header("���������Ʈ���� ������ ����ȭ �� ������Ʈ")]
    [SerializeField] public List<PlanetData> DataList = new List<PlanetData>();

    internal void UpdateStats(List<GSTU_Cell> list, int itemID)
    {
        string name = string.Empty;
        string require = string.Empty;
        int chip = 0;
        int muliple = 0;
        string image = string.Empty;


        for (int i = 0; i < list.Count; i++)
        {
            switch (list[i].columnId)
            {
                case "PlanetName":
                    {
                        name = list[i].value;
                        break;
                    }
                case "Require":
                    {
                        require = list[i].value;
                        break;
                    }
                case "Chip":
                    {

                        if (!int.TryParse(list[i].value, out chip))
                        {
                            Debug.LogError($"�߸��� id ��: {list[i].value}");
                        }

                        break;
                    }
                case "Multiple":
                    {

                        if (!int.TryParse(list[i].value, out muliple))
                        {
                            Debug.LogError($"�߸��� id ��: {list[i].value}");
                        }

                        break;
                    }
                case "PlanetImage":
                    {
                        image = list[i].value;
                        break;
                    }
            }
        }

        DataList.Add(new PlanetData(name, require, chip, muliple, image));
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PlanetDataReader))]
public class PlanetItemDataReaderEditor : Editor
{
    PlanetDataReader data;

    void OnEnable()
    {
        data = (PlanetDataReader)target;
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