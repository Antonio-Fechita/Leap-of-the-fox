using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpposumController : MonoBehaviour
{
    [SerializeField] Transform leftBound;
    [SerializeField] Transform rightBound;
    [SerializeField] float speed = 2;
    [SerializeField] float poisonTime;
    [SerializeField] int damageAmount = 4;
    private bool canAttack = true;
    Vector2 destination;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        destination = leftBound.position;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player") && canAttack)
        {
            audioSource.Play();
            StartCoroutine(poisonEffect());
            PlayerHealthController.instance.takeDamage(damageAmount, -5 * Mathf.Sign(transform.localScale.x));
        }
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, destination) < 0.05)
            destination = destination.x == leftBound.position.x ? rightBound.position : leftBound.position;
        transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        if((destination.x == leftBound.position.x && transform.localScale.x < 0) || (destination.x == rightBound.position.x && transform.localScale.x > 0))
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
        }
    }

    IEnumerator poisonEffect()
    {
        canAttack = false;
        float timeElapsed = 0f;
        float originalPlayerMoveSpeed = PlayerController.instance.moveSpeed;
        float originalPlayerJumpForce = PlayerController.instance.jumpForce;


        while (timeElapsed < 2)
        {
            float t = timeElapsed / 2;
            PlayerController.instance.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.green, t);
            PlayerController.instance.moveSpeed = Mathf.Lerp(originalPlayerMoveSpeed, originalPlayerMoveSpeed / 2, t);
            PlayerController.instance.jumpForce = Mathf.Lerp(originalPlayerJumpForce, originalPlayerJumpForce / 2, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(poisonTime);

        timeElapsed = 0;
        while (timeElapsed < 2)
        {
            float t = timeElapsed / 2;
            PlayerController.instance.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.green, Color.white, t);
            PlayerController.instance.moveSpeed = Mathf.Lerp(originalPlayerMoveSpeed / 2, originalPlayerMoveSpeed, t);
            PlayerController.instance.jumpForce = Mathf.Lerp(originalPlayerJumpForce / 2, originalPlayerJumpForce, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        canAttack = true;

    }
}
