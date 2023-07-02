using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogMovement : MonoBehaviour
{

    [SerializeField] float followPlayerFromDistance = 20; //If the player is less than 20 units away the frog will start following it
    [SerializeField] float jumpForce = 10;
    [SerializeField] float maxXVelocity = 15;
    [SerializeField] float jumpCooldown = 3;
    [SerializeField] LayerMask groundLayers;
    [SerializeField] Transform feet;
    private bool inCooldown = false;
    private Rigidbody2D frogRigidBody;
    private Animator animator;
    private FrogController frogController;
    private bool onGround;
    private AudioSource audioSource;
    [SerializeField] AudioClip jumpClip;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        frogRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        frogController = GetComponent<FrogController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, PlayerController.instance.transform.position) < followPlayerFromDistance)
        {
            float direction = Mathf.Sign(PlayerController.instance.transform.position.x - transform.position.x);
            if((direction < 0 && transform.localScale.x == -1) || (direction > 0 && transform.localScale.x == 1))
            {
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            }

            if (OnGround() && !inCooldown && !frogController.reachesPlayer && !frogController.attacking)
            {
                jump(direction);
            }
        }

        animator.SetFloat("yVelocity", frogRigidBody.velocity.y);
    }

    IEnumerator startCooldown()
    {
        yield return new WaitForSeconds(jumpCooldown);
        inCooldown = false;
    }

    void jump(float direction)
    {
        frogRigidBody.velocity = new Vector2(direction * 
                                            Mathf.Min(Mathf.Abs(transform.position.x - PlayerController.instance.transform.position.x) * 1.15f, maxXVelocity)
                                            , jumpForce);
        inCooldown = true;
        StartCoroutine(startCooldown());
        audioSource.clip = jumpClip;
        audioSource.Play();
    }



    private bool OnGround()
    {
        return Physics2D.OverlapCircle(feet.position, .05f, groundLayers);
    }

}
