using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Ui { AbilityPanel, DungeonPanel, GoldPanel}

public class UI : MonoBehaviour
{
    public Ui ui;
    private TMP_Text text;
    public Button buyButton;

    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
        if(ui == Ui.AbilityPanel) buyButton = GetComponentInChildren<Button>();
    }

    private void LateUpdate()
    {
        switch(ui)
        {
            case Ui.GoldPanel:
                text.text = string.Format("{0:N0}", GameManager.instance.gold);
                break;
        }
    }
}
