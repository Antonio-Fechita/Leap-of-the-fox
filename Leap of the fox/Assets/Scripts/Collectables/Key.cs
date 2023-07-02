using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, ResetableObject
{
    private bool acquired = false;
    [SerializeField] Transform upperLimit;
    [SerializeField] Transform lowerLimit;
    private bool goingUp = true;
    [SerializeField] float speed;
    public ColorEnum color;
    private Vector2 position;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !acquired)
        {
            acquired = true;
            PlayerController.instance.GetComponent<KeyManager>().AddKey(this);
        }
    }


    private void Update()
    {
        if (!acquired)
        {
            Vector2 destination = goingUp ? upperLimit.position : lowerLimit.position;
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            if(Vector2.Distance(transform.position, destination) < 0.05)
            {
                goingUp = !goingUp;
            }
        }
        else
        {
            transform.position = Vector2.Lerp(transform.position, PlayerController.instance.transform.position, speed * Time.deltaTime);
        }
    }

    public void ResetObject()
    {
        throw new System.NotImplementedException();
    }
}
