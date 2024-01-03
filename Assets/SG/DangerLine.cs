using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerLine : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]
    float speed;

    // �÷��̾ ������ ���
    Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    void Update()
    {
        // �÷��̾ ���� �Ѿ� �߻�
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }
}
