using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject Monster;
    public GameObject Monster2;
    public GameObject Monster3;

    public GameObject BossMonster;
    public GameObject BossMonsterTwo;
    public GameObject BossMonsterThree;

    public Transform[] SpawnPos;
    public Transform BossSpawnPoint;
    public int MonsterSpawnCOunt;

    float Wait = 0.4f;
    [SerializeField]
    int BossNum;

  
    // Start is called before the first frame update
    void Start()
    {
      if (BossNum == 1)
            StartCoroutine(MonsterSpawn());
        if (BossNum == 2)
            StartCoroutine(MonsterSpawnTwo());
        if (BossNum == 3)
            StartCoroutine(MonsterSpawnThree());
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
            if (MonsterSpawnCOunt == 50)
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
            Instantiate(Monster2, SpawnPos[randomIndex].position, Quaternion.identity);
            if (MonsterSpawnCOunt == 3)
            {
                Instantiate(BossMonsterTwo, BossSpawnPoint.transform.position, Quaternion.identity);
                Wait = 10000f;
            }
            yield return new WaitForSeconds(Wait);
        }

    }
    IEnumerator MonsterSpawnThree()
    {

        yield return new WaitForSeconds(5f);
        while (true)
        {
            MonsterSpawnCOunt++;
            int randomIndex = Random.Range(0, SpawnPos.Length);
            Instantiate(Monster3, SpawnPos[randomIndex].position, Quaternion.identity);
            if (MonsterSpawnCOunt == 3)
            {
                Instantiate(BossMonsterThree, BossSpawnPoint.transform.position, Quaternion.identity);
                Wait = 10000f;
            }
            yield return new WaitForSeconds(Wait);
        }

    }

}
