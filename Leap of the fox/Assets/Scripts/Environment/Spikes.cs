using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{

    private PlayerHealthController playerHealthController;
    public int damageValue;

    private void Start()
    {
        playerHealthController = PlayerHealthController.instance;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerHealthController.takeDamage(damageValue);
        }
        else if (collision.gameObject.tag == "Shield")
        {
            collision.gameObject.GetComponentInParent<Shielding>().BreakShield(5);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerHealthController.takeDamage(damageValue);
        }
        else if(collision.gameObject.tag == "Shield")
        {
            collision.gameObject.GetComponentInParent<Shielding>().BreakShield(5);
        }
    }

}
