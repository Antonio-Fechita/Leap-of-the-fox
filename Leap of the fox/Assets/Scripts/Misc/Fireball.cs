using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] Transform firstPoint;
    [SerializeField] Transform secondPoint;
    [SerializeField] float cooldownBetweenJumps = 0.5f;
    [SerializeField] float speed = 5;
    [SerializeField] float arcHeight = 5;
    [SerializeField] float delay = 0;
    private Vector2 nextPosition;
    private Vector2 startingPoint;
    private Vector2 targetPoint;

    public void Start()
    {
        transform.position = firstPoint.position;
        startingPoint = firstPoint.position;
        targetPoint = secondPoint.position;
        StartCoroutine(Jump());
    }


    IEnumerator Jump()
    {
        yield return new WaitForSeconds(delay);
        // Compute the next position, with arc added in
        while (true)
        {
            float xDistance = Mathf.Abs(startingPoint.x - targetPoint.x);
            float nextX = Mathf.MoveTowards(transform.position.x, targetPoint.x, speed * Time.deltaTime);
            float baseY = Mathf.Lerp(startingPoint.y, targetPoint.y, (nextX - startingPoint.x) / xDistance);
            float arc = arcHeight * (nextX - startingPoint.x) * (nextX - targetPoint.x) / (-0.25f * xDistance * xDistance);
            nextPosition = new Vector2(nextX, baseY + arc);
            /*
                    // Rotate to face the next position, and then move there
                    transform.rotation = LookAt2D(nextPos - transform.position);*/
            transform.position = nextPosition;

            if(Vector2.Distance(transform.position, targetPoint) < 0.05)
            {
                transform.position = targetPoint;
                startingPoint = new Vector2(targetPoint.x, targetPoint.y);
                targetPoint = (targetPoint == (Vector2)firstPoint.position) ? secondPoint.position : firstPoint.position;
                yield return new WaitForSeconds(cooldownBetweenJumps);
            }

            yield return null;
        }

    }

}
