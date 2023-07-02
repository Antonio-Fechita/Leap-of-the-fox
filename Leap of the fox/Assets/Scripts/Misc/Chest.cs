using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator animator;
    [SerializeField] List<GameObject> contents;
    private SpriteRenderer spriteRenderer;
    [SerializeField] Sprite openSprite;
    private bool isOpen = false;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        foreach(GameObject gift in contents)
        {
            gift.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOpen)
        {
            isOpen = true;
            StartCoroutine(OpenChest());

        }
        
    }

    IEnumerator OpenChest()
    {
        animator.SetTrigger("open");
        yield return new WaitUntil(() => spriteRenderer.sprite == openSprite);


        foreach (GameObject gift in contents)
        {
            gift.SetActive(true);
        }
    }
}
