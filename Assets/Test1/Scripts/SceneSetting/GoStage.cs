using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoStage : MonoBehaviour
{
    public void MoveStage()
    {
        ButtonManager.DestroySelf();
        HoldManager.DestroySelf();
        CardManager.DestroySelf();
        TextManager.DestroySelf();
        Round.DestroySelf();
        AnimationManager.DestroySelf();
        GameManager.DestroySelf();
        PokerManager.DestroySelf();
        ScoreManager.DestroySelf();
        HandDelete.DestroySelf();

        SceneLoaderUI.Instance.LoadSceneWithLoadingScreen("StageScene");
    }
}
