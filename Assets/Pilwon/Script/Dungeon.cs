using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Dungeon : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text cardName;

    private string bossName;
    private int index;

    public void Init(BossData bossData, int index)
    {
        bossName = bossData.bossName;
        this.index = index;

        icon.sprite = bossData.sprite;

    }

    public void SceneMove()
    {
        GameManager.instance.inStageCount = index;
        SceneManager.LoadScene("Cube " + index);
    }
}
