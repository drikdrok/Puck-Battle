using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdAnimation : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        int n = Random.Range(0, 3);
        if (n == 0)
        {
            animator.SetTrigger("Idle");
        }else if (n == 1)
        {
            animator.SetTrigger("Shout");
        }else if (n == 2)
        {
            animator.SetTrigger("Wave");
        }
        
        yield return new WaitForSeconds(Random.Range(1, 10));
        StartCoroutine(PlayAnimation());
    }
}
