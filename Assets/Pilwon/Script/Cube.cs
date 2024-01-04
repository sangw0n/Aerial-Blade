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
    public GameObject deco;

    [Header(" [ Data Header ] ")]
    public BossData bossData;

    public bool isFirstParicle = false;

    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        isFirstParicle = PlayerPrefs.GetInt("IsParticle" + id) != 1 ? false : true;
    }

    private void Start()
    {
        spawnPos = GetComponentsInChildren<Transform>();

        if (GameManager.instance.isClear[id])
        {
            if(!isFirstParicle)
            {
                PlayerPrefs.SetInt("IsParticle" + id, 1);
                Instantiate(CubeManager.instance.levelParticle[id], spawnPos[1].transform.position,Quaternion.identity);
                StartCoroutine(WaitTime());
                Debug.Log("Èå¾æ ÆÄÆ¼Å¬ ¹ßµ¿!!!!!");
            }
            else
            {
                CubeManager.instance.CubeSave(id, BossClear.Clear);
            }
        }
        else CubeManager.instance.CubeSave(id, BossClear.Fail);
    }

    private IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(1.0f);
        CubeManager.instance.CubeSave(id, BossClear.Clear);
    }
}