using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EasyTransition;

public class InGameUiManager : MonoBehaviour
{
    public static InGameUiManager instance { get; private set; }

    public GameObject[] panel;
    private bool isScene = false;

    private void Awake()
    {
        instance = this;
    }
}
