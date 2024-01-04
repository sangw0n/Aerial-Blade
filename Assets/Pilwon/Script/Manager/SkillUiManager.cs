using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUiManager : MonoBehaviour
{
    public static SkillUiManager instance;

    public Image skillCollTime_1;
    public Image skillCollTime_2;
    public Image skillCollTime_3;

    private void Awake()
    {
        instance = this;
    }
}
