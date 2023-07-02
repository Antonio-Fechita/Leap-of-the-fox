using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformController : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed;
    private float originalMoveSpeed;
    public List<Transform> travelPoints = new List<Transform>();
    [HideInInspector] public int destinationIndex = 1;
    bool forwardDirection = true;
    [SerializeField] bool showPath = false;
    public bool loopPath = false;
    [SerializeField] Material lineMaterial;
    [SerializeField] float delayBetweenPoints = 0f;
    [SerializeField] Lever lever;
    [HideInInspector] public UnityEvent changedDestinationIndex = new UnityEvent();
    void Start()
    {

        if (lever != null)
        {
            lever.upEvent.AddListener(ResetPlatform);
            lever.downEvent.AddListener(ResetPlatform);
        }

        originalMoveSpeed = moveSpeed;
        transform.position = travelPoints[0].position;

        if (showPath)
        {
            for (int index = 0; index<travelPoints.Count - 1; index++)
            {
                DrawBetween2Points(travelPoints[index].position, travelPoints[index + 1].position);
            }
            if (loopPath)
            {
                DrawBetween2Points(travelPoints[0].position, travelPoints[travelPoints.Count - 1].position);
            }
        }
    }

    private void ResetPlatform()
    {
        destinationIndex = 1;
        forwardDirection = true;
        transform.position = travelPoints[0].position;
        transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    private void DrawBetween2Points(Vector2 firstPoint, Vector2 secondPoint)
    {
        LineRenderer lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.SetPosition(0, new Vector3(firstPoint.x, firstPoint.y, 1));
        lineRenderer.SetPosition(1, new Vector3(secondPoint.x, secondPoint.y, 1));
        lineRenderer.material = lineMaterial;
        lineRenderer.transform.parent = transform.parent;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
            col.gameObject.transform.parent = transform;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.gameObject.transform.parent = null;
    }


    IEnumerator Delay()
    {
        moveSpeed = 0;
        yield return new WaitForSeconds(delayBetweenPoints);
        moveSpeed = originalMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, travelPoints[destinationIndex].position) < 0.05)
        {
            if (loopPath)
            {
                changedDestinationIndex.Invoke();
                destinationIndex = (destinationIndex + 1) % travelPoints.Count;
            }
            else
            {
                if ((forwardDirection && destinationIndex == travelPoints.Count - 1) || (!forwardDirection && destinationIndex == 0))
                    forwardDirection = !forwardDirection;
                changedDestinationIndex.Invoke();
                destinationIndex += (forwardDirection ? 1 : -1);
                StartCoroutine(Delay());
            }

            
        }

        transform.position = Vector2.MoveTowards(transform.position, travelPoints[destinationIndex].position, moveSpeed * Time.deltaTime);

    }
}
