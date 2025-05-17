using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


// 타로의 정보 + 스프라이트 구조체
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

    // 타로 카드 버퍼
    [SerializeField] private TaroDataReader dataReader;

    // 타로 데이터 리스트
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

            // 이미지 할당

            if (spriteDict.TryGetValue(baseData.image, out Sprite found))
            {
                matchedSprite = found;
            }

            // 이미지와 일치하는 matchedSprite도 보내야함

            TaroTotalData totalData = new TaroTotalData(baseData, matchedSprite);

            // 리스트에 추가
            tarodata.Add(totalData);
        }
    }

    // |----

    // 셔플
    public void ShuffleBuffer()
    {
        tarodata = tarodata.OrderBy(_ => Random.value).ToList();
    }

    public TaroTotalData PopData()
    {
        if (tarodata.Count == 0)
        {
            Debug.Log("행성카드 버퍼 빔");
        }

        TaroTotalData data = tarodata[0];
        tarodata.RemoveAt(0);

        return data;
    }


    // 정보 되돌리기 (카드 선택 건너뛰기 시)
    public void PushData(TaroTotalData data)
    {
        tarodata.Add(data);
    }
}
