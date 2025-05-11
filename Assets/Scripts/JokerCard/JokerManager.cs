using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;


// ��Ŀ�� ���� + ��������Ʈ ����ü
[System.Serializable]
public class JokerTotalData
{
    // ��Ŀ ���� (�̸�, ����, ���, �䱸 ����, ��Ŀ Ÿ��)
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


    // |------------------------------------------------

    [SerializeField] MyJokerCard myjokercard;

    private void Awake() // ��������Ʈ ��ųʸ��� ����
    {
        // ��Ŀ �̹��� ����Ʈ
        Sprite[] loadedSprites = Resources.LoadAll<Sprite>("JokerSprites");

        foreach (Sprite sprite in loadedSprites)
        {
            spriteDict[sprite.name] = sprite; // �̸����� ����
        }

        // |---------------------------



    }


    private void Start()
    {
        SetupJokerBuffer();
    }

    // ��Ŀ�� ����Ʈ�� �Ҵ�
    public void SetupJokerBuffer()
    {
        jokerBuffer.Clear();

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

        //Debug.Log("jokerBuffer ���� �Ϸ�! �� ����: " + jokerBuffer.Count);
    }

    // |------------------------------------------------

    // ����
    public void ShuffleBuffer()
    {
        jokerBuffer = jokerBuffer.OrderBy(_ => Random.value).ToList();
    }


    // ���� ������
    public JokerTotalData PopData()
    {
        if (jokerBuffer.Count == 0)
        {
            Debug.Log("��Ŀ ���ۿ� ��Ŀ�� �����ϴ�");
        }

        var data = jokerBuffer[0];
        jokerBuffer.RemoveAt(0);
        return data;
    }

    // ���� �ǵ����� (�Ǹ� �� & �������� ��ȯ ��)
    public void PushData(JokerTotalData data)
    {
        jokerBuffer.Add(data);
    }

    public void MyJokerReset()
    {
         myjokercard.ResetJokerCard();
    }

}
