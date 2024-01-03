using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public static StatManager instance { get; private set; }
    public float att;

    [Header("[ Touch Var Header ]")]
    public float coolTime;
    public float maxHealth;
    public float baseAtt;
    public float baseCoolTime;

    public float speed;

    public Player player;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != null) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        // Player Var Init
        baseAtt = att;
        baseCoolTime = coolTime;
    }

    public void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        player.maxHp = maxHealth;
        player.curHp = maxHealth;
        player.HitDamage = att;
        player.coolTime = this.coolTime;
        player.moveSpeed = speed;
    }

    public void AttUpgrade(float rate)
    {
        att = baseAtt + (att * rate);
    }

    public void AttSpeedUpgrade(float rate)
    {
        coolTime = baseCoolTime - rate;
    }

    public void healthUpgrade(float rate)
    {
        maxHealth = maxHealth + (maxHealth * rate);
    }
}
