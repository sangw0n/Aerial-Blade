using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;


public class Monster : MonoBehaviour
{
    Animator anim;
    [SerializeField] float speed;
    [SerializeField] float stoppingDistance;
    [SerializeField] float attackCooldown = 2f;
    [SerializeField] string playerTag = "Player";
    [SerializeField]
    GameObject bulletPrefab;
    bool isLive = true;
    bool hasFired = false;
    [SerializeField]
    float MaxHP = 10;
    [SerializeField]
    float CurHP = 10 ;
    [SerializeField]
    GameObject HitPtc;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    [SerializeField]
    Slider Hpbar;
    [SerializeField]
    GameObject Damagetext;
    [SerializeField] float knockbackForce = 5f;
    [SerializeField]
    bool Attacktrue = false;
    private Color originalColor;
    public Color newColor = Color.black;
    [SerializeField]
    SpriteRenderer spriteRenderer;
    float timeSinceLastAttack = 0f;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        originalColor = spriteRenderer.color;
    }

    void FixedUpdate()
    {
        Hpbar.value = (float)CurHP / (float)MaxHP;
       
        StartCoroutine(Die());
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

            if (Time.time - timeSinceLastAttack >= attackCooldown && Attacktrue)
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
    public void TakeDamage(float damage)
    {
        
        StartCoroutine(Attackbool());
       
        Destroy(Instantiate(HitPtc, transform.position, Quaternion.identity), 3f);
        Destroy(Instantiate(Damagetext, transform.position + new Vector3(0, 1.7f, 0), Quaternion.identity), 3f);
        CurHP -= damage;
        CameraShake.instance.Shake();
        StartCoroutine(getDamege());

        if (CurHP > 0)
        {
            GameObject player = GameObject.FindGameObjectWithTag(playerTag);
            if (player != null)
            {
                Vector2 knockbackDirection = (transform.position - player.transform.position).normalized;
                transform.Translate(knockbackDirection * knockbackForce);
            }
        }
    }

   IEnumerator Die()
    {
        if (CurHP <= 0)
        {
            anim.SetTrigger("Die");
            gameObject.tag = "Untagged";
            GetComponent<Collider2D>().isTrigger = true;
            rigid.simulated = false;
            Hpbar.gameObject.SetActive(false);

            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }
    }
    IEnumerator Attackbool()
    {
        Attacktrue = false;
        yield return new WaitForSeconds(5f);
        Attacktrue = true;
    }
    private IEnumerator getDamege()
    {
        spriteRenderer.color = newColor;
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.color = originalColor;
    }
}
