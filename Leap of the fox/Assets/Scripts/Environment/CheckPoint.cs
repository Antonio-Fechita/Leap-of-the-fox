using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool isOn = false;
    private SpriteRenderer spriteRenderer;
    public Sprite onSprite;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !isOn)
        {
            isOn = true;
            spriteRenderer.sprite = onSprite;
            Respawn.instance.currentCheckPoint = transform;
        }
    }

}
