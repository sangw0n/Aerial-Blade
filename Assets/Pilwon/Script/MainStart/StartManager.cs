using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EasyTransition;

public class StartManager : MonoBehaviour
{
    public TransitionSettings transition;
    private bool isClick = false;

    private void Update()
    {
        if(Input.anyKeyDown && !isClick)
        {
            isClick = true;
            TransitionManager.Instance().Transition("MainCube", transition, 1.0f);
        }
    }
}
