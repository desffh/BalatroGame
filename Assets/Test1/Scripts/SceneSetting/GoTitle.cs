using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoTitle : MonoBehaviour
{
    public void MoveStage()
    {
        SceneLoaderUI.Instance.LoadSceneWithLoadingScreen("TitleScene");
    }
}
