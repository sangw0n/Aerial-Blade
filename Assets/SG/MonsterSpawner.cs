using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject Monster;
    public Transform[] SpawnPos;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MonsterSpawn());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator MonsterSpawn()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, SpawnPos.Length);
            Instantiate(Monster, SpawnPos[randomIndex].position, Quaternion.identity);
            yield return new WaitForSeconds(3f);
        }
    }
}
