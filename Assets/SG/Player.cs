using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float targetSize = 5f; // 목표 카메라 크기
    public float lerpSpeed = 2f; // 보간 속도

    [SerializeField]
    GameObject Dashpt;
    bool isdash = false;
    public Vector2 inputVec;
    public CinemachineVirtualCamera virtualCamera;

    public float maxHp;
    public float curHp;

    public float health;

    Rigidbody2D rb;
    public float moveSpeed;
    [SerializeField]
    Slider Hpbar;
    Animator anim;
    float curTime;
    float SkillcurTime;
    [SerializeField]
    float Skill3curTime;

    public float Skill2curTime;

    public float DashcurTime;
    [SerializeField] public float HitDamage = 1;
    public float coolTime = 0.5f;
    public float SkillcoolTime = 5f;
    public float Skill2coolTime = 5f;
    public float Skill3coolTime = 5f;


    public float DashcoolTime = 1f;

    public Transform pos;
    public Transform Skillpos;
    public Transform Skill2pos;

    public float DashSpeed = 2f;

    public Vector2 boxSize;
    public Vector2 skillboxSize;
    public Vector2 skill2boxSize;

    [SerializeField] GameObject SlashPtc;
    [SerializeField] GameObject SlashPtc2;
    [SerializeField] GameObject SkillPtc;
    [SerializeField] GameObject POWER;
    [SerializeField] GameObject POWER2;
    [SerializeField] GameObject Eyeptc;
    [SerializeField] GameObject Dark;
    [SerializeField] GameObject Red;
    [SerializeField] GameObject FlashPtc;



    [SerializeField]
    SpriteRenderer spriteRenderer;
    [SerializeField] Color playerA;
    [SerializeField]
    bool NeverDie = false;

    private bool isSlashPtc1Active = true;
    private bool isSideAttack1 = true;  // sideattack과 sideattack2를 번갈아가며 발동하기 위한 변수

    void Start()
    {
        curHp = maxHp;
        GameManager.instance.isStop = false;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(curHp <= 0)
        {
            GameManager.instance.isStop = true;
            GameManager.instance.GameOver();
        }

        if (Skill3curTime <= 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartCoroutine(NeverDieS());
                StartCoroutine(LerpCameraSize(targetSize));
                StartCoroutine(MoveToMonsters());
                 
               
                Skill3curTime = Skill3coolTime;
                SkillUiManager.instance.skillCollTime_3.fillAmount = 1;
            }

        }
        else
        {
            Skill3curTime -= Time.deltaTime;
            SkillUiManager.instance.skillCollTime_3.fillAmount = Skill3curTime / Skill3coolTime;
        }
        Hpbar.value = Mathf.Lerp(Hpbar.value, (float)curHp / (float)maxHp, Time.deltaTime * 4); ;
        Move();
        Skill();
        Dash();
        Skill2();
        if (curTime <= 0)
        {
            if (Input.GetKey(KeyCode.X))
            {
                AudioManager.instance.PlaySound(transform.position, 0, Random.Range(1.4f, 2.5f), 1);

                if (transform.localScale.x < 0)
                {
                    Destroy(Instantiate(isSlashPtc1Active ? SlashPtc : SlashPtc2, transform.position + new Vector3(-0.5f, 0f, 0), Quaternion.identity), 3f);

                    // sideattack1 또는 sideattack2 트리거 발동
                    anim.SetTrigger(isSideAttack1 ? "SideAttack" : "SideAttack2");
                }
                if (transform.localScale.x > 0)
                {
                    Destroy(Instantiate(isSlashPtc1Active ? SlashPtc : SlashPtc2, transform.position + new Vector3(0.5f, 0f, 0), Quaternion.Euler(new Vector3(0, 180, 0))), 3f);

                    // sideattack1 또는 sideattack2 트리거 발동
                    anim.SetTrigger(isSideAttack1 ? "SideAttack" : "SideAttack2");
                }

                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
                foreach (Collider2D collider in collider2Ds)
                {
                    if (collider != null)
                    {
                        if (collider.tag == "Monster")
                        {
                            collider.GetComponent<Monster>().TakeDamage(StatManager.instance.att);
                        }
                        if (collider.tag == "BossMonster")
                        {
                            collider.GetComponent<MiniBossOne>().TakeDamage(StatManager.instance.att);
                        }
                        if (collider.tag == "LastBoss")
                        {
                            collider.GetComponent<LastBoss>().TakeDamage(StatManager.instance.att);
                        }
                    }
                }

                curTime = coolTime;
                isSlashPtc1Active = !isSlashPtc1Active; // 번갈아가면서 활성화 여부를 변경
                isSideAttack1 = !isSideAttack1;  // sideattack1과 sideattack2를 번갈아가면서 발동하기 위해 변경
            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Skillpos.position, skillboxSize);
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(Skill2pos.position, skill2boxSize);
    }

    void Move()
    {
        if (isdash)
            return;

        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        Vector2 dirVec = inputVec.normalized * moveSpeed;
        rb.velocity = dirVec;

        if (Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            if (rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1.2f, 1.2f, 1.2f);
            }
            if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            anim.SetBool("run", true);
        }
        else
        {
            anim.SetBool("run", false);
            anim.SetBool("SideIdle", true);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            anim.SetBool("Frontrun", true);
        }
        else
        {
            anim.SetBool("Frontrun", false);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            anim.SetBool("SideIdle", true);
        }
        else
        {
            anim.SetBool("SideIdle", false);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            anim.SetBool("BackIdle", true);
        }
        else
        {
            anim.SetBool("BackIdle", false);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            anim.SetBool("BackRun", true);
        }
        else
        {
            anim.SetBool("BackRun", false);
        }
    }

    void Skill()
    {
        if (SkillcurTime <= 0)
        {
            if (Input.GetKey(KeyCode.V))
            {

                AudioManager.instance.PlaySound(transform.position, 2, Random.Range(1.4f, 1.7f), 1);
                anim.SetTrigger("SKill1");
                StartCoroutine(SkillCor());
                SkillcurTime = SkillcoolTime; // 여기서 SkillcurTime을 초기화해야 합니다.
                SkillUiManager.instance.skillCollTime_2.fillAmount = 1;
            }
        }
        else
        {
            SkillcurTime -= Time.deltaTime;
            SkillUiManager.instance.skillCollTime_2.fillAmount = SkillcurTime / SkillcoolTime;

        }
    }
    void Skill2()
    {
        if (Skill2curTime <= 0)
        {

            if (Input.GetKey(KeyCode.C))
            {
                StartCoroutine(SKill2Cor());
                Skill2curTime = Skill2coolTime;
                SkillUiManager.instance.skillCollTime_1.fillAmount = 1;
            }

        }
        else
        {
            Skill2curTime -= Time.deltaTime;
            SkillUiManager.instance.skillCollTime_1.fillAmount = Skill2curTime / Skill2coolTime;
        }

    }

    IEnumerator SkillCor()
    {
        NeverDie = true;
        for (int i = 0; i < 5; i++)
        {
            Destroy(Instantiate(SkillPtc, transform.position, Quaternion.identity), 3f);
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(Skillpos.position, skillboxSize, 0);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider != null)
                {
                    NeverDie = true;
                    if (collider.tag == "Monster")
                    {
                        collider.GetComponent<Monster>().TakeDamage(StatManager.instance.att);
                    }
                    if (collider.tag == "BossMonster")
                    {
                        collider.GetComponent<MiniBossOne>().TakeDamage(StatManager.instance.att);
                    }
                    if (collider.tag == "LastBoss")
                    {
                        collider.GetComponent<LastBoss>().TakeDamage(StatManager.instance.att);
                    }
                }
            }
            yield return new WaitForSeconds(0.08f);
        }
        NeverDie = false;
    }

    void Dash()
    {
        if (DashcurTime <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Instantiate(Dashpt, transform.position, Quaternion.identity);
                StartCoroutine(Dashcor());
                // 대시 방향을 입력 방향으로 설정
                Vector2 dashDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;


                // 대시 방향으로 힘을 가하기
                rb.AddForce(dashDirection * DashSpeed, ForceMode2D.Impulse);

                StartCoroutine(DashTrigger());
                DashcurTime = DashcoolTime;
            }
        }
        else
        {
            DashcurTime -= Time.deltaTime;
        }
    }



    IEnumerator DashTrigger()
    {
        NeverDie = true;
        yield return new WaitForSeconds(1f);
        NeverDie = false;
    }
    IEnumerator SKill2Cor()
    {
        NeverDie = true;
        for (int i = 0; i < 10; i++)
        {
            AudioManager.instance.PlaySound(transform.position, 0, Random.Range(1.4f, 2.4f), 1);

            if (transform.localScale.x < 0)
            {
                Destroy(Instantiate(isSlashPtc1Active ? SlashPtc : SlashPtc2, transform.position + new Vector3(-0.5f, 0f, 0), Quaternion.identity), 3f);

                // sideattack1 또는 sideattack2 트리거 발동
                anim.SetTrigger(isSideAttack1 ? "SideAttack" : "SideAttack2");
            }
            if (transform.localScale.x > 0)
            {
                Destroy(Instantiate(isSlashPtc1Active ? SlashPtc : SlashPtc2, transform.position + new Vector3(0.5f, 0f, 0), Quaternion.Euler(new Vector3(0, 180, 0))), 3f);

                // sideattack1 또는 sideattack2 트리거 발동
                anim.SetTrigger(isSideAttack1 ? "SideAttack" : "SideAttack2");
            }

            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider != null)
                {
                    if (collider.tag == "Monster")
                    {
                        collider.GetComponent<Monster>().TakeDamage(StatManager.instance.att);
                    }
                    if (collider.tag == "BossMonster")
                    {
                        collider.GetComponent<MiniBossOne>().TakeDamage(StatManager.instance.att);
                    }
                    if (collider.tag == "LastBoss")
                    {
                        collider.GetComponent<LastBoss>().TakeDamage(StatManager.instance.att);
                    }
                }
            }


            isSlashPtc1Active = !isSlashPtc1Active; // 번갈아가면서 활성화 여부를 변경
            isSideAttack1 = !isSideAttack1;  // sideattack1과 sideattack2를 번갈아가면서 발동하기 위해 변경
            yield return new WaitForSeconds(0.07f);
        }

        yield return new WaitForSeconds(0.3f);
        StartCoroutine(Dashcor());
        // 대시 방향을 입력 방향으로 설정
        Vector2 dashDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;


        // 대시 방향으로 힘을 가하기
        rb.AddForce(dashDirection * DashSpeed, ForceMode2D.Impulse);

        StartCoroutine(DashTrigger());
        yield return new WaitForSeconds(0.05f);
        AudioManager.instance.PlaySound(transform.position, 1, Random.Range(1.0f, 1.0f), 1);
        if (transform.localScale.x < 0)
        {
            Destroy(Instantiate(POWER, transform.position + new Vector3(-0.5f, 0f, 0), Quaternion.identity), 3f);


            anim.SetTrigger("SideAttack");
        }
        if (transform.localScale.x > 0)
        {
            Destroy(Instantiate(POWER, transform.position + new Vector3(0.5f, 0f, 0), Quaternion.Euler(new Vector3(0, 180, 0))), 3f);


            anim.SetTrigger("SideAttack");
        }
        Collider2D[] collider2D = Physics2D.OverlapBoxAll(Skill2pos.position, skill2boxSize, 0); ;
        foreach (Collider2D collider in collider2D)
        {
            if (collider != null)
            {
                if (collider.tag == "Monster")
                {
                    StatManager.instance.att *= 5;
                    collider.GetComponent<Monster>().TakeDamage(StatManager.instance.att);
                    yield return new WaitForSeconds(0.1f);
                    StatManager.instance.att /= 5;


                }
                if (collider.tag == "BossMonster")
                {
                    StatManager.instance.att *= 5;
                    collider.GetComponent<MiniBossOne>().TakeDamage(StatManager.instance.att);
                    yield return new WaitForSeconds(0.1f);
                    StatManager.instance.att /= 5;

                }
            }


        }
        ;
        NeverDie = false;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MonsterAttack" && !NeverDie)
        {
            curHp -= 1;
            //StartCoroutine(RedCor());
            CameraShake.instance.Shake();
            StartCoroutine(NeverDieCor());
        }
    }

    private void OnEnable()
    {
        StatManager.instance.Init();
    }
    IEnumerator Dashcor()
    {
        isdash = true;
        yield return new WaitForSeconds(0.1f);
        isdash = false;
    }
    IEnumerator NeverDieCor()
    {

        playerA.a = 0.5f;
        spriteRenderer.color = playerA;
        NeverDie = true;
        yield return new WaitForSeconds(1.5f);
        NeverDie = false;

        playerA.a = 1f;
        spriteRenderer.color = playerA;
    }

    IEnumerator NeverDieS()
    {
        NeverDie = true;
        yield return new WaitForSeconds(5f);
        NeverDie = false;
    }

    IEnumerator MoveToMonsters()
    {

        
        Dark.SetActive(true);
        anim.SetTrigger("Ready");
        yield return new WaitForSeconds(0.5f);

        if (transform.localScale.x < 0)
        {
            Destroy(Instantiate(Eyeptc, transform.position + new Vector3(-0.5f, -0.2f, 0), Quaternion.identity), 3f);



        }
        if (transform.localScale.x > 0)
        {
            Destroy(Instantiate(Eyeptc, transform.position + new Vector3(0.5f, -0.2f, 0), Quaternion.identity), 3f);



        }
        yield return new WaitForSeconds(1f);
        Dark.SetActive(false);
        // "Monster" 태그를 가진 모든 오브젝트를 찾습니다.

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

      

        // 각 몬스터에 대해 순차적으로 이동합니다.
        foreach (GameObject monster in monsters)
        {
            // 몬스터의 위치로 이동합니다.
            Destroy(Instantiate(FlashPtc, transform.position, Quaternion.identity), 3f);
            StartCoroutine(MoveToTarget(monster.transform.position));

            AudioManager.instance.PlaySound(transform.position, 0, Random.Range(2f, 2.5f), 1);
            // 기다립니다. (이동이 완료될 때까지 대기)
            yield return new WaitForSeconds(0.1f); // 예시로 1초 대기 (조절 가능)
            if (transform.localScale.x < 0)
            {
                Destroy(Instantiate(POWER2, transform.position + new Vector3(-0.5f, 0f, 0), Quaternion.identity), 3f);


                anim.SetTrigger("SideAttack");
            }
            if (transform.localScale.x > 0)
            {
                Destroy(Instantiate(POWER2, transform.position + new Vector3(0.5f, 0f, 0), Quaternion.Euler(new Vector3(0, 180, 0))), 3f);


                anim.SetTrigger("SideAttack");
            }
            Collider2D[] collider2D = Physics2D.OverlapBoxAll(Skill2pos.position, skill2boxSize, 0); ;
            foreach (Collider2D collider in collider2D)
            {
                if (collider != null)
                {
                    if (collider.tag == "Monster")
                    {
                        StatManager.instance.att *= 5;
                        collider.GetComponent<Monster>().TakeDamage(StatManager.instance.att);
                        StartCoroutine(SkillCor());
                        AudioManager.instance.PlaySound(transform.position, 2, Random.Range(1.4f, 1.7f), 1);

                        yield return new WaitForSeconds(0.07f);
                        StatManager.instance.att /= 5;


                    }
                }
            }
        }
     

    }


    IEnumerator MoveToTarget(Vector3 targetPosition)
    {
        // 목표 위치로 플레이어를 부드럽게 이동시킵니다.
        float elapsedTime = 0f;
        Vector3 startingPos = transform.position;

        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(startingPos, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * moveSpeed; // moveSpeed에 따라 이동 속도 조절 가능
            yield return null;
        }

        // 정확한 목표 위치로 이동합니다.
        transform.position = targetPosition;
    }

    public void TakeDamage(float damage)
    {
        if (!NeverDie)
        {
            curHp -= damage;
            //StartCoroutine(RedCor());
            CameraShake.instance.Shake();
            StartCoroutine(NeverDieCor());
        }

    }
    IEnumerator RedCor()
    {
        Red.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Red.gameObject.SetActive(false);
    }
    IEnumerator LerpCameraSize(float targetSize)
    {
        float elapsedTime = 0f;
        float currentSize = virtualCamera.m_Lens.OrthographicSize;

        // 크기를 부드럽게 작게 조절
        while (elapsedTime < lerpSpeed)
        {
            float lerpedSize = Mathf.Lerp(currentSize, targetSize, elapsedTime / lerpSpeed);
            virtualCamera.m_Lens.OrthographicSize = lerpedSize;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 1초 대기
        yield return new WaitForSeconds(1f);

        // 크기를 부드럽게 다시 키우기
        elapsedTime = 0f;
        while (elapsedTime < lerpSpeed)
        {
            float lerpedSize = Mathf.Lerp(targetSize, currentSize, elapsedTime / lerpSpeed);
            virtualCamera.m_Lens.OrthographicSize = lerpedSize;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 정확한 목표 크기로 설정
        virtualCamera.m_Lens.OrthographicSize = currentSize;
    }

}
