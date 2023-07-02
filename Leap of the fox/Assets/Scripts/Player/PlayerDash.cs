using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private Rigidbody2D playerRB;

    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerRB.velocity = new Vector2(30 * PlayerController.instance.gameObject.transform.localScale.x, 0);
            PlayerController.instance.dashing = true;
            playerRB.gravityScale = 0f;
            StartCoroutine(DashingTimer());
        }
    }

    IEnumerator DashingTimer()
    {
        yield return new WaitForSeconds(0.2f);
        PlayerController.instance.dashing = false;
        playerRB.gravityScale = 5;
        /*animator.SetBool("Dash", false);*/
    }
}
