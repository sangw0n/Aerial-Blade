using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Buff
{
    public string buffName;
    public Sprite Icon;
    public string docs;
    public Ability ability;

    [Space(5)]
    public float[] buffNum;
    public int[] upgradeMoney;
    public int upgradeCount;
}

public enum TextState { Title, Docs, ButtonText }

public class BuffManager : MonoBehaviour
{
    public static BuffManager instance { get; private set; }
    public Buff[] buff;

    [Header("[ Buff Panel Header ]")]
    [SerializeField] private TMP_Text[] text;
    [SerializeField] private Image icon;
    [SerializeField] private Button button;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Init(0);
    }

    public void Init(int index)
    {
        var buff = this.buff[index];
        text[0].text = string.Format("{0} °­È­", buff.buffName);
        text[1].text = string.Format("{0} + {1}%", buff.buffName, buff.buffNum[buff.upgradeCount] * 100);
        text[2].text = string.Format("- {0} Gold", buff.upgradeMoney[buff.upgradeCount]);

        icon.sprite = buff.Icon;
    }

    private void UpgradeButton(Ability ability)
    {
        GameManager gameMgr = GameManager.instance;
        Buff buff = this.buff[(int)ability];
        UpgeadeMaxCheck(ability);

        switch (ability)
        {
            case Ability.DamageUP:
            case Ability.AttSpeedUP:
            case Ability.HealthUp:
                if (gameMgr.gold >= buff.upgradeMoney[buff.upgradeCount])
                {
                    gameMgr.gold -= buff.upgradeMoney[buff.upgradeCount];
                    StatManager.instance.AttUpgrade(buff.buffNum[buff.upgradeCount]);
                    buff.upgradeCount++;
                }
                else
                {
                    // ¶ò¼Ò¸® Àç»ý
                    Debug.Log("µ· ¾øÀ½ ¤»¤»");
                }
                break;
        }
    }

    private void UpgeadeMaxCheck(Ability ability)
    {
        if(buff[(int)ability].upgradeCount == buff[(int)ability].buffNum.Length - 1)
        {
            button.interactable = false;
            return;
        }
        else
        {
            button.interactable = true;
        }
    }
}
