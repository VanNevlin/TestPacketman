using UnityEngine;

public class EndSceneTrigger : MonoBehaviour
{
    public GameObject targetObject; // Set this through the Unity Inspector
    public bool detected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision's GameObject matches the targetObject
        if (collision.gameObject == targetObject)
        {
            Debug.Log("Detected");
            detected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Reset the detected variable if the targetObject exits the trigger
        if (collision.gameObject == targetObject)
        {
            detected = false;
        }
    }
}

