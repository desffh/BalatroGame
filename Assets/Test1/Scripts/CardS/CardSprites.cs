using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSprites : Singleton<CardSprites>
{
    [SerializeField] private string sheetName = "Textures/card_list_2d";

    private Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();

    protected override void Awake()
    {
        base.Awake();

        LoadSprites();
    }

    private void LoadSprites()
    {
        // ���� ���ڿ� �ϵ��ڵ� (Inspector �� ����)
        string fullPath = "Textures/card_list_2d";

        Sprite[] sprites = Resources.LoadAll<Sprite>(fullPath);

        //Debug.Log($"[CardSprites] �� {sprites.Length}�� ��������Ʈ �ε� ����");

        foreach (var sprite in sprites)
        {
            spriteDict[sprite.name] = sprite;
            //Debug.Log($"[CardSprites] �ε��: {sprite.name}");
        }
    }


    public Sprite Get(string name)
    {
        if (spriteDict.TryGetValue(name, out Sprite sprite))
            return sprite;

        Debug.LogWarning($"[CardSpriteStore] '{name}'�� �ش��ϴ� ��������Ʈ ����");
        return null;
    }
}
