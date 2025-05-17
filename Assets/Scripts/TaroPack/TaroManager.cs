using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


// Ÿ���� ���� + ��������Ʈ ����ü
[System.Serializable]
public class TaroTotalData
{
    public TaroData baseData;

    public Sprite image;

    public TaroTotalData(TaroData data, Sprite sprite)
    {
        baseData = data;
        image = sprite;
    }
}

public class TaroManager : MonoBehaviour
{
    private Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();

    // Ÿ�� ī�� ����
    [SerializeField] private TaroDataReader dataReader;

    // Ÿ�� ������ ����Ʈ
    [SerializeField] public List<TaroTotalData> tarodata;





    private void Awake()
    {
        ImageBuild();
    }

    private void Start()
    {
        SetupTaroBuffer();
    }

    private void ImageBuild()
    {
        Sprite[] loadedSprites = Resources.LoadAll<Sprite>("Tarot");

        foreach (Sprite sprite in loadedSprites)
        {
            spriteDict[sprite.name] = sprite;
        }
    }


    public void SetupTaroBuffer()
    {
        tarodata.Clear();

        if (tarodata == null)
        {
            tarodata = new List<TaroTotalData>(dataReader.DataList.Count);
        }
        else
        {
            tarodata.Clear();
        }


        for (int i = 0; i < dataReader.DataList.Count; i++)
        {
            TaroData baseData = dataReader.DataList[i];

            Sprite matchedSprite = null;

            // �̹��� �Ҵ�

            if (spriteDict.TryGetValue(baseData.image, out Sprite found))
            {
                matchedSprite = found;
            }

            // �̹����� ��ġ�ϴ� matchedSprite�� ��������

            TaroTotalData totalData = new TaroTotalData(baseData, matchedSprite);

            // ����Ʈ�� �߰�
            tarodata.Add(totalData);
        }
    }

    // |----

    // ����
    public void ShuffleBuffer()
    {
        tarodata = tarodata.OrderBy(_ => Random.value).ToList();
    }

    public TaroTotalData PopData()
    {
        if (tarodata.Count == 0)
        {
            Debug.Log("�༺ī�� ���� ��");
        }

        TaroTotalData data = tarodata[0];
        tarodata.RemoveAt(0);

        return data;
    }


    // ���� �ǵ����� (ī�� ���� �ǳʶٱ� ��)
    public void PushData(TaroTotalData data)
    {
        tarodata.Add(data);
    }
}
