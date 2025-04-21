using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ��Ŀ�� ���� + ��������Ʈ ����ü
[System.Serializable]
public struct JokerTotalData
{
    // ��Ŀ ���� 
    public JokerData baseData; 
    
    public Sprite image;

    public JokerTotalData(JokerData data, Sprite sprite)
    {
        baseData = data;
        image = sprite;
    }
}


public class JokerManager : MonoBehaviour
{

    private Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();

    // |------------------------------------------------

    // ��Ŀ ī�� ����
    [SerializeField] private JokerDataReader dataReader;

    // DataList(��Ŀ����) + ��������Ʈ
    [SerializeField] public List<JokerTotalData> jokerBuffer;


    private void Awake() // ��������Ʈ ��ųʸ��� ����
    {
        // ��Ŀ �̹��� ����Ʈ
        Sprite[] loadedSprites = Resources.LoadAll<Sprite>("JokerSprites");

        foreach (Sprite sprite in loadedSprites)
        {
            spriteDict[sprite.name] = sprite; // �̸����� ����
        }
    }

    private void Start()
    {
        SetupJokerBuffer();
    }

    // ��Ŀ�� ����Ʈ�� �Ҵ�
    public void SetupJokerBuffer()
    {
        if (jokerBuffer == null)
        {
            jokerBuffer = new List<JokerTotalData>(dataReader.DataList.Count);
        }
        else
        {
            jokerBuffer.Clear();
        }

        for (int i = 0; i < dataReader.DataList.Count; i++)
        {
            JokerData baseData = dataReader.DataList[i];
         
            Sprite matchedSprite = null;

            if (spriteDict.TryGetValue(baseData.name, out Sprite found))
            {
                matchedSprite = found;
            }

            // JokerTotalData ����ü�� �Ҵ�
            JokerTotalData totalData = new JokerTotalData(baseData, matchedSprite);

            // ����Ʈ�� �߰�
            jokerBuffer.Add(totalData);
        }

        Debug.Log("jokerBuffer ���� �Ϸ�! �� ����: " + jokerBuffer.Count);
    }
}
