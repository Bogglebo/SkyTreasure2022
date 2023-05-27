using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBlendTree : MonoBehaviour
{
    public Animator animator;
    public int i;

    // Start is called before the first frame update
    void Start()
    {
        i = 1;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("MoveSpeed", 0.4f);
        Debug.Log("int i = " + i);
        if (i < 1000)
        {
            animator.SetBool("IsAttacking", true);
            i++;
        } else
        {
            animator.SetBool("IsAttacking", false);
            animator.SetFloat("MoveSpeed", 0f);
        }
       
       
    }
}
