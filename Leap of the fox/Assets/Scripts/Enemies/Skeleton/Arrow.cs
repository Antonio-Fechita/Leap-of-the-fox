using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    bool touchedSomething = false;
    public int damageValue;
    public float secondUntilDespawn;
    public AudioClip hitClip;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!touchedSomething)
        {
            if (collision.collider.gameObject.tag == "Player")
            {
                PlayerHealthController.instance.takeDamage(damageValue);
                /*Destroy(gameObject);*/
            }

            else if(collision.collider.gameObject.tag == "Shield")
            {
                collision.gameObject.GetComponentInParent<Shielding>().BreakShield(5);
                Debug.Log("Hit shield");
                Destroy(gameObject);
            }

            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = hitClip;
            audioSource.Play();
            touchedSomething = true;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Destroy(gameObject.GetComponent<Rigidbody2D>());
            Destroy(gameObject.GetComponent<BoxCollider2D>());
            gameObject.transform.parent = collision.gameObject.transform;
        }


    }

    private void Start()
    {
        gameObject.layer = 8; //EnemyProjectile layer
        damageValue = Shooting.instance.arrowDamage;
        secondUntilDespawn = Shooting.instance.secondsUntilArrowDespawn;
        StartCoroutine(Despawn());
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(secondUntilDespawn);
        Destroy(gameObject);
    }
}
