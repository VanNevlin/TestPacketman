using UnityEngine;
using System.Collections;

public class MultiPlatformButton : MonoBehaviour
{
    [Header("Button Properties :")]
    public GameObject[] objectsToMove;

    [Space(10)]
    [Header("Delays :")]
    public float moveDelay = 2f;
    public float camDelay = 2f;
    public float pressDelay = 2f;

    private BuddySystem buddySystem;
    private CameraMovement cameraMovement;

    private bool canPress = true;
    private bool isActivePlayer;
    private bool playerinrange;
    private bool firstTime = true;
    private GameManager gameManager;

    [Space(10)]
    [Header("UI :")]
    public bool UICheck = true;
    public GameObject PressUI;
    private GameObject playerObject;
    public bool playerYesPress;

    [Space(10)]
    [Header("Audio :")]

    public AudioSource buttonSound;



    private void Start()
    {
        buddySystem = FindObjectOfType<BuddySystem>();
        cameraMovement = FindObjectOfType<CameraMovement>();
        gameManager = FindObjectOfType<GameManager>();


    }

    private void Update()
    {
        StartCoroutine(CheckUI());
        PressCheck();

        if (playerYesPress)
        {
            if (Input.GetKeyDown(KeyCode.E) && playerinrange && isActivePlayer && canPress)
            {
                buttonSound.Play();
                if (firstTime)
                {

                    StartCoroutine(ExecuteMoveDoorOneByOne());
                }
                else
                {

                    StartCoroutine(ExecuteMoveDoorAtOnce());
                }
            }
        }
    }

    private void PressCheck()
    {
        if (playerObject != null)
        {
            PlayerMovement playerMovement = playerObject.GetComponent<PlayerMovement>();

            if (playerMovement.YesPress == true)
            {
                playerYesPress = true;
            }

            else if (playerMovement.YesPress == false)
            {
                playerYesPress = false;
            }
        }
    }
    IEnumerator CheckUI()
    {

        while (isActivePlayer && playerinrange && UICheck && playerYesPress)
        {
            PressUI.SetActive(true);

            yield return null;
        }

        PressUI.SetActive(false);
    }



    private IEnumerator ExecuteMoveDoorAtOnce()
    {

        foreach (var obj in objectsToMove)
        {
            canPress = false;
            MultiPlatform multiPlatform = obj.GetComponent<MultiPlatform>();
            if (multiPlatform != null)
            {
                UICheck = false;
                multiPlatform.MoveDoor();

            }

        }

        yield return new WaitForSeconds(pressDelay);
        canPress = true;
        UICheck = true;
    }

    private IEnumerator ExecuteMoveDoorOneByOne()
    {
        int index = 0;

        foreach (var obj in objectsToMove)
        {
            canPress = false;
            firstTime = false;
            UICheck = false;

            MultiPlatform multiPlatform = obj.GetComponent<MultiPlatform>();
            if (multiPlatform != null)
            {
                cameraMovement.CameraTarget = obj.transform;


                yield return new WaitForSeconds(camDelay);

                multiPlatform.MoveDoor();

                if (index == objectsToMove.Length - 1)
                {

                    yield return new WaitForSeconds(camDelay);
                    cameraMovement.CameraTarget = buddySystem.players[buddySystem.activePlayerIndex].transform;

                }
                else
                {

                    yield return new WaitForSeconds(moveDelay);
                }

                index++;
            }
        }
        UICheck = true;
        canPress = true;



    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerinrange = true;
            playerObject = collision.gameObject;

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
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerinrange = false;
            playerObject = null;
        }
    }
}
