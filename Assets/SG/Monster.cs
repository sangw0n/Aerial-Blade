using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Die();
        if (!isLive)
            return;

        // �÷��̾� ã��
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        if (player == null)
            return; 

        Vector2 dirVec = player.transform.position - transform.position;
        float distanceToPlayer = dirVec.magnitude;

        if (distanceToPlayer > stoppingDistance)
        {
            // �÷��̾���� �Ÿ��� ���� �Ÿ����� ũ�� ��� �̵��ϴ� ��
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
        }
        else
        {
            // �÷��̾���� ���� �Ÿ� ���̸� ���ߴ� ��
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
