using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoStage : MonoBehaviour
{

    public void MoveStage()
    {
        SceneLoaderUI.Instance.LoadSceneWithLoadingScreen("StageScene");
    }

}
