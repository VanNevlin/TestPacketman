using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObjectSystem : MonoBehaviour
{
    [Header("Ray Positions :")]
    public Transform grabPoint;
    public Transform rayPoint;
    public Transform dropPoint;
    [Space(5)]

    public float rayDistance;

    [Space(10)]
    private GameObject grabbedObject;
    private int layerIndex;
    private GameManager gameManager;
    private bool isActivePlayer;
    private BuddySystem buddySystem;
    private PlayerMovement playerMovement;
    private LadderSystem ladderSystem;

    private Transform player;

    [Header("UI :")]
    public GameObject PickUpUI;
    public GameObject DropUI;
    private bool inRange;
    [Space(10)]

    [Header("Audio :")]
    public AudioSource PickUpAudio;
    public AudioSource DropAudio;

    private void Start()
    {
        layerIndex = LayerMask.NameToLayer("Grabbable");
        gameManager = FindObjectOfType<GameManager>();
        buddySystem = FindObjectOfType<BuddySystem>();


        playerMovement = GetComponent<PlayerMovement>();
        ladderSystem = GetComponent<LadderSystem>();

        player = transform;
    }

    private void Update()
    {

        float playerZRotation = transform.rotation.eulerAngles.z;

        // Set the text object's Z rotation to match the player's rotation
        PickUpUI.transform.rotation = Quaternion.Euler(0, 0, playerZRotation);
        DropUI.transform.rotation = Quaternion.Euler(0, 0, playerZRotation);

        isActive();
        StartCoroutine(PickUpCheck());
        StartCoroutine(DropCheck());

        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, transform.right, rayDistance);


        if (isActivePlayer)
        {

            if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == layerIndex)
            {
                Transform PickUpUIPos = hitInfo.collider.gameObject.transform.Find("PickUp");

                inRange = true;

                if (Input.GetKeyDown(KeyCode.F) && grabbedObject == null)
                {
                    grabbedObject = hitInfo.collider.gameObject;
                    grabbedObject.GetComponent<Rigidbody2D>().isKinematic = true;
                    grabbedObject.transform.position = grabPoint.position;
                    grabbedObject.transform.SetParent(transform);

                    playerMovement.YesPress = false;
                    playerMovement.canJump = false;
                    ladderSystem.canClimb = false;

                    PickUpAudio.Play();
                }

            }
            else if (Input.GetKeyDown(KeyCode.F) && grabbedObject != null)
            {
                grabbedObject.transform.SetParent(null);
                grabbedObject.transform.position = dropPoint.position;
                grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;

                grabbedObject = null;

                playerMovement.YesPress = true;
                playerMovement.canJump = true;
                ladderSystem.canClimb = true;

                DropAudio.Play();
            }

            else if (hitInfo.collider == null)
            {
                inRange = false;
            }
        }

        Debug.DrawRay(rayPoint.position, transform.right * rayDistance);

    }

    IEnumerator PickUpCheck()
    {

        while (isActivePlayer && !grabbedObject && inRange)
        {
            PickUpUI.SetActive(true);

            yield return null;
        }

        PickUpUI.SetActive(false);
    }

    IEnumerator DropCheck()
    {
        while (isActivePlayer && grabbedObject)
        {
            DropUI.SetActive(true);

            yield return null;
        }

        DropUI.SetActive(false);
    }


    private void isActive()
    {
        if (this.gameObject == buddySystem.players[buddySystem.activePlayerIndex])
        {
            isActivePlayer = true;
        }

        else
        {
            isActivePlayer = false;
        }
    }
}
