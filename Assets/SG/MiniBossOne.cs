using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;


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
    SpriteRenderer spriter;

    [Header("보스2")]
    [SerializeField]
    public Vector2 boxSize;
    public Transform boxpos;
    float timeSinceLastAttack = 0f;
    bool Attacktrue = true;
    [SerializeField]
    GameObject StunPtc;
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
        
    }
    void Update()
    {
        Hpbar.value = Mathf.Lerp(Hpbar.value, (float)CurHP / (float)MaxHP, Time.deltaTime * 20); ;
        Hpbar2.value = Mathf.Lerp(Hpbar2.value, (float)CurHP / (float)MaxHP, Time.deltaTime * 4); ;
        if (CurHP <= 0)
        {
            if (GameManager.instance.isClear[GameManager.instance.inStageCount]) 
            {
                Destroy(gameObject);
                return; 
            }
            GameManager.instance.isClear[GameManager.instance.inStageCount] = true;
            PlayerPrefs.SetInt("isClear" + GameManager.instance.inStageCount, 1);

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
    }

    void LateUpdate()
    {
        if (!isLive)
            return;

        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        if (player != null)
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










    private void OnDrawGizmos()
    {
        if (BossNum == 2)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(boxpos.position, boxSize);
        }
       

    }
}
  

