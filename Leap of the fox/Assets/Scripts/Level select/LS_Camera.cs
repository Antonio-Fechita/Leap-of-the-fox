using UnityEngine;

public class LS_Camera : MonoBehaviour
{
    [SerializeField] Transform head;
    [SerializeField] Transform upperRightBound;
    [SerializeField] Transform lowerLeftBound;
    [SerializeField] float xOffset = 0.2f;
    [SerializeField] float yOffset = 0.2f;

    private void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(head.position.x + xOffset, lowerLeftBound.position.x, upperRightBound.position.x),
                                         Mathf.Clamp(head.position.y + yOffset, lowerLeftBound.position.y, upperRightBound.position.y), -5);
    }
}
