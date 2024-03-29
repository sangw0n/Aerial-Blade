using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class MiniBossOne : MonoBehaviour
{
    Animator anim;
    [SerializeField] int BossNum;
    [SerializeField] float speed;
    [SerializeField] float stoppingDistance;
    [SerializeField] float attackCooldown = 2f;
    [SerializeField] string playerTag = "Player";
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField] int AttackCount;
    [SerializeField] int MaxCount = 10;
    [SerializeField]
    float MaxHP = 10;
    [SerializeField]
    float CurHP = 10;
    [SerializeField]
    GameObject HitPtc;
    [SerializeField]
    Slider Hpbar;
    [SerializeField]
    Slider Hpbar2;
    bool isLive = true;
    bool hasFired = false;
    [SerializeField]
    GameObject Damagetext;
    Rigidbody2D rigid;
    [SerializeField]
    SpriteRenderer spriter;

    [Header("보스2")]
    [SerializeField]
    public Vector2 boxSize;
    public Transform boxpos;
  
    float timeSinceLastAttack = 0f;
    bool Attacktrue = true;
    [SerializeField]
    GameObject StunPtc;

    [Header("보스3")]
    [SerializeField]
    GameObject Boss3Ptc;
    [SerializeField]
    GameObject Boss3Ptc2;
    [SerializeField]
    public Vector2 boxSize2;
    public Transform boxpos2;
    [SerializeField]
    GameObject Danger;
   


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
   void Start()
    {
        if (BossNum == 2)
        {
            StartCoroutine(BossTwoAtk());
            StartCoroutine(BossTwoDash());
        }
        if (BossNum == 3)
        {
            StartCoroutine( BossThreeCor());
        }
      


    }
    void Update()
    {
        
        Hpbar.value = Mathf.Lerp(Hpbar.value, (float)CurHP / (float)MaxHP, Time.deltaTime * 20); ;
        Hpbar2.value = Mathf.Lerp(Hpbar2.value, (float)CurHP / (float)MaxHP, Time.deltaTime * 4); ;
        if (CurHP <= 0)
        {
            GameManager.instance.GameClear();
            GameManager.instance.isStop = true;
            GameManager.instance.gold += GameManager.instance.clearGold;

            if (GameManager.instance.isClear[GameManager.instance.inStageCount]) 
            {
                Destroy(gameObject);
                return; 
            }
            GameManager.instance.isClear[GameManager.instance.inStageCount] = true;
            PlayerPrefs.SetInt("isClear" + GameManager.instance.inStageCount, 1);
            GameManager.instance.clearStageIndex++;
            PlayerPrefs.SetInt("ClearIndex", GameManager.instance.clearStageIndex);

            Destroy(gameObject);
        }
    }
    void FixedUpdate()
    {
        if (BossNum == 1)
        {
            BossOne();
        }
        if (BossNum == 2)
        {
            BossTwo();
           
        }
        if (BossNum == 3)
        {
            BossThree();

        }
    }

    void LateUpdate()
    {
        if (!isLive)
            return;

        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
       
        if (player != null )
        {
            if (player.transform.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(3, 3, 3);
            }
            else
            {
                transform.localScale = new Vector3(-3, 3, 3);
            
            }
        }
    }
    public void TakeDamage(float damage)
    {
        Destroy(Instantiate(HitPtc, transform.position, Quaternion.identity), 3f);
        Destroy(Instantiate(Damagetext, transform.position + new Vector3(0,1.5f,0), Quaternion.identity), 3f);
        CurHP = CurHP - damage;
        CameraShake.instance.Shake();

    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void BossOne()
    {
        if (!isLive)
            return;

        // 플레이어 찾기
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        if (player == null)
            return;

        Vector2 dirVec = player.transform.position - transform.position;
        float distanceToPlayer = dirVec.magnitude;

        if (distanceToPlayer > stoppingDistance)
        {
            // 플레이어와의 거리가 멈춤 거리보다 크면 계속 이동하는 거
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
        }
        else
        {
            // 플레이어와의 멈춤 거리 안이면 멈추는 거
            rigid.velocity = Vector2.zero;

            if (Time.time - timeSinceLastAttack >= attackCooldown)
            {
                if (AttackCount < MaxCount)
                {
                    attackCooldown = 0.2f;
                    AudioManager.instance.PlaySound(transform.position, 3, Random.Range(0.7f, 1.2f), 1);
                    Destroy(Instantiate(bulletPrefab, transform.position, Quaternion.identity), 3f);
                    AttackCount++;
                    timeSinceLastAttack = Time.time;
                }
                else if (AttackCount >= MaxCount)
                {
                    attackCooldown = 5;
                    AttackCount = 0;
                }
            }
        }
    }






    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




    void BossTwo()
    {
        if (!isLive)
            return;

        // 플레이어 찾기
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        
        if (player == null)
            return;

        Vector2 dirVec = player.transform.position - transform.position;
        float distanceToPlayer = dirVec.magnitude;

      
           
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
       


    }
    IEnumerator BossTwoAtk()
    {
        while (true)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(boxpos.position, boxSize, 0);
            bool hasPlayerCollision = false;

            foreach (Collider2D collider in collider2Ds)
            {
                if (collider != null)
                {
                    if (collider.tag == "Player" && Attacktrue)
                    {
                        anim.SetBool("Attack", true);
                        yield return new WaitForSeconds(0.45f);
                        collider.GetComponent<Player>().TakeDamage(1);
                        hasPlayerCollision = true;
                    }
                }
            }

            if (!hasPlayerCollision)
            {
                anim.SetBool("Attack", false);
            }

            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator BossTwoDash()
    {
        while(true){
            yield return new WaitForSeconds(1f);
            speed = 10;
            anim.SetBool("isMoving", true);
        yield return new WaitForSeconds(1.5f);
            anim.SetBool("isMoving", false);
            speed = 0;
            Attacktrue = false;
            Destroy(Instantiate(StunPtc, transform.position + new Vector3(0, 4, 0), Quaternion.Euler(-90f, 0f, 0f)), 4f);

            yield return new WaitForSeconds(4f);
            speed = 3;
            Attacktrue = true;
            yield return new WaitForSeconds(7f);
        }
    }




    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    void BossThree()
    {
        if(!isLive)
            return;

        // 플레이어 찾기
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        if (player == null)
            return;

        Vector2 dirVec = player.transform.position - transform.position;
        float distanceToPlayer = dirVec.magnitude;



        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

    }

    IEnumerator BossThreeCor()
    {   while (true) 
      { 
        yield return new WaitForSeconds(1f);

        Destroy(Instantiate(Boss3Ptc, transform.position, Quaternion.identity), 3f);

        spriter.enabled = false;
            gameObject.tag = "Untagged";
        yield return new WaitForSeconds(3f);
       
        // 플레이어의 위치에 다시 나타나는 로직...
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
           

            transform.position = player.transform.position;
            Danger.SetActive(true);
            yield return new WaitForSeconds(1f);
            Danger.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            Destroy(Instantiate(Boss3Ptc2, transform.position, Quaternion.identity), 3f);
                gameObject.tag = "BossMonster";
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(boxpos2.position, boxSize2, 0);

            foreach (Collider2D collider in collider2Ds)
            {
                if (collider != null)
                {
                    if (collider.tag == "Player")
                    {
                        
                        collider.GetComponent<Player>().TakeDamage(5);
                    }
                }
            }

            
            spriter.enabled = true;
        }
            yield return new WaitForSeconds(3f);

        }
    }


private void OnDrawGizmos()
    {
        if (BossNum == 2)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(boxpos.position, boxSize);
            
        }
        if (BossNum == 3) { 
            Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boxpos2.position, boxSize2);
        }


    }
}
  

