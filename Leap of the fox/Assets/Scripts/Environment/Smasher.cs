using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smasher : MonoBehaviour
{

    private Rigidbody2D smasherRigidBody;
    public float liftMoveSpeed;
    public Transform upperLimit;
    private bool lifting = false;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        smasherRigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        lifting = false;
    }



    // Update is called once per frame
    void Update()
    {
        if (lifting && transform.position.y >= upperLimit.position.y) {
            lifting = false;
            StartCoroutine(dropSmasher());
        }

        else if(smasherRigidBody.velocity.y == 0 && !lifting)
        {
            lifting = true;
            Debug.Log("Before lifting coroutine");
            StartCoroutine(liftSmasher());
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            audioSource.Play();
        }
    }

    IEnumerator liftSmasher()
    {
        yield return new WaitForSeconds(1.5f);
        smasherRigidBody.gravityScale = 0;
        smasherRigidBody.velocity = new Vector2(0, liftMoveSpeed);
    }

    IEnumerator dropSmasher()
    {
        smasherRigidBody.gravityScale = 0;
        smasherRigidBody.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(1.5f);
        smasherRigidBody.gravityScale = 3;
    }

}
