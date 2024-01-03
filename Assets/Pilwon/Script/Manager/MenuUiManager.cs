using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuUiManager : MonoBehaviour
{
    public static MenuUiManager instance;

    [SerializeField] private UI[] MenuUi;

    private void Awake()
    {
        instance = this;
        MenuUi = GetComponentsInChildren<UI>(true);
    }

    public void Show(Vector3 pos, Ui ui)
    {
        RectTransform rect = MenuUi[(int)ui].GetComponent<RectTransform>();
        rect.DOLocalMove(pos, 0.75f, false);
    }

    public void Hide(Vector3 pos, Ui ui)
    {
        RectTransform rect = MenuUi[(int)ui].GetComponent<RectTransform>();
        rect.DOLocalMove(pos, 0.75f, false);
    }

    public IEnumerator NoticePanel(Ui ui, float time)
    {
        RectTransform rect = MenuUi[(int)ui].GetComponent<RectTransform>();
        Vector3 orginPos = rect.transform.localPosition;

        Show(new Vector3(0, 447, 0), ui);
        yield return new WaitForSeconds(time);
        Hide(orginPos, ui);
    }
}
