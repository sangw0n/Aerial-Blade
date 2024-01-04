using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour
{
    public void Exit()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
}
