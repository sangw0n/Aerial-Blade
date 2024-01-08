using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossClear { Fail, Clear }

public class CubeManager : MonoBehaviour
{
    public static CubeManager instance { get; private set; }

    public Cube[] cube;
    public Sprite[] UnlcokSprite;
    public Sprite[] LockSprite;

    public GameObject[] levelParticle;

    private void Awake()
    {
        instance = this;
        cube = GetComponentsInChildren<Cube>();
    }

    private void Start()
    {
        GameManager.instance.isStop = false;
        for (int index = 0; index < cube.Length; index++) Init(index);
    }

    public void Init(int index)
    {
        SpriteRenderer sprite = cube[index].GetComponent<SpriteRenderer>();

        if (PlayerPrefs.GetInt("Clear" + index) >= 1)
        {
            sprite.sprite = UnlcokSprite[cube[index].id];
            cube[index].deco.SetActive(true);
            cube[index].cubeLock = CubeLock.Unlock;
        }
        else
        {
            sprite.sprite = LockSprite[index];
            cube[index].deco.SetActive(false);
            cube[index].cubeLock = CubeLock.Lock;
        }
    }

    public void CubeSave(int index, BossClear clear)
    {
        PlayerPrefs.SetInt("Clear" + index, (int)clear);
        
        Init(index); 
    }
}
