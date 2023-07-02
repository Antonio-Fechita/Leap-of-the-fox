using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SemiSolidObject : MonoBehaviour
{
    private bool isSolid = true;


    private void OnCollisionStay2D(Collision2D collision)
    {
        if((Input.GetAxisRaw("Vertical") > 0 && collision.transform.position.y < transform.position.y) ||
            (Input.GetAxisRaw("Vertical") < 0 && collision.transform.position.y > transform.position.y))
        {
            GetComponent<Collider2D>().isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<Collider2D>().isTrigger = false;
    }


}
