using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Ui { AttAbilityPanel, AttSpeedtAbilityPanel, HealthtAbilityPanel, DungeonPanel, GoldPanel, DevNoticePanel, DocsPanel}

public class UI : MonoBehaviour
{
    public Ui ui;
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {
        switch(ui)
        {
            case Ui.GoldPanel:
                text.text = string.Format("{0:N0}", GameManager.instance.gold);
                break;
        }
    }   
}
