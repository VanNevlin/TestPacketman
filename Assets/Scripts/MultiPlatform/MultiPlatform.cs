using System.Collections;
using UnityEngine;

public class MultiPlatform : MonoBehaviour
{
    private Transform doorBlock;
    [Header("Platform Properties :")]
    public Vector2 offset = new Vector2(0f, 0f);
    [Space(5)]
    public float moveSpeed = 5f;
    [Space(5)]
    public MultiPlatformButton ButtonReference;
    [Space(10)]

    private Vector3 currentPosition;
    private Vector3 targetPosition;
    private bool isForward = true;

    [Header("Audio :")]
    public AudioSource audioSource;
    public AudioSource EndSound;



    public void Start()
    {
        doorBlock = GetComponent<Transform>();
        //  audioSource = GetComponent<AudioSource>();
    }


    public void MoveDoor()
    {
        currentPosition = doorBlock.position;

        if (isForward)
        {
            targetPosition = currentPosition + new Vector3(offset.x, offset.y, 0);
        }
        else if (!isForward)
        {
            targetPosition = currentPosition - new Vector3(offset.x, offset.y, 0);
        }

        StartCoroutine(MoveFunctionality(targetPosition));



    }

    IEnumerator MoveFunctionality(Vector3 target)
    {
        Vector3 currentPos = doorBlock.position;
        float startTime = Time.time;
        float journeyLength = Vector3.Distance(currentPos, target);

        while (doorBlock.position != target)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            doorBlock.position = Vector3.Lerp(currentPos, target, fractionOfJourney);

            audioSource.mute = false;

            yield return null;
        }

        EndSound.Play();
        audioSource.mute = true;

        if (isForward)
        {
            isForward = false;
        }

        else if (!isForward)
        {
            isForward = true;
        }
    }


}
