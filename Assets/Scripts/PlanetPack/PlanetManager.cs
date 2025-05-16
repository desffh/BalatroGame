using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


// 조커의 정보 + 스프라이트 구조체
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

    // 행성 카드 버퍼
    [SerializeField] private PlanetDataReader dataReader;

    // 행성 데이터 리스트
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
        // 행성 이미지 리스트
        Sprite[] loadedSprites = Resources.LoadAll<Sprite>("Planets");

        foreach (Sprite sprite in loadedSprites)
        {
            spriteDict[sprite.name] = sprite; // 이름으로 매핑
        }
    }


    // 행성 데이터를 리스트에 할당 (원본 보존)
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

            // 이미지 할당
            
            if (spriteDict.TryGetValue(baseData.image, out Sprite found))
            {
                matchedSprite = found;
            }

            // 이미지와 일치하는 matchedSprite도 보내야함

            PlanetTotalData totalData = new PlanetTotalData(baseData, matchedSprite);

            // 리스트에 추가
            planetdata.Add(totalData);
        }

        Debug.Log("행성Buffer 생성 완료! 총 개수: " + planetdata.Count);
    }


    // |----

    // 셔플
    public void ShuffleBuffer()
    {
        planetdata = planetdata.OrderBy(_ => Random.value).ToList();
    }



    // 행성카드 구매 시 행성카드 정보 반환
    public PlanetTotalData PopData()
    {
        if (planetdata.Count == 0)
        {
            Debug.Log("행성카드 버퍼 빔");
        }

        PlanetTotalData data = planetdata[0];
        planetdata.RemoveAt(0);

        return data;
    }


    // 정보 되돌리기 (카드 선택 건너뛰기 시)
    public void PushData(PlanetTotalData data)
    {
        planetdata.Add(data);
    }
}
