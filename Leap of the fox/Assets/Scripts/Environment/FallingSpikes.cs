using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpikes : MonoBehaviour, ResetableObject
{

    Vector2 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
        Respawn.respawnEvent.AddListener(ResetObject);
    }


    void Update()
    {
        if((Mathf.Abs(transform.position.x - PlayerController.instance.transform.position.x) < 0.5) &&
          (transform.position.y > PlayerController.instance.transform.position.y))
            {
                 GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            }
    }

    public void ResetObject()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        transform.position = initialPosition;
    }

}
