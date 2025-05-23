using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카드 이미지 설정 담당 
// 
// 런타임 시 이미지를 모두 배열에 저장해두고 필요할 때 키값으로 이미지 사용하기

public class CardSprites : Singleton<CardSprites>
{
    private Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();

    protected override void Awake()
    {
        base.Awake();

        LoadSprites();
    }

    private void LoadSprites()
    {
        // 강제 문자열 하드코딩 (Inspector 값 무시)
        string fullPath = "Textures/card_list_2d";

        Sprite[] sprites = Resources.LoadAll<Sprite>(fullPath);

        //Debug.Log($"[CardSprites] 총 {sprites.Length}개 스프라이트 로드 성공");

        foreach (var sprite in sprites)
        {
            spriteDict[sprite.name] = sprite;
            //Debug.Log($"[CardSprites] 로드됨: {sprite.name}");
        }
    }


    public Sprite Get(string name)
    {
        if (spriteDict.TryGetValue(name, out Sprite sprite))
            return sprite;

        Debug.LogWarning($"[CardSpriteStore] '{name}'에 해당하는 스프라이트 없음");
        return null;
    }
}
