using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : MonoBehaviour, ResetableObject
{
    GameObject tongue;
    Transform tongueTip;
    private Transform playerTransform;
    public float tongueSpeed;
    public float swallowedTime;
    public float maxScale;
    public float minScale;
    public bool swallowed = false;
    private Animator playerAnimator;
    private Animator frogAnimator;

    public float cooldown;
    public bool attacking = false;
    public bool playerHitted = false;
    public bool obstacleHitted = false;
    public bool reachesPlayer = false;
    private Vector2 tongueOriginalScale;
    private Vector3 raycastPosition;

    [SerializeField] float visibilityRange;

    private LayerMask layerMask;
    private Rigidbody2D frogRigidBody;
    private Coroutine attackCoroutine;
    private Vector2 initialPosition;

    public static FrogController instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Respawn.respawnEvent.AddListener(ResetObject);
        initialPosition = transform.position;
        tongue = transform.GetChild(0).gameObject;
        tongueTip = transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform;
        tongue.SetActive(false);
        tongueOriginalScale = tongue.transform.localScale;
        playerAnimator = PlayerController.instance.gameObject.GetComponent<Animator>();
        frogAnimator = GetComponent<Animator>();
        raycastPosition = tongue.transform.position;
        raycastPosition.z = 0;

        layerMask = Physics2D.AllLayers;
        layerMask &= ~(1 << LayerMask.NameToLayer("Camera"));
        layerMask &= ~(1 << LayerMask.NameToLayer("Enemy"));
        frogRigidBody = GetComponent<Rigidbody2D>();

        playerTransform = PlayerController.instance.transform;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            reachesPlayer = true;
            raycastPosition = tongue.transform.position;
            raycastPosition.z = 0;

            Vector2 direction = new Vector2(visibilityRange * -Mathf.Sign(transform.localScale.x), 0);
            RaycastHit2D hit = Physics2D.Raycast(raycastPosition, direction, visibilityRange, layerMask);
            Debug.DrawRay(raycastPosition, direction, Color.red);
            if (hit.collider != null)
            {
                if (!attacking && hit.collider.tag == "Player" && frogRigidBody.velocity.y == 0)
                {
                    Debug.Log("Found player");
                    attackCoroutine = StartCoroutine(Attack());
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            reachesPlayer = false;
        }
    }
    IEnumerator Attack()
    {
        attacking = true;
        frogAnimator.SetBool("open-mouth", true);
        tongue.SetActive(true);
        while (!playerHitted && tongue.transform.localScale.x < maxScale && !obstacleHitted)
        {
            yield return new WaitForSeconds(1 / tongueSpeed);
            tongue.transform.localScale = new Vector3(Mathf.Min(tongue.transform.localScale.x + 0.1f, maxScale), 1, 1);
        }

        while (tongue.transform.localScale.x > minScale)
        {
            yield return new WaitForSeconds(1 / tongueSpeed);
            tongue.transform.localScale = new Vector3(Mathf.Max(tongue.transform.localScale.x - 0.1f, minScale), 1, 1);
            if (playerHitted)
            {
                playerTransform.position = new Vector3(tongueTip.position.x, playerTransform.position.y, playerTransform.position.z);
            }
        }

        if (playerHitted)
        {
            swallowed = true;
            PlayerController.instance.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            tongue.SetActive(false);
            frogAnimator.SetBool("swallowing", true);
            frogAnimator.SetBool("open-mouth", false);


            PlayerController.instance.gameObject.SetActive(false);


            yield return new WaitForSeconds(swallowedTime);


            PlayerController.instance.gameObject.SetActive(true);


            Debug.Log(PlayerController.instance.gameObject.name);


            PlayerHealthController.instance.takeDamage(4, -5 * Mathf.Sign(transform.localScale.x));


            frogAnimator.SetBool("swallowing", false);
            playerHitted = false;
            swallowed = false;
        }

        obstacleHitted = false;
        tongue.SetActive(false);
        frogAnimator.SetBool("open-mouth", false);
        yield return new WaitForSeconds(cooldown);
        attacking = false;
    }

    public void ResetObject()
    {
        swallowed = false;
        attacking = false;
        playerHitted = false;
        obstacleHitted = false;
        reachesPlayer = false;
        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);

        frogRigidBody.velocity = new Vector2(0, 0);
        transform.position = initialPosition;
        frogAnimator.SetBool("swallowing", false);
        frogAnimator.SetBool("open-mouth", false);
        tongue.transform.localScale = tongueOriginalScale;
        tongue.SetActive(false);
    }
}
