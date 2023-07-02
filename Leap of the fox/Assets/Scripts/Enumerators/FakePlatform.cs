using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlatform : MonoBehaviour
{

    Vector2 initialPosition;
    [SerializeField] float aliveTime = 0.5f;
    [SerializeField] float respawnTime = 10f;
    private Coroutine fallCoroutine;
    private Coroutine respawnCoroutine;
    private Collider2D collider;

    private void Start()
    {
        initialPosition = transform.position;
        Respawn.respawnEvent.AddListener(ResetObject);
        collider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            fallCoroutine = StartCoroutine(platformFall());
        }

    }


    IEnumerator platformFall()
    {
        yield return new WaitForSeconds(aliveTime);
        collider.isTrigger = true;
        transform.gameObject.AddComponent<Rigidbody2D>();
        GetComponent<Rigidbody2D>().gravityScale = 10;
        respawnCoroutine = StartCoroutine(respawnPlatform());
    }

    IEnumerator respawnPlatform()
    {
        yield return new WaitForSeconds(respawnTime);
        ResetObject();
    }

    private void ResetObject()
    {
        KillCoroutines();
        Destroy(GetComponent<Rigidbody2D>());
        transform.rotation = Quaternion.identity;
        transform.position = initialPosition;
        GetComponent<Collider2D>().isTrigger = false;
    }

    private void KillCoroutines()
    {
        if (fallCoroutine != null)
            StopCoroutine(fallCoroutine);
        if (respawnCoroutine != null)
            StopCoroutine(respawnCoroutine);
    }
}
