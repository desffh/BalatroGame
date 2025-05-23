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
        AnimationManager.DestroySelf();
        GameManager.DestroySelf();
        PokerManager.DestroySelf();
        ScoreManager.DestroySelf();
        StateManager.DestroySelf();
        RunSetting.DestroySelf();
        StageManager.DestroySelf();

        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        SceneLoaderUI.Instance.LoadSceneWithLoadingScreen("StageScene");
    }
}
