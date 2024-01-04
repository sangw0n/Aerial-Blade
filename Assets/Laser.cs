using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Vector2 boxSize;
    public Transform boxpos;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Lasers();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxpos.position, boxSize);




    }
    void Lasers() { 
  
    Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(boxpos.position, boxSize, 0);

        foreach (Collider2D collider in collider2Ds)
        {
            if (collider != null)
            {
                if (collider.tag == "Player")
                {

                    collider.GetComponent<Player>().TakeDamage(50);
                }
            }
        }
      
    }
}
