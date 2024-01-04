using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTransition;

public class SceneMoveButton : MonoBehaviour
{
    public TransitionSettings transition;

    public void GameQuit()
    {
        TransitionManager.Instance().Transition("MainCube", transition, 0);
    }

    public void GameRetry()
    {
        TransitionManager.Instance().Transition(GameManager.instance.inStageCount + 2, transition, 0);
    }
}
