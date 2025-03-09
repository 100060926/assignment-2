using UnityEngine;
using System.Collections;

public class CameraZoomEffect : MonoBehaviour
{
    public Transform targetPosition; // Where the camera should move toward
    public float zoomSpeed = 2f;     // How fast the zoom occurs
    public float moveSpeed = 2f;     // How fast the camera moves

    void Start()
    {
        StartCoroutine(MoveAndZoomCamera());
    }

    IEnumerator MoveAndZoomCamera()
    {
        float elapsedTime = 0f;
        float duration = 3f; // Camera zoom duration

        Camera camera = GetComponent<Camera>();
        float startFOV = camera.fieldOfView;
        float targetFOV = startFOV * 0.7f; // Zoom in effect (adjust as needed)

        Vector3 startPosition = transform.position;
        Vector3 targetPos = targetPosition.position; // Move toward this position

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Smoothly move the camera toward the target position
            transform.position = Vector3.Lerp(startPosition, targetPos, elapsedTime / duration);

            // Smoothly zoom the camera
            camera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsedTime / duration);

            yield return null;
        }

        Debug.Log("Camera zoom-in complete.");
    }
}
