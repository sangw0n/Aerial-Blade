using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Buff
{
    public string buffName;
    public string docs;
    public Ability ability;

    public float[] rate;
    public int[] price;
    public int level = 1;
}

public class BuffManager : MonoBehaviour
{
    public static BuffManager instance { get; private set; }
    public Buff[] buff;

    private void Awake()
    {
        instance = this;
    }

    public void BuffBuyClick(int index)
    {
        if (GameManager.instance.gold >= buff[index].price[buff[index].level])
        {
            buff[index].level++;
            PlayerPrefs.SetInt("Ability" + index, buff[index].level);

            AbilityCard abilityCard = MenuUiManager.instance.MenuUi[index].gameObject.GetComponent<AbilityCard>();
            if (buff[index].level > buff[index].rate.Length - 1)
            {
                buff[index].level = Mathf.Min(buff[index].level, buff[index].rate.Length - 1);
                abilityCard.ButtonCheck(false);
            }

            GameManager.instance.gold -= buff[index].price[buff[index].level];
            Buff(index);
            abilityCard.Init();
        }
    }

    public void Buff(int index)
    {
        switch (index)
        {
            case 0:
                StatManager.instance.AttUpgrade(buff[index].rate[buff[index].level]);
                break;
            case 1:
                StatManager.instance.AttSpeedUpgrade(buff[index].rate[buff[index].level]);
                break;
            case 2:
                StatManager.instance.HealthUpgrade(buff[index].rate[buff[index].level]);
                break;
            default:
                break;
        }
    }

    private void OnEnable()
    {
        for (int index = 0; index < buff.Length; index++)
        {
            buff[index].level = PlayerPrefs.GetInt("Ability" + index);
            AbilityCard abilityCard = MenuUiManager.instance.MenuUi[index].gameObject.GetComponent<AbilityCard>();

            if (buff[index].level > buff[index].rate.Length - 1)
            {
                buff[index].level = Mathf.Min(buff[index].level, buff[index].rate.Length - 1);
                abilityCard.ButtonCheck(false);
            }
            if (buff[index].level != 0) Buff(index);
            abilityCard.Init();
        }


        // 만렙이면 버튼 비활성화

        // UI 초기화
    }
}
