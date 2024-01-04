using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using EasyTransition;

public class Dungeon : MonoBehaviour
{
    public TransitionSettings transition;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text buttonText;

    private string bossName;
    private int index;

    public void Init(BossData bossData, int index)
    {
        this.index = index;
        bossName = bossData.bossName;
        cardName.text = "보스 " + bossName;
        icon.sprite = bossData.sprite;

        if (index == 3 && GameManager.instance.clearStageIndex < CubeManager.instance.cube.Length - 2)
        {
            button.interactable = false;
            buttonText.text = string.Format("Stage : {0} / {1}", GameManager.instance.clearStageIndex, CubeManager.instance.cube.Length - 1);
        }
        else if (index != 3)
        {
            button.interactable = true;
            buttonText.text = "도전 가능!";
        }
    }


    public void SceneMove()
    {   
        GameManager.instance.inStageCount = index;
        TransitionManager.Instance().Transition("Cube " + index, transition, 0);
    }
}
