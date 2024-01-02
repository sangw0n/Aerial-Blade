using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerLine : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]
    float speed;

    // 플레이어를 추적할 대상
    Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    void Update()
    {
        // 플레이어를 향해 총알 발사
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }
}
