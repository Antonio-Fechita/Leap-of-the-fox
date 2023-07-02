using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : MonoBehaviour
{
    FrogController frogController;

    private void Start()
    {
        frogController = transform.parent.parent.GetComponent<FrogController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            frogController.playerHitted = true;
        }
        else if (collision.gameObject.tag != "Enemy")
        {
            Debug.Log(collision.gameObject.name);
            frogController.obstacleHitted = true;
            if(collision.gameObject.tag == "Shield")
            {
                collision.gameObject.GetComponentInParent<Shielding>().BreakShield(5);
            }
        }
    }


}
