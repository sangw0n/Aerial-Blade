using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public static InGameManager instance { get; private set; }

    public Dungeon dungeon;

    private void Awake()
    {
        instance = this;
    }
}
