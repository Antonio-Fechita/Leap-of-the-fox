using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    private List<string> states = new List<string> { "iddle", "attacking", "walking" };
    protected Animator animator;
    protected Transform raycastTransform;
    [SerializeField]
    protected Transform initialPoint;
    [SerializeField]
    protected Transform destinationPoint;
    [SerializeField]
    protected float iddleTime;
    [SerializeField]
    protected float visibilityRange;
    [SerializeField] 
    protected float moveSpeed;
    public float cooldown;

    [SerializeField] AudioClip[] footsteps;
    private AudioSource audioSource;

    public bool inCooldown = false;
    protected Transform currentTarget;
    protected Rigidbody2D rigidBody;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        raycastTransform = transform.GetChild(0);

        animator.SetInteger("currentStateIndex", 0);
        currentTarget = initialPoint;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 direction = new Vector2(visibilityRange * Mathf.Sign(transform.localScale.x), 0);
        RaycastHit2D hit = Physics2D.Raycast(raycastTransform.position, direction, visibilityRange, LayerMask.GetMask("Player"));
        Debug.DrawRay(raycastTransform.position, direction, Color.red);

        if(hit.collider != null && !inCooldown) //skeleton detects player
        {
            rigidBody.velocity = Vector2.zero;
            if(states[animator.GetInteger("currentStateIndex")] != "attacking")
            {
                animator.SetInteger("currentStateIndex", 1); //animator set on attacking
            }
        }
        else //skeleton doesn't detect player. should either walk or wait in iddle animation in one of the points
        {
            if (reachedPoint(currentTarget) && states[animator.GetInteger("currentStateIndex")] != "iddle")
            {
                rigidBody.velocity = Vector2.zero;
                animator.SetInteger("currentStateIndex", 0); //animator set on iddle
                StartCoroutine(SwitchTarget());
            }
            else if(!reachedPoint(currentTarget) /*&& states[animator.GetInteger("currentStateIndex")] != "walking"*/)
            {
                animator.SetInteger("currentStateIndex", 2); //animator set on walking
                transform.localScale = new Vector2(Mathf.Sign(currentTarget.position.x - transform.position.x), 1);
                rigidBody.velocity = new Vector2(Mathf.Sign(currentTarget.position.x - transform.position.x) * moveSpeed, 0);
            }
        }


        //audio stuff
        if(states[animator.GetInteger("currentStateIndex")] == "walking" && !audioSource.isPlaying)
        {
            audioSource.clip = footsteps[Random.Range(0, footsteps.Length)];
            audioSource.Play();
        }

    }

    IEnumerator SwitchTarget()
    {
        yield return new WaitForSeconds(iddleTime);
        currentTarget = currentTarget == initialPoint ? destinationPoint : initialPoint;
    }


    private bool reachedPoint(Transform point)
    {
        return Mathf.Abs(transform.position.x - point.position.x) < 0.05f;
    }

}
