using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushingObject : MonoBehaviour
{
    public DirectionEnum currentMovingDirection;
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    Vector2 destination;
    [SerializeField] float speed = 2;

    private void Start()
    {
        transform.position = pointA.position;
        destination = pointB.position;

        if(transform.position.x < destination.x)
        {
            currentMovingDirection = DirectionEnum.right;
        }
        else if(transform.position.x > destination.x)
        {
            currentMovingDirection = DirectionEnum.left;
        }
        else if (transform.position.y < destination.y)
        {
            currentMovingDirection = DirectionEnum.up;
        }
        else if (transform.position.y > destination.y)
        {
            currentMovingDirection = DirectionEnum.down;
        }

    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, destination) < 0.05)
        {
            destination = (destination.x == pointA.position.x && destination.y == pointA.position.y) ? pointB.position : pointA.position;
            currentMovingDirection = swapDirection();
        }
        transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        
    }

    private DirectionEnum swapDirection()
    {
        switch (currentMovingDirection)
        {
            case DirectionEnum.right:
                return DirectionEnum.left;
            case DirectionEnum.left:
                return DirectionEnum.right;
            case DirectionEnum.up:
                return DirectionEnum.down;
            default: //case DirectionEnum.down
                return DirectionEnum.up;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.parent = transform;
            collision.transform.localScale = new Vector2(1 / transform.localScale.x, 1 / transform.localScale.y);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.parent = null;
            collision.transform.localScale = new Vector2(1, 1);
        }
    }

}
