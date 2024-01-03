using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    [SerializeField]
    GameObject Dashpt;
    bool isdash = false;
    public Vector2 inputVec;

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
    public float Skill2curTime;
    
    public float DashcurTime;
    [SerializeField] public float HitDamage = 1;
    public float coolTime = 0.5f;
    public float SkillcoolTime = 5f;
    public float Skill2coolTime = 5f;

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


    private bool isSlashPtc1Active = true;
    private bool isSideAttack1 = true;  // sideattack과 sideattack2를 번갈아가며 발동하기 위한 변수

    void Start()
    {
        curHp = maxHp;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Hpbar.value = Mathf.Lerp(Hpbar.value, (float)curHp / (float)maxHp, Time.deltaTime * 4); ;
        Move();
        Skill();
        Dash();
        Skill2();
        if (curTime <= 0)
        {
            if (Input.GetKey(KeyCode.X))
            {
                AudioManager.instance.PlaySound(transform.position, 0, Random.Range(2.0f, 2.0f), 1);

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
                    if (collider.tag == "Monster")
                    {
                        collider.GetComponent<Monster>().TakeDamage(HitDamage);
                    }
                    if (collider.tag == "BossMonster")
                    {
                        collider.GetComponent<MiniBossOne>().TakeDamage(HitDamage);
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
            if (Input.GetKey(KeyCode.A))
            {
                anim.SetTrigger("SKill1");
                StartCoroutine(SkillCor());
                SkillcurTime = SkillcoolTime; // 여기서 SkillcurTime을 초기화해야 합니다.
            }
        }
        else
        {
            SkillcurTime -= Time.deltaTime;
        }
    }
    void Skill2()
    {
        if (Skill2curTime <= 0)
        {

            if (Input.GetKey(KeyCode.S))
            {
                StartCoroutine(SKill2Cor());
                Skill2curTime = Skill2coolTime;

            }
            
        }
        else
        {
            Skill2curTime -= Time.deltaTime;
        }

    }

    IEnumerator SkillCor()
    {
        for (int i = 0; i < 5; i++)
        {
            Destroy(Instantiate(SkillPtc, transform.position, Quaternion.identity), 3f);
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(Skillpos.position, skillboxSize, 0);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.tag == "Monster")
                {
                    collider.GetComponent<Monster>().TakeDamage(1);
                }
                if (collider.tag == "BossMonster")
                {
                    collider.GetComponent<MiniBossOne>().TakeDamage(1);

                }
            }
            yield return new WaitForSeconds(0.08f);
        }
    }

    void Dash()
    {
        if (DashcurTime <= 0)
        {
            if (Input.GetKeyDown(KeyCode.C))
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
        GetComponent<Collider2D>().isTrigger = true;
        yield return new WaitForSeconds(1f);
        GetComponent<Collider2D>().isTrigger = false;
    }
   IEnumerator SKill2Cor()
    {
        for (int i = 0; i < 10; i++)
        {
            AudioManager.instance.PlaySound(transform.position, 0, Random.Range(2.0f, 2.0f), 1);

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
                if (collider.tag == "Monster")
                {
                    collider.GetComponent<Monster>().TakeDamage(HitDamage);
                }
                if (collider.tag == "BossMonster")
                {
                    collider.GetComponent<MiniBossOne>().TakeDamage(HitDamage);
                }
            }


            isSlashPtc1Active = !isSlashPtc1Active; // 번갈아가면서 활성화 여부를 변경
            isSideAttack1 = !isSideAttack1;  // sideattack1과 sideattack2를 번갈아가면서 발동하기 위해 변경
            yield return new WaitForSeconds(0.07f);
        }
        
        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.PlaySound(transform.position, 0, Random.Range(2.0f, 2.0f), 1);
        if (transform.localScale.x < 0)
        {
            Destroy(Instantiate(POWER, transform.position + new Vector3(-0.5f, 0f, 0), Quaternion.identity), 3f);

            // sideattack1 또는 sideattack2 트리거 발동
            anim.SetTrigger("SideAttack");
        }
        if (transform.localScale.x > 0)
        {
            Destroy(Instantiate(POWER, transform.position + new Vector3(0.5f, 0f, 0), Quaternion.Euler(new Vector3(0, 180, 0))), 3f);

            // sideattack1 또는 sideattack2 트리거 발동
            anim.SetTrigger("SideAttack");
        }
        Collider2D[] collider2D = Physics2D.OverlapBoxAll(Skill2pos.position, skill2boxSize, 0); ;
        foreach (Collider2D collider in collider2D)
        {
            
            if (collider.tag == "Monster")
            {
                StatManager.instance.baseAtt *= 5;
                collider.GetComponent<Monster>().TakeDamage(HitDamage);
                yield return new WaitForSeconds(0.1f);
                StatManager.instance.baseAtt /= 5;


            }
            if (collider.tag == "BossMonster")
            {
                StatManager.instance.baseAtt *= 5;
                collider.GetComponent<MiniBossOne>().TakeDamage(HitDamage);
                yield return new WaitForSeconds(0.1f);
                StatManager.instance.baseAtt /= 5;

            }
           


        }
       ;


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MonsterAttack")
        {
            curHp--;
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
}

