using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed;
    Transform player;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player != null)
        {
            // �÷��̾� ���ϴ� ���� 
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * speed;

        
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            rb.velocity = Vector2.left;
        }
    }

    void Update()
    {
      
    }

   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            Destroy(gameObject);
    }
}
