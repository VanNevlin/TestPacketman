using System.Collections;
using UnityEngine;

public class MakeBoxChild : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            // Make the player a child of the platform                      
            transform.parent = collision.transform;
        }
    }


}
