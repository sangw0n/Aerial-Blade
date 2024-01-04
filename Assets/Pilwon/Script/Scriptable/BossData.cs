using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName ="ScirptableObejct/BossData")]
public class BossData : ScriptableObject
{
    [Header("[ Boss Var Header ]")]
    public string bossName;
    public float health;
    public float maxHealth;
    public int speed;
    public float att;

    [Header("[ Boss Ui Var ]")]
    public Sprite sprite;
}
