using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonManager : MonoBehaviour
{
    private bool isDevMode = false;
    public Button devMode;

    public void DataDestory()
    { 
        PSoundManager.instance.PlaySfx(PSoundManager.sfx.Select);
        PlayerPrefs.DeleteAll();
    }

    public void DevMode()
    {
        devMode.interactable = false;
        StatManager.instance.AttUpgrade(6);
        StatManager.instance.HealthUpgrade(10); 
        GameManager.instance.gold += 10000;
        PSoundManager.instance.PlaySfx(PSoundManager.sfx.Select);
    }

    public void DecoPanel()
    {
        PSoundManager.instance.PlaySfx(PSoundManager.sfx.Select);
        MenuUiManager.instance.Show(new Vector3(572, 0, 0), 6);
    }

    public void Cancel()
    {
        PSoundManager.instance.PlaySfx(PSoundManager.sfx.Select);
        MenuUiManager.instance.Show(new Vector3(1350, 0, 0), 6);
    }
}


