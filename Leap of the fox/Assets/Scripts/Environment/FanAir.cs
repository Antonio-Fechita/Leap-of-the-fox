using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanAir : MonoBehaviour
{

    Vector2 localUpDirection = Vector2.up;
    Vector2 globalPushingDirection;
    [SerializeField] float forceWithParachute = 80;
    [SerializeField] float forceWithoutParachute = 1;
    private static float currentForce;
    [SerializeField] float forceIncreaseSpeed = 20;
    private Coroutine increaseForceCoroutine;
    private bool coroutineWorking = false;
    // Start is called before the first frame update
    void Start()
    {
        globalPushingDirection = transform.TransformDirection(localUpDirection);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!Parachute.on)
        {
            if (coroutineWorking == true)
            {
                StopCoroutine(increaseForceCoroutine);
                coroutineWorking = false;
            }
                
            currentForce = forceWithoutParachute;
        }
        else
        {
            if (coroutineWorking == false)
            {
                increaseForceCoroutine = StartCoroutine(IncreaseForce());
            }
        }

        if (collision.CompareTag("Player"))
        {
            collision.transform.GetComponent<Rigidbody2D>().AddForce(globalPushingDirection * currentForce, ForceMode2D.Force);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (coroutineWorking == true)
        {
            StopCoroutine(increaseForceCoroutine);
            coroutineWorking = false;
        }

        currentForce = forceWithoutParachute;
    }

    private IEnumerator IncreaseForce()
    {
        coroutineWorking = true;
        while(currentForce < forceWithParachute)
        {
            currentForce = Mathf.MoveTowards(currentForce, forceWithParachute, forceIncreaseSpeed * Time.deltaTime);
            yield return null;
        }
        coroutineWorking = false;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
