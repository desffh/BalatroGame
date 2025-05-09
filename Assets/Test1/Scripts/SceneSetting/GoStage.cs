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
        StateManager.DestroySelf();

        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        SceneLoaderUI.Instance.LoadSceneWithLoadingScreen("StageScene");
    }
}
