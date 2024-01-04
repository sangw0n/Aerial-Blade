using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuUiManager : MonoBehaviour
{
    public static MenuUiManager instance;

    public UI[] MenuUi;

    private void Awake()
    {
        instance = this;
        MenuUi = GetComponentsInChildren<UI>(true);
    }

    public void Show(Vector3 pos, int uiNum)
    {

        RectTransform rect = MenuUi[uiNum].GetComponent<RectTransform>();
        rect.DOLocalMove(pos, 0.75f, false);
    }

    public void Hide(Vector3 pos, int uiNum)
    {

        RectTransform rect = MenuUi[uiNum].GetComponent<RectTransform>();
        rect.DOLocalMove(pos, 0.75f, false);
    }
}
