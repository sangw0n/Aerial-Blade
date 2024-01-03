using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ability { HealthUp, Skill, DamageUP, AttSpeedUP, Boss}
public enum CubeLock { Lock, Unlock }

public class Cube : MonoBehaviour
{
    [Header(" [ Enum Header ] ")]
    public Ability ability;
    public CubeLock cubeLock;

    [Header(" [ Var Header ] ")]
    public int id;
    public Transform[] spawnPos;

    [Header(" [ Data Header ] ")]
    public BossData bossData;

    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        spawnPos = GetComponentsInChildren<Transform>();
    }
}
