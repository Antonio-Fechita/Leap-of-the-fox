using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{

    public int healthPoints;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            int sum = PlayerHealthController.instance.currentHealth + healthPoints;
            if(PlayerHealthController.instance.currentHealth != PlayerHealthController.instance.maxHealth)
            {
                if (sum <= PlayerHealthController.instance.maxHealth)
                {
                    PlayerHealthController.instance.currentHealth = sum;
                }
                else
                {
                    PlayerHealthController.instance.currentHealth = PlayerHealthController.instance.maxHealth;
                }
                Destroy(gameObject);
            }
        }
    }
}
