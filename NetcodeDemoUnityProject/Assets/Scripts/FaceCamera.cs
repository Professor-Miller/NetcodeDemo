using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera mainCamera;

    // Find the main camera on start
    private void Start()
    {
        mainCamera = Camera.main;
    }

    // Look at the main camera in LateUpdate to ensure it's found before use
    private void LateUpdate()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            return;
        }

        // Look at the main camera
        transform.LookAt(transform.position + mainCamera.transform.forward, mainCamera.transform.up);
    }
}
