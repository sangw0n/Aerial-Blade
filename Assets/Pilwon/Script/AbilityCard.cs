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
        texts[0].text = string.Format("{0} 강화", buff.buffName);
        texts[1].text = string.Format("기본 {0} + {1}% 증가", buff.buffName, buff.rate[buff.level] * 100);
        if (button.interactable)
        {
            texts[2].text = string.Format("{0} Gold", buff.price[buff.level]);
            texts[3].text = string.Format("[ Level {0} ]", buff.level);
        }
        else
        {
            texts[2].text = string.Format("Complete");
            texts[3].text = string.Format("[ Level {0} ]", buff.level + 1);
        }
    }

    public void ButtonCheck(bool isbool)
    {
        var buff = BuffManager.instance.buff[id];
        button.interactable = isbool;
    }
}
