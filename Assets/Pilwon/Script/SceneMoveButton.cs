using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTransition;

public class SceneMoveButton : MonoBehaviour
{
    public TransitionSettings transition;

    public void GameQuit()
    {
        Time.timeScale = 1;
        TransitionManager.Instance().Transition("MainCube", transition, 0);
    }

    public void GameRetry()
    {
        Time.timeScale = 1;
        TransitionManager.Instance().Transition("Cube " + GameManager.instance.inStageCount, transition, 0);
    }
}
