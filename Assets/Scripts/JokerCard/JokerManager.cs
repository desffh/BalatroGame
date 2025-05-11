using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;


// 조커의 정보 + 스프라이트 구조체
[System.Serializable]
public class JokerTotalData
{
    // 조커 정보 (이름, 가격, 배수, 요구 조건, 조커 타입)
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

    // 조커 카드 버퍼
    [SerializeField] private JokerDataReader dataReader;

    // DataList(조커정보) + 스프라이트
    [SerializeField] public List<JokerTotalData> jokerBuffer;


    // |------------------------------------------------

    [SerializeField] MyJokerCard myjokercard;

    private void Awake() // 스프라이트 딕셔너리에 저장
    {
        // 조커 이미지 리스트
        Sprite[] loadedSprites = Resources.LoadAll<Sprite>("JokerSprites");

        foreach (Sprite sprite in loadedSprites)
        {
            spriteDict[sprite.name] = sprite; // 이름으로 매핑
        }

        // |---------------------------



    }


    private void Start()
    {
        SetupJokerBuffer();
    }

    // 조커를 리스트에 할당
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

            // JokerTotalData 구조체에 할당
            JokerTotalData totalData = new JokerTotalData(baseData, matchedSprite);

            // 리스트에 추가
            jokerBuffer.Add(totalData);
        }

        //Debug.Log("jokerBuffer 생성 완료! 총 개수: " + jokerBuffer.Count);
    }

    // |------------------------------------------------

    // 셔플
    public void ShuffleBuffer()
    {
        jokerBuffer = jokerBuffer.OrderBy(_ => Random.value).ToList();
    }


    // 정보 꺼내기
    public JokerTotalData PopData()
    {
        if (jokerBuffer.Count == 0)
        {
            Debug.Log("조커 버퍼에 조커가 없습니다");
        }

        var data = jokerBuffer[0];
        jokerBuffer.RemoveAt(0);
        return data;
    }

    // 정보 되돌리기 (판매 시 & 상점에서 반환 시)
    public void PushData(JokerTotalData data)
    {
        jokerBuffer.Add(data);
    }

    public void MyJokerReset()
    {
         myjokercard.ResetJokerCard();
    }

}
