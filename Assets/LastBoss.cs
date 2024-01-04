using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using EasyTransition;

public class LastBoss : MonoBehaviour
{
    Animator anim;
    public TransitionSettings transition;

    [SerializeField] int BossNum;
    [SerializeField] string playerTag = "Player";
    [SerializeField] GameObject bulletPrefab;

    [SerializeField] int MaxCount = 10;
    [SerializeField] float MaxHP = 10;
    [SerializeField] float CurHP = 10;
    [SerializeField] GameObject HitPtc;
    [SerializeField] Slider Hpbar;
    [SerializeField] Slider Hpbar2;
    bool isLive = true;
    [SerializeField] GameObject Damagetext;
    Rigidbody2D rigid;
    [SerializeField] SpriteRenderer spriter;

    [Header("이동로직")]
    [SerializeField] Collider2D[] Area;
    [SerializeField] Transform[] pos;
    [Header("공격")]
    [SerializeField] GameObject Laser;
    [SerializeField] GameObject DangerLaser;
    [SerializeField] Transform StunPos;
    [SerializeField] int AttackCount = 0;
    [SerializeField] int StunCount = 5;
    [SerializeField] bool NeverDie = true;

    bool isCoroutineRunning = false;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        StartCoroutine(Paton());
    }

    IEnumerator Paton()
    {
        while (true)
        {

            yield return StartCoroutine(FirstPaton());

            if (AttackCount >= 5)
            {
                NeverDie = false;
                yield return new WaitForSeconds(3);
                NeverDie = true;
                AttackCount = 0;
            }

        }
    }

    void Update()
    {
        Hpbar.value = Mathf.Lerp(Hpbar.value, (float)CurHP / (float)MaxHP, Time.deltaTime * 20); ;
        Hpbar2.value = Mathf.Lerp(Hpbar2.value, (float)CurHP / (float)MaxHP, Time.deltaTime * 2); ;

        if (CurHP <= 0)
        {
            if (GameManager.instance.isClear[GameManager.instance.inStageCount])
            {
                Destroy(gameObject);
                return;
            }
            GameManager.instance.isClear[GameManager.instance.inStageCount] = true;
            PlayerPrefs.SetInt("isClear" + GameManager.instance.inStageCount, 1);
            TransitionManager.Instance().Transition("EndScene", transition, 0);
            TransitionManager.Instance().Transition("EndScene", transition, 0);

            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        // 필요한 경우에만 FixedUpdate 로직 추가
    }

    void LateUpdate()
    {
        if (!isLive)
            return;

        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        // 필요한 경우에만 LateUpdate 로직 추가
    }

    public void TakeDamage(float damage)
    {
        if (!NeverDie)
        {
            Destroy(Instantiate(HitPtc, transform.position, Quaternion.identity), 3f);
            Destroy(Instantiate(Damagetext, transform.position + new Vector3(0, 1.5f, -50), Quaternion.identity), 3f);
            CurHP -= damage;

            CameraShake.instance.Shake();
        }

    }

    IEnumerator DangerLaserCor()
    {
        DangerLaser.SetActive(true);
        float timer = 0.0f;
        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        DangerLaser.SetActive(false);
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////












    IEnumerator FirstPaton()
    {
        while (AttackCount <= 5)
        {
            GameObject player = GameObject.FindGameObjectWithTag(playerTag);

            if (player != null)
            {
                for (int i = 0; i < Area.Length; i++)
                {
                    if (Area[i].bounds.Contains(player.transform.position))
                    {
                        if (i < pos.Length && !isCoroutineRunning)
                        {
                            yield return StartCoroutine(MoveBoss(pos[i].position, 0.3f));
                        }
                        break;
                    }
                }
            }
            yield return null;
        }
    }

    IEnumerator MoveBoss(Vector3 targetPosition, float moveTime)
    {
        isCoroutineRunning = true;

        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.LerpUnclamped(startPosition, targetPosition, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return StartCoroutine(DangerLaserCor());
        yield return new WaitForSeconds(0.4f);

        InstantiateLaser(transform.position + new Vector3(0, -30, 0), 0.4f);

        isCoroutineRunning = false;
    }

    void InstantiateLaser(Vector3 position, float destroyTime)
    {
        GameObject laserInstance = Instantiate(Laser, position, Quaternion.identity);
        AttackCount++;
        Destroy(laserInstance, destroyTime);
    }








    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///






}
