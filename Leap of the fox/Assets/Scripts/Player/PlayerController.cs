using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    [SerializeField] float climpSpeed;
    public float jumpForce;
    public int maxNumberOfJumps = 2;
    public Rigidbody2D playerRigidbody;
    public Transform playerFeet; //used to check if the player's feet are touching the ground
    public LayerMask groundLayers; //layers that the player can jump from
    public Animator animator;
    public Vector2 knockbackVelocity = new Vector2(0, 0); //used when the player takes damage
    public bool isTakingDamage = false; //todo: replace with isTakingDamage
    public int availableNumberOfJumps;
    public bool dashing = false;
    public bool onLadder = false;
    public bool hasBootsPowerup = false; //Player can buy boots powerup from the store that removes movement inertia

    public bool onGround;
    private bool recentlyJumped = false;
    private Transform playerTransform;


    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

        animator = GetComponent<Animator>();
        playerTransform = GetComponent<Transform>();
    }

    void Update()
    {
        float axisHorizontal = Mathf.Clamp(Input.GetAxis("Horizontal") + ControllerInput.instance.GetAxisHorizontal(), -1, 1);
        float axisHorizontalRaw = Mathf.Clamp(Input.GetAxisRaw("Horizontal") + ControllerInput.instance.GetAxisRawHorizontal(), -1, 1);
        float axisVerticalRaw = Mathf.Clamp(Input.GetAxisRaw("Vertical") + ControllerInput.instance.GetAxisRawVertical(), -1, 1);

        float horizontalAxisToUse;

        if (hasBootsPowerup)
            horizontalAxisToUse = axisHorizontalRaw;
        else
            horizontalAxisToUse = axisHorizontal;

        bool swallowed = false;
        if( FrogController.instance != null)
            swallowed = FrogController.instance.swallowed;
        if (!isTakingDamage && !dashing && !swallowed)
        {
            if(!onLadder)
            {
                animator.speed = 1;
                playerRigidbody.velocity = new Vector2(moveSpeed * horizontalAxisToUse, playerRigidbody.velocity.y) + knockbackVelocity;
            }
            else
            {
                animator.SetBool("onLadder", true);
                animator.Play("Player_climb");
                animator.speed = Mathf.Abs(axisVerticalRaw);
                playerRigidbody.velocity = new Vector2(climpSpeed * axisHorizontalRaw, climpSpeed * axisVerticalRaw) + knockbackVelocity;
            }

            if (axisHorizontalRaw != 0)
                playerTransform.localScale = new Vector3(Mathf.Sign(axisHorizontalRaw) * Mathf.Abs(playerTransform.localScale.x), 
                    playerTransform.localScale.y, playerTransform.localScale.z);


        }


        onGround = checkIfThePlayerIsOnGround();

        if (onGround || onLadder)
        {
            availableNumberOfJumps = maxNumberOfJumps;
        }

        if (Input.GetButtonDown("Jump") && availableNumberOfJumps > 0 && !dashing && !swallowed){ 
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpForce);
            availableNumberOfJumps--;
            StartCoroutine(recentlyJumpedTimer());
        }

        animator.SetFloat("moveSpeed", Mathf.Abs(playerRigidbody.velocity.x));
        animator.SetBool("onGround", onGround);
        animator.SetBool("onLadder", onLadder);
    }

    public bool checkIfThePlayerIsOnGround()
    {
        if (!recentlyJumped)
            return Physics2D.OverlapCircle(playerFeet.position, .05f, groundLayers);
        else
            return false;
    }


    IEnumerator recentlyJumpedTimer()
    {
        recentlyJumped = true;
        yield return new WaitForSeconds(0.2f);
        recentlyJumped = false;
    }
}
