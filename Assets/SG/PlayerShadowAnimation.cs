using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadowAnimation : MonoBehaviour
{
    Animator anim;
    private bool isSideAttack1 = true;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
        if (Input.GetKey(KeyCode.X))
        {
            anim.SetTrigger(isSideAttack1 ? "SideAttack" : "SideAttack2");
            isSideAttack1 = !isSideAttack1;
        }
      



    }
}
