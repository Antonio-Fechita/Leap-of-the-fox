using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltedPlatform : MonoBehaviour
{
    private bool touchingPlayer = false;
    private float originalMoveSpeed;
    private bool inCooldown = false;
    private Coroutine resetRotationCoroutine;
    private Rigidbody2D platformRigidBody;
    [SerializeField] float resetRotationCooldown = 5f;

    private void Start()
    {
        platformRigidBody = GetComponent<Rigidbody2D>();
        originalMoveSpeed = PlayerController.instance.moveSpeed;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Player"))
            touchingPlayer = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            touchingPlayer = false;
            PlayerController.instance.moveSpeed = originalMoveSpeed;
            PlayerController.instance.transform.eulerAngles = Vector3.zero;
            if (resetRotationCoroutine != null)
                StopCoroutine(resetRotationCoroutine);
            resetRotationCoroutine = StartCoroutine(ResetRotationCooldown());
        }

    }

    IEnumerator ResetRotationCooldown()
    {
        inCooldown = true;
        yield return new WaitForSeconds(resetRotationCooldown);
        platformRigidBody.Sleep();
        transform.eulerAngles = Vector3.zero;
        platformRigidBody.WakeUp();
        inCooldown = false;
    }



    void Update()
    {
        if (touchingPlayer)
        {
            PlayerController.instance.transform.eulerAngles = new Vector3(0, 0, 0);
            if (!PlayerController.instance.checkIfThePlayerIsOnGround())
                PlayerController.instance.moveSpeed = 0;
            else
                PlayerController.instance.moveSpeed = originalMoveSpeed;
        }
    }
}
