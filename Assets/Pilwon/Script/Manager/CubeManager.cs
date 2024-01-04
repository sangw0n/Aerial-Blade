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
        for (int index = 0; index < cube.Length; index++) Init(index);
    }

    private void Update()
    {
        // Test Code
        if (Input.GetKeyDown(KeyCode.Alpha1)) CubeSave(0, BossClear.Clear);
        if (Input.GetKeyDown(KeyCode.Alpha2)) CubeSave(1, BossClear.Clear);
        if (Input.GetKeyDown(KeyCode.Alpha3)) CubeSave(2, BossClear.Clear);
        if (Input.GetKeyDown(KeyCode.Alpha4)) CubeSave(3, BossClear.Clear);

    }

    public void Init(int index)
    {
        SpriteRenderer sprite = cube[index].GetComponent<SpriteRenderer>();

        if (PlayerPrefs.GetInt("Clear" + index) >= 1)
        {
            sprite.sprite = UnlcokSprite[cube[index].id];
            cube[index].cubeLock = CubeLock.Unlock;
        }
        else
        {
            sprite.sprite = LockSprite[index];
            cube[index].cubeLock = CubeLock.Lock;
        }
    }

    public void CubeSave(int index, BossClear clear)
    {
        PlayerPrefs.SetInt("Clear" + index, (int)clear);
        
        Init(index); 
    }
}
