using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringAttack : MonoBehaviour
{
    private RopeBridge spiderString;
    private KeepTrackOfPlayer keepTrackOfPlayer;
    private bool downOnString;
    private SpringJoint2D joint;
    private Rigidbody2D spiderRigidBody;
    private float stringRequiredLength;
    private Spider spider;
    private bool inCooldown = false;
    private bool onString = false;
    private bool playerCatched = false;
    private bool climbingUp = false;
    private LineRenderer stringRenderer;
    [SerializeField] float timePlayerStuck = 2;
    [SerializeField] Rigidbody2D anchor;
    [SerializeField] float stringClimbSpeed = 5;
    [SerializeField] float cooldown = 5;


    private void Start()
    {
        stringRenderer = GetComponentInParent<LineRenderer>();
        spider = GetComponent<Spider>();
        spiderString = GetComponentInParent<RopeBridge>();
        keepTrackOfPlayer = GetComponent<KeepTrackOfPlayer>();
        joint = GetComponent<SpringJoint2D>();
    }

    public void Attack()
    {
        anchor.gameObject.transform.position = transform.position;
        spiderString.StartPoint = anchor.transform;
        spiderString.EndPoint = transform;
        spider.isActive = false;
        spiderString.ApplyConstraint();
        spiderString.isActive = true;


        spiderRigidBody = transform.gameObject.AddComponent<Rigidbody2D>();
        //spiderRigidBody.drag = 0.3f;
        spiderRigidBody.drag = 0;


        joint = transform.gameObject.AddComponent<SpringJoint2D>();
        joint.autoConfigureDistance = false;
        joint.connectedBody = anchor;
        spiderString.ropeSegLen = 0;

        StartCoroutine(ClimbStringDown());
    }

    IEnumerator ClimbStringDown()
    {
        onString = true;
        stringRenderer.enabled = true;
        Debug.Log("ropeSegLen: " + spiderString.ropeSegLen);
        Debug.Log("stringRequiredLength: " + stringRequiredLength);
        Debug.Log("stringRequiredLength / spiderString.segmentLength: " + stringRequiredLength / spiderString.segmentLength);
        
        while (Mathf.Abs(spiderString.totalLength - stringRequiredLength) > 0.05)
        {
            spiderString.ropeSegLen = Mathf.MoveTowards(spiderString.ropeSegLen, stringRequiredLength / spiderString.segmentLength, stringClimbSpeed * Time.deltaTime);
            yield return null;
        }
        spiderRigidBody.velocity = new Vector2(8, 0);

        yield return new WaitForSeconds(2);
        StartCoroutine(ClimbStringUp());
    }

    IEnumerator ClimbStringUp()
    {
        climbingUp = true;
        spiderRigidBody.drag = 1;
        spiderRigidBody.mass = 10;
        while (spiderString.totalLength > 0.05)
        {
            spiderString.ropeSegLen = Mathf.MoveTowards(spiderString.ropeSegLen, 0, stringClimbSpeed * Time.deltaTime);
            yield return null;
        }

        onString = false;
        stringRenderer.enabled = false;
        
        Destroy(joint);
        Destroy(spiderRigidBody);
        transform.position = anchor.position;
        spiderString.isActive = false;
        spider.isActive = true;

        yield return new WaitForSeconds(cooldown);
        inCooldown = false;
    }

    IEnumerator CatchPlayerTimer()
    {
        yield return new WaitForSeconds(timePlayerStuck);
        playerCatched = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && onString && !playerCatched)
        {
            playerCatched = true;
            
        }
    }

    private void Update()
    {
        if (joint != null)
        {
            joint.distance = spiderString.totalLength;
            //Debug.Log("Joint: " + joint.distance);
        }
            
        stringRequiredLength = anchor.position.y - PlayerController.instance.transform.position.y;


        if(keepTrackOfPlayer.seeingPlayer && transform.position.y > PlayerController.instance.transform.position.y &&
            Mathf.Abs(transform.position.x - PlayerController.instance.transform.position.x) < 3 && !inCooldown)
        {
            inCooldown = true;
            Debug.Log("Could attack player");
            Attack();
        }

    }
}
