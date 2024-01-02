using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossOne : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float stoppingDistance;
    [SerializeField] float attackCooldown = 2f;
    [SerializeField] string playerTag = "Player";
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField] int AttackCount;
    [SerializeField] int MaxCount = 10;


    bool isLive = true;
    bool hasFired = false;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    float timeSinceLastAttack = 0f;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
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

    void LateUpdate()
    {
        if (!isLive)
            return;

        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        if (player != null)
            spriter.flipX = player.transform.position.x < transform.position.x;
    }
}
