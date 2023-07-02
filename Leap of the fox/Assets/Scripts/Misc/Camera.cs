using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float offsetX;
    [SerializeField] float offsetY;
    [SerializeField] float maxOffsetX = 5;
    [SerializeField] float maxOffsetY = 5;
    [SerializeField] float cursorFollowingSpeed = 100;
    private Transform playerTransform;
    private Transform cameraTransform;
    public Transform farBG, closeBG;
    public float farBGSpeed, closeBGSpeed;
    private Vector3 oldPosition;
    public float minHeight, maxHeight;
    private Vector2 screenPositionOfCursor;
    private Vector2 worldPositionOfCursor;
    private Vector2 worldPositionOfJoystick;
    private bool locked = true; //camera is locked on player
    private UnityEngine.Camera camera;
    [SerializeField] float cameraDefaultSize = /*5f;*/ 8.5f;
    [SerializeField] float cameraMinSize = 3f;
    [SerializeField] float cameraMaxSize = 8.5f;
    [SerializeField] float zoomSpeed = 2f;
    private float cameraTargetSize;

    void Start()
    {
        camera = GetComponent<UnityEngine.Camera>();
        camera.orthographicSize = cameraDefaultSize;
        cameraTargetSize = camera.orthographicSize;
        playerTransform = PlayerController.instance.transform;
        cameraTransform = GetComponent<Transform>();
        cameraTransform.position = new Vector3(playerTransform.position.x + offsetX, Mathf.Clamp(playerTransform.position.y + offsetY, minHeight, maxHeight), 
                                               cameraTransform.position.z);
        oldPosition = cameraTransform.position;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 playerFollowingCameraPosition = new Vector3(playerTransform.position.x + offsetX, Mathf.Clamp(playerTransform.position.y + offsetY, minHeight, maxHeight),
                                                   cameraTransform.position.z);

        Vector2 controllerRightJoystick = new Vector2(Input.GetAxisRaw("Secondary Horizontal"), Input.GetAxisRaw("Secondary Vertical"));
        //Debug.Log("Horizontal: " + controllerRightJoystick.x + ", Vertical: " + controllerRightJoystick.y);

        if (Input.GetMouseButton(1)) //using mouse right button to look around
        {
            locked = false;
            screenPositionOfCursor = Input.mousePosition;
            worldPositionOfCursor = UnityEngine.Camera.main.ScreenToWorldPoint(screenPositionOfCursor);

            Vector3 positionToFollow = new Vector3(Mathf.Clamp(worldPositionOfCursor.x, playerFollowingCameraPosition.x - maxOffsetX, playerFollowingCameraPosition.x + maxOffsetX),
                                                   Mathf.Clamp(worldPositionOfCursor.y, playerFollowingCameraPosition.y - maxOffsetY, playerFollowingCameraPosition.y + maxOffsetY),
                                                   cameraTransform.position.z);

            cameraTransform.position = Vector3.Lerp(cameraTransform.position, positionToFollow, cursorFollowingSpeed * Time.deltaTime);

        }

        else if (controllerRightJoystick.x != 0 || controllerRightJoystick.y != 0) //using right joystick to look around
        {
            locked = false;
            worldPositionOfJoystick = new Vector2(Mathf.Lerp(playerFollowingCameraPosition.x - maxOffsetX, playerFollowingCameraPosition.x + maxOffsetX, (controllerRightJoystick.x + 1) / 2),
                                                  Mathf.Lerp(playerFollowingCameraPosition.y - maxOffsetY, playerFollowingCameraPosition.y + maxOffsetY, (controllerRightJoystick.y + 1) / 2));

            Vector3 positionToFollow = new Vector3(Mathf.Clamp(worldPositionOfJoystick.x, playerFollowingCameraPosition.x - maxOffsetX, playerFollowingCameraPosition.x + maxOffsetX),
                                       Mathf.Clamp(worldPositionOfJoystick.y, playerFollowingCameraPosition.y - maxOffsetY, playerFollowingCameraPosition.y + maxOffsetY),
                                       cameraTransform.position.z);

            cameraTransform.position = Vector3.Lerp(cameraTransform.position, positionToFollow, cursorFollowingSpeed * Time.deltaTime);

        }

        else
        {
            if (Vector3.Distance(cameraTransform.position, playerFollowingCameraPosition) > 0.1f && !locked)
            {
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, playerFollowingCameraPosition, cursorFollowingSpeed * 10 * Time.deltaTime);
            }
            else
            {
                locked = true;
            }
            if(locked)
            {
                if (playerTransform != null)
                    cameraTransform.position = playerFollowingCameraPosition;
                
            }


        }
        Vector2 distanceBetweenOldAndNewCameraPositions = new Vector2(cameraTransform.position.x - oldPosition.x, cameraTransform.position.y - oldPosition.y);
        farBG.position = new Vector3(oldPosition.x + distanceBetweenOldAndNewCameraPositions.x * farBGSpeed,
                            oldPosition.y + distanceBetweenOldAndNewCameraPositions.y * farBGSpeed, farBG.position.z);
        closeBG.position = new Vector3(oldPosition.x + distanceBetweenOldAndNewCameraPositions.x * closeBGSpeed,
                                       oldPosition.y + distanceBetweenOldAndNewCameraPositions.y * closeBGSpeed, closeBG.position.z);

        CameraZoom();
    }

    private void CameraZoom()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        float bumpers = Input.GetAxis("Controller Bumpers");
        //Debug.Log(bumpers);
        cameraTargetSize = Mathf.Clamp(cameraTargetSize + scrollWheel + bumpers, cameraMinSize, cameraMaxSize);

        camera.orthographicSize = Mathf.MoveTowards(camera.orthographicSize, cameraTargetSize, Time.deltaTime * zoomSpeed);
    }

}
