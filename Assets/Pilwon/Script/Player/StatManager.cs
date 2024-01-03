using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public static StatManager instance { get; private set; }
    public float heatlh;
    public float maxHealth;
    public float speed;
    public float coolTime;

    public Player player;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != null) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
}
