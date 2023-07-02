using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    public Transform checkPoint;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.activeSelf)
        {
            PlayerHealthController.instance.takeDamage(PlayerHealthController.instance.maxHealth);
        }
    }
}
