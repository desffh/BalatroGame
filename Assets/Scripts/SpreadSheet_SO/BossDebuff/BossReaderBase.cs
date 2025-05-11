using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossReaderBase : ScriptableObject
{
    [Header("��Ʈ�� �ּ�")][SerializeField] public string associatedSheet = "1Wj-NhHOCt8KN1YjZ6-hHP5KF8mozMdXT5zwoziEVQEc";
    [Header("�������� ��Ʈ�� ��Ʈ �̸�")][SerializeField] public string associatedWorksheet = "BossDebuff";
    [Header("�б� ������ �� ��ȣ")][SerializeField] public int START_ROW_LENGTH = 2;
    [Header("���� ������ �� ��ȣ")][SerializeField] public int END_ROW_LENGTH = 8;
}
