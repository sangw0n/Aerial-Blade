using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]
    float movespeed;
    Animator anim;
    float curTime;
    public float coolTime = 0.5f;
    public Transform pos;
    public Vector2 boxSize;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        
            if (curTime <= 0)
            {
                if (Input.GetKey(KeyCode.X))   
                {

                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
                foreach (Collider2D collider in collider2Ds)
                {
                    if(collider.tag == "Monster")
                    {
                        collider.GetComponent<Monster>().TakeDamage(1);
                    }
                }
             
                curTime = coolTime;
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
    }
    void Move()
    {
        float x = Input.GetAxis("Horizontal") * movespeed;

        float y = Input.GetAxis("Vertical") * movespeed;

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

        rb.velocity = new Vector2(x, rb.velocity.y);
        rb.velocity = new Vector2(rb.velocity.x, y);
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
}
