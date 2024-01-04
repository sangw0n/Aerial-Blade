using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityCard : MonoBehaviour
{
    [SerializeField] private TMP_Text[] texts;
    [SerializeField] private Button button;
    [SerializeField] private int id;

    private void Awake()
    {
        texts = GetComponentsInChildren<TMP_Text>();
        button = GetComponentInChildren<Button>();
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        var buff = BuffManager.instance.buff[id];
        texts[0].text = string.Format("{0} °­È­", buff.buffName);
        if (button.interactable)
        {
            texts[1].text = string.Format("{0} Gold", buff.price[buff.level]);
            texts[2].text = string.Format("[ Level {0} ]", buff.level);
        }
        else
        {
            texts[1].text = string.Format("Complete");
            texts[2].text = string.Format("[ Level {0} ]", buff.level + 1);
        }
    }

    public void ButtonCheck(bool isbool)
    {
        var buff = BuffManager.instance.buff[id];
        button.interactable = isbool;
    }
}
