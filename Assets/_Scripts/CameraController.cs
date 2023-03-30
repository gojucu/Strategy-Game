using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed = 5f; // Camera movement speed

    [SerializeField]
    private float cameraZoomSpeed = 2f; // Camera zoom speed

    [SerializeField]
    private float minCameraSize = 3f; // Minimum camera size

    [SerializeField]
    private float maxCameraSize = 10f; // Maximum camera size

    [SerializeField]
    private Vector2 boundary = new Vector2(10f, 10f); // Boundary for camera movement

    [SerializeField]
    private Color gizmoColor = Color.green;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        // Check if mouse is within screen boundaries
        if (Input.mousePosition.x < -Screen.width * .1f || Input.mousePosition.x > Screen.width*1.1f ||
            Input.mousePosition.y < -Screen.height * .1f || Input.mousePosition.y > Screen.height*1.1f)
        {
            return; // If mouse is outside screen, don't move camera
        }

        if (true/*!EventSystem.current.IsPointerOverGameObject()*/)
        {
            // Move the camera when cursor is near the screen edge
            Vector3 move = Vector3.zero;
            if (Input.mousePosition.x < Screen.width * .05f && transform.position.x > -boundary.x)
            {
                move.x -= cameraSpeed * Time.deltaTime;
            }
            else if (Input.mousePosition.x > Screen.width*.95f && transform.position.x < boundary.x)
            {
                move.x += cameraSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.y < Screen.height * .1f && transform.position.y > -boundary.y)
            {
                move.y -= cameraSpeed * Time.deltaTime;
            }
            else if (Input.mousePosition.y > Screen.height* .9f && transform.position.y < boundary.y)
            {
                move.y += cameraSpeed * Time.deltaTime;
            }

            transform.position += move;

            // Zoom the camera in/out with the mouse wheel
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            float newSize = mainCamera.orthographicSize - scroll * cameraZoomSpeed;
            mainCamera.orthographicSize = Mathf.Clamp(newSize, minCameraSize, maxCameraSize);
           }
    }

    private void OnDrawGizmos()
    {
        // Draw a wireframe rectangle around the screen edge area
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position, new Vector3(Screen.width, Screen.height, 0f));
        Gizmos.DrawWireCube(transform.position, new Vector3(Screen.width - Screen.width * .05f, Screen.height - Screen.height * .1f, 0f));
    }
}


