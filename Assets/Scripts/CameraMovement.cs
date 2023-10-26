using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform CameraTarget;
    public float followSpeed = 5.0f;
    public float yOffset = 1.0f;
    public Vector2 deadzoneSize = new Vector2(2.0f, 2.0f);

    void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(CameraTarget.position.x, CameraTarget.position.y + yOffset, transform.position.z);

        // Calculate the boundaries of the deadzone.
        float minX = transform.position.x - deadzoneSize.x / 2;
        float minY = transform.position.y - deadzoneSize.y / 2;
        float maxX = transform.position.x + deadzoneSize.x / 2;
        float maxY = transform.position.y + deadzoneSize.y / 2;

        // Check if the target is outside the deadzone.
        if (targetPosition.x < minX || targetPosition.x > maxX || targetPosition.y < minY || targetPosition.y > maxY)
        {
            // Smoothly move the camera towards the target position.
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
        }
    }
}
