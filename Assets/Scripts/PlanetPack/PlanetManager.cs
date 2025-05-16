using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


// ��Ŀ�� ���� + ��������Ʈ ����ü
[System.Serializable]
public class PlanetTotalData
{
    public PlanetData baseData;

    public Sprite image;

    public PlanetTotalData(PlanetData data, Sprite sprite)
    {
        baseData = data;
        image = sprite;
    }
}


public class PlanetManager : MonoBehaviour
{
    private Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();

    // |------------------------------------------------

    // �༺ ī�� ����
    [SerializeField] private PlanetDataReader dataReader;

    // �༺ ������ ����Ʈ
    [SerializeField] public List<PlanetTotalData> planetdata;





    private void Awake()
    {
        ImageBuild();
    }

    private void Start()
    {
        SetupPlanetBuffer();
    }



    private void ImageBuild()
    {
        // �༺ �̹��� ����Ʈ
        Sprite[] loadedSprites = Resources.LoadAll<Sprite>("Planets");

        foreach (Sprite sprite in loadedSprites)
        {
            spriteDict[sprite.name] = sprite; // �̸����� ����
        }
    }


    // �༺ �����͸� ����Ʈ�� �Ҵ� (���� ����)
    public void SetupPlanetBuffer()
    {
        planetdata.Clear();

        if (planetdata == null)
        {
            planetdata = new List<PlanetTotalData>(dataReader.DataList.Count);
        }
        else
        {
            planetdata.Clear();
        }


        for (int i = 0; i < dataReader.DataList.Count; i++)
        {
            PlanetData baseData = dataReader.DataList[i];

            Sprite matchedSprite = null;

            // �̹��� �Ҵ�
            
            if (spriteDict.TryGetValue(baseData.image, out Sprite found))
            {
                matchedSprite = found;
            }

            // �̹����� ��ġ�ϴ� matchedSprite�� ��������

            PlanetTotalData totalData = new PlanetTotalData(baseData, matchedSprite);

            // ����Ʈ�� �߰�
            planetdata.Add(totalData);
        }

        Debug.Log("�༺Buffer ���� �Ϸ�! �� ����: " + planetdata.Count);
    }


    // |----

    // ����
    public void ShuffleBuffer()
    {
        planetdata = planetdata.OrderBy(_ => Random.value).ToList();
    }



    // �༺ī�� ���� �� �༺ī�� ���� ��ȯ
    public PlanetTotalData PopData()
    {
        if (planetdata.Count == 0)
        {
            Debug.Log("�༺ī�� ���� ��");
        }

        PlanetTotalData data = planetdata[0];
        planetdata.RemoveAt(0);

        return data;
    }


    // ���� �ǵ����� (ī�� ���� �ǳʶٱ� ��)
    public void PushData(PlanetTotalData data)
    {
        planetdata.Add(data);
    }
}
