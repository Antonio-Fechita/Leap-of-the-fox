using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    private bool isUp;
    private bool touchingLever;
    [HideInInspector]
    public UnityEvent upEvent = new UnityEvent();
    [HideInInspector]
    public UnityEvent downEvent = new UnityEvent();
    private SpriteRenderer spriteRenderer;
    [SerializeField] Sprite upSprite;
    [SerializeField] Sprite downSprite;
    [SerializeField] bool invokeDownEventAtStart = true;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        isUp = false;
        touchingLever = false;
        spriteRenderer.sprite = downSprite;
        if(invokeDownEventAtStart)
            StartCoroutine(delayedStartInvoke());
    }

    IEnumerator delayedStartInvoke()
    {
        yield return new WaitForEndOfFrame();
        downEvent.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            touchingLever = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            touchingLever = false;
        }
    }


    private void Update()
    {
        if (Input.GetButtonDown("Interact") && touchingLever)
        {
            audioSource.Play();
            isUp = !isUp;

            if (isUp)
            {
                upEvent.Invoke();
                spriteRenderer.sprite = upSprite;
            }
            else
            {
                downEvent.Invoke();
                spriteRenderer.sprite = downSprite;
            }
        }
    }
}
