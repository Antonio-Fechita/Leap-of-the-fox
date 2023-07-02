using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Ladder : MonoBehaviour
{
    [SerializeField] float ladderStickingJumpCooldown = 0.5f; //if the player jumps while on the ladder he shouldn't instantly stick back to the ladder basically canceling his jump
    private bool canStick = true;
    private Coroutine cooldownCoroutine;

    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!PlayerController.instance.onGround && !PlayerController.instance.onLadder && canStick)
            {
                PlayerController.instance.transform.parent = transform;
                PlayerController.instance.onLadder = true;
                PlayerController.instance.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
            if (PlayerController.instance.onGround && PlayerController.instance.onLadder)
            {
                PlayerController.instance.transform.parent = null;
                PlayerController.instance.onLadder = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.instance.transform.parent = null;
            PlayerController.instance.onLadder = false;
            PlayerController.instance.GetComponent<Rigidbody2D>().gravityScale = 5;

            if (cooldownCoroutine != null)
            {
                StopCoroutine(cooldownCoroutine);
                canStick = true;
            }
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && PlayerController.instance.onLadder)
        {
            PlayerController.instance.transform.parent = null;
            PlayerController.instance.onLadder = false;
            PlayerController.instance.GetComponent<Rigidbody2D>().gravityScale = 5;
            PlayerController.instance.GetComponent<Animator>().Play("Player_Jump");
            cooldownCoroutine = StartCoroutine(cooldown());
        }

        if (PlayerController.instance.onGround && cooldownCoroutine!=null)
        {
            StopCoroutine(cooldownCoroutine);
            canStick = true;
        }
    }

    IEnumerator cooldown()
    {
        canStick = false;
        yield return new WaitForSeconds(ladderStickingJumpCooldown);
        canStick = true;

    }
}
