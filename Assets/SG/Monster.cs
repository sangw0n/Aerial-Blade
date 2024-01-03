using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Monster : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float stoppingDistance;
    [SerializeField] float attackCooldown = 2f;
    [SerializeField] string playerTag = "Player";
    [SerializeField]
    GameObject bulletPrefab;
    bool isLive = true;
    bool hasFired = false;
    [SerializeField]
    int MaxHP = 10;
    [SerializeField]
    int CurHP = 10 ;
    [SerializeField]
    GameObject HitPtc;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    [SerializeField]
    Slider Hpbar;

    float timeSinceLastAttack = 0f;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Hpbar.value = (float)CurHP / (float)MaxHP;
        Die();
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
                Destroy(Instantiate(bulletPrefab, transform.position, Quaternion.identity), 3f);
                timeSinceLastAttack = Time.time;
            }
        }
    }

    void LateUpdate()
    {
        if (!isLive)
            return;
     
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        if (player != null)
            spriter.flipX = player.transform.position.x < transform.position.x;
    }
    public void TakeDamage(int damage)
    {
        Destroy(Instantiate(HitPtc,transform.position, Quaternion.identity),3f);
       CurHP = CurHP - damage;
        CameraShake.instance.Shake();
       
    }
    void Die()
    {
        if (CurHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
