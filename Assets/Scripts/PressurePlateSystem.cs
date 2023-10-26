using System.Collections;
using UnityEngine;

public class PressurePlateSystem : MonoBehaviour
{
    [Header("References :")]
    // References to other game objects and components
    public GameObject TargetBlock;
    [Space(5)]
    private Transform platform;
    private CameraMovement cameraMovement;
    private BuddySystem buddySystem;

    [Header("Properties :")]
    // Movement related variables
    public Vector2 offset = new Vector2(0f, 0f); // Offset from the initial position
    [Space(5)]
    private Vector3 targetPosition;
    private Vector3 initialPosition;
    private bool isActivePlayer = false;
    private bool inRange = false;
    public float moveSpeed = 5f; // Adjust this as needed
    public bool selfReference = false; //To create lift or collapsable platform
    [Space(5)]


    [Header("Development :")]
    // Player interaction variables
    public int minCount; // Minimum number of players needed
    private int playerCount = 0; // Current number of players on the pressure plate
    private bool countCheck = false;
    public bool Triggered = false;
    [Space(10)]

    private BoxCollider2D holder;
    private BoxCollider2D TriggerCollider;


    [Header("UI Stuff :")]
    public float BoxColliderScale;
    public float MovingScale;
    private Vector2 initialSize;
    [Space(10)]

    // Delays

    private float blockDelay = 2.5f;
    private float blockTravel;
    private float camDelay;

    public GameObject PressurePlateUI;

    [Header("Audio :")]
    public AudioSource platformSound;


    private void Start()
    {
        platform = TargetBlock.transform;

        initialPosition = platform.position;
        // Calculate the target position based on offsets
        targetPosition = initialPosition + new Vector3(offset.x, offset.y, 0);

        cameraMovement = FindObjectOfType<CameraMovement>();
        buddySystem = FindObjectOfType<BuddySystem>();

        blockTravel = Vector2.Distance(initialPosition, targetPosition);
        camDelay = (blockTravel / moveSpeed) + 2f;

        TriggerCollider = GetComponent<BoxCollider2D>();
        holder = GetComponent<BoxCollider2D>();


        if (selfReference)
        {
            camDelay = 0f;
            blockDelay = 0f;
        }

        initialSize = TriggerCollider.size;
    }

    private void Update()
    {
        if (selfReference)
        {
            ColliderScaler();
        }

        moveCheck();
        StartCoroutine(CheckUI());
        StartCoroutine(playMovingAudio());

        if (countCheck)
        {

            if (!Triggered)
            {
                cameraMovement.CameraTarget = platform.transform;
                StartCoroutine(BlockDelay(blockDelay));


            }
            else
            {
                MovePlatform(targetPosition);

            }

        }
        else
        {
            MovePlatform(initialPosition);


        }
    }

    IEnumerator CheckUI()
    {

        while (isActivePlayer && !countCheck && inRange)
        {
            PressurePlateUI.SetActive(true);


            yield return null;
        }

        PressurePlateUI.SetActive(false);
    }

    private void MovePlatform(Vector3 destination)
    {
        platform.position = Vector3.MoveTowards(platform.position, destination, Time.deltaTime * moveSpeed);
    }

    IEnumerator playMovingAudio()
    {
        while (platform.position != targetPosition && platform.position != initialPosition)
        {

            platformSound.mute = false;
            yield return null;
        }

        platformSound.mute = true;


    }

    // OnTriggerEnter2D is called when a player enters the trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            playerCount++;
            inRange = true;

        }

        else if (other.CompareTag("Grabbable"))
        {
            playerCount++;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == buddySystem.players[buddySystem.activePlayerIndex])
        {
            isActivePlayer = true;


        }
        else
        {
            isActivePlayer = false;


        }
    }
    // OnTriggerExit2D is called when a player exits the trigger zone
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            playerCount--;
            inRange = false;

        }

        else if (other.CompareTag("Grabbable"))
        {
            playerCount--;
        }
    }

    // Coroutine to trigger the platform with a delay
    IEnumerator BlockDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        MovePlatform(targetPosition);
        StartCoroutine(CamDelay(camDelay));

        Triggered = true;

    }

    IEnumerator CamDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        cameraMovement.CameraTarget = buddySystem.players[buddySystem.activePlayerIndex].transform;
    }

    private void moveCheck()
    {
        if (playerCount >= minCount)
        {
            countCheck = true;
        }
        else if (playerCount < minCount)
        {
            countCheck = false;
        }
    }

    void ScaleTriggerCollider(float scale)
    {
        // Scale the trigger BoxCollider2D
        TriggerCollider.size = new Vector2(initialSize.x * scale, initialSize.y);
    }

    void RevertToInitialSize()
    {
        // Revert to the initial size
        TriggerCollider.size = initialSize;
    }

    void ColliderScaler()
    {
        if (transform.position == targetPosition)
        {
            ScaleTriggerCollider(BoxColliderScale);
        }

        else if (transform.position == initialPosition)
        {
            RevertToInitialSize();
        }

        else if (transform.position != initialPosition && transform.position != targetPosition)
        {
            ScaleTriggerCollider(MovingScale);
        }
    }
}
