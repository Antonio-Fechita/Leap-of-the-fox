using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    [SerializeField] float bouncePower = 20;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.transform.GetComponent<Rigidbody2D>().velocity.x, bouncePower);
            animator.SetTrigger("Touched");
        }
    }
}
