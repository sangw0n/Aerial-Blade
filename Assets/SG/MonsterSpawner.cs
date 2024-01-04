using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject Monster;
    public GameObject BossMonster;
    public GameObject BossMonsterTwo;
    public Transform[] SpawnPos;
    public Transform BossSpawnPoint;
    public int MonsterSpawnCOunt;

    float Wait = 2f;
    [SerializeField]
    int BossNum;

  
    // Start is called before the first frame update
    void Start()
    {
      if (BossNum == 1)
            StartCoroutine(MonsterSpawn());
        if (BossNum == 2)
            StartCoroutine(MonsterSpawnTwo());
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
                Wait = 10000f;
            }
            yield return new WaitForSeconds(Wait);
        }

    }
    IEnumerator MonsterSpawnTwo()
    {

        yield return new WaitForSeconds(5f);
        while (true)
        {
            MonsterSpawnCOunt++;
            int randomIndex = Random.Range(0, SpawnPos.Length);
            Instantiate(Monster, SpawnPos[randomIndex].position, Quaternion.identity);
            if (MonsterSpawnCOunt == 30)
            {
                Instantiate(BossMonsterTwo, BossSpawnPoint.transform.position, Quaternion.identity);
                Wait = 10000f;
            }
            yield return new WaitForSeconds(Wait);
        }

    }
  
}
