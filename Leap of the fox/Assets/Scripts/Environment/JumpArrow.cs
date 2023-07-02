using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpArrow : MonoBehaviour, ResetableObject
{

    private void Start()
    {
        Respawn.respawnEvent.AddListener(ResetObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().availableNumberOfJumps++;
            gameObject.SetActive(false);
        }
    }


    public void ResetObject()
    {
        gameObject.SetActive(true);
    }
}
