using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parachute : MonoBehaviour
{
    private float playerOriginalGravityScale;
    private Rigidbody2D playerRigidBody2D;
    [SerializeField] float parachuteGravityScale = 0.5f;
    [SerializeField] float yVelocityTresholdForActivation = 0.5f;
    static public bool on;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        playerRigidBody2D = PlayerController.instance.GetComponent<Rigidbody2D>();
        playerOriginalGravityScale = playerRigidBody2D.gravityScale;
        on = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool turnOnParachuteCondition = playerRigidBody2D.velocity.y < -yVelocityTresholdForActivation && !on;
        bool turnOffParachuteCondition = (playerRigidBody2D.velocity.y > 0 || PlayerController.instance.onGround) && on;

        turnOnParachuteCondition = Input.GetButtonDown("Parachute") && !on;
        turnOffParachuteCondition = Input.GetButtonDown("Parachute") && on;

        if (turnOnParachuteCondition)
        {
            TurnParachuteOn();
        }
        else if(turnOffParachuteCondition)
        {
            TurnParachuteOff();
        }

        if(on)
        {
            if(playerRigidBody2D.velocity.y < 0)
            {
                playerRigidBody2D.gravityScale = parachuteGravityScale;
            }
            else
            {
                playerRigidBody2D.gravityScale = playerOriginalGravityScale;
            }
        }
    }

    private void TurnParachuteOn()
    {
        on = true;
        playerRigidBody2D.velocity = new Vector2(playerRigidBody2D.velocity.x, 0);
        playerRigidBody2D.gravityScale = parachuteGravityScale;
        animator.SetBool("on", on);
    }

    private void TurnParachuteOff()
    {
        on = false;
        playerRigidBody2D.gravityScale = playerOriginalGravityScale;
        animator.SetBool("on", on);
    }
}
