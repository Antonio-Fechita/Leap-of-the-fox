using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    private List<Transform> travelPoints;
    private PlatformController platformController;
    public float timeToRotate;
    private Coroutine rotateCoroutine;
    private bool rotating = false;

    private void Start()
    {
        platformController = GetComponent<PlatformController>();
        platformController.changedDestinationIndex.AddListener(OnChangedDestinationPoint);
        travelPoints = platformController.travelPoints;
    }


    private void OnChangedDestinationPoint()
    {
        Vector2 anteriorDestinationPoint = (platformController.destinationIndex == 0) ? travelPoints[travelPoints.Count - 1].position :
                                                                                        travelPoints[platformController.destinationIndex - 1].position;
        Vector2 currentDestinationPoint = travelPoints[platformController.destinationIndex].position;

        Vector2 futureDestinationPoint = (platformController.destinationIndex == travelPoints.Count - 1) ? travelPoints[0].position :
                                                                                        travelPoints[platformController.destinationIndex + 1].position;
        float angle = Vector2.SignedAngle(currentDestinationPoint - anteriorDestinationPoint, futureDestinationPoint - currentDestinationPoint);
                                //* ((futureDestinationPoint.x <= currentDestinationPoint.x) ? -1 : 1);

        //transform.Rotate(0, 0, angle);
        if(!rotating)
            rotateCoroutine = StartCoroutine(RotatePart(angle));
        else
        {
            Debug.LogError("suprapunere");
        }
    }


    IEnumerator RotatePart(float angle)
    {
        rotating = true;
        float initialAngle = transform.eulerAngles.z;
        float targetAngle = angle + initialAngle;
        float timeElapsed = 0;
        while(timeElapsed < timeToRotate)
        {
            float t = timeElapsed / timeToRotate;
            transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(initialAngle, targetAngle, t));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.eulerAngles = new Vector3(0, 0, targetAngle);
        rotating = false;
    }

}
