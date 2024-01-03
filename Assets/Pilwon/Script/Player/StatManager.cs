using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public static StatManager instance { get; private set; }
    public float heatlh;
    public int att;

    [Header("[ Touch Var Header ]")]
    public float coolTime;
    public float maxHealth;
    public int baseAtt;
    public float baseCoolTime;

    public float speed;

    public Player player;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != null) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        // Player Var Init
        baseAtt = att;
        baseCoolTime = coolTime;
    }

    public void Init()
    {
        //player.health = maxHealth;
        //player.HitDamage = att;
        //player.coolTime = this.coolTime;
        //player.movespeed = speed;
    }

    public void AttUpgrade(int rate)
    {
        att = baseAtt + (att * rate);
        Init();
    }

    public void AttSpeedUpgrade(float rate)
    {
        coolTime = baseCoolTime - rate;
        Init();
    }

    public void healthUpgrade(float rate)
    {
        maxHealth = maxHealth + (maxHealth * rate);
        Init();
    }
}
