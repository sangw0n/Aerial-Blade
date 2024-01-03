using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject Monster;
    public GameObject BossMonster;
    public Transform[] SpawnPos;
    public Transform BossSpawnPoint;
    public int MonsterSpawnCOunt;
    float Wait = 2f;
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

        yield return new WaitForSeconds(5f);
        while (true)
        {
            MonsterSpawnCOunt++;
            int randomIndex = Random.Range(0, SpawnPos.Length);
            Instantiate(Monster, SpawnPos[randomIndex].position, Quaternion.identity);
            if (MonsterSpawnCOunt == 30)
            {
                Instantiate(BossMonster, BossSpawnPoint.transform.position, Quaternion.identity);
                Wait = 7f;
            }
            yield return new WaitForSeconds(Wait);
        }

    }
}
