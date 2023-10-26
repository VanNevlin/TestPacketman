using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    [Header("UI Elements :")]
    public Dialogue dialogue;
    [Space(8)]

    [Header("Development :")]
    public bool invisible;
    [Space(5)]
    public bool repeating;

    private bool inRange;

    public bool ActiveTrigger = false;
    private BuddySystem buddySystem;
    private DialogueManager dialogueManager;
    private bool invisibleBool; // Safety //

    public Image[] characterImages; // Add this variable for character images

    private int currentImageIndex = 0; // Add this variable to track the current image index
    public GameObject DialogueTriggerUI;
    private bool UICheck = true;
    private GameObject playerObject;
    public bool playerYesPress;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        buddySystem = FindObjectOfType<BuddySystem>();

        // Disable all character images at the beginning
        foreach (var characterImage in characterImages)
        {
            characterImage.enabled = false;
        }

    }

    public void Update()
    {
        StartCoroutine(CheckUI());
        PressCheck();


        if (inRange && ActiveTrigger)
        {
            //Debug.Log("Conditions Met");
            if (!repeating && invisible && !invisibleBool)
            {
                TriggerDialogue();
                invisibleBool = true;

                // Enable the first character image
                currentImageIndex = 0; // Reset the current index
                if (characterImages.Length > 0)
                {
                    characterImages[currentImageIndex].enabled = true;

                }
            }

            if (repeating && Input.GetKeyDown(KeyCode.E) && !invisible && playerYesPress)
            {
                TriggerDialogue();

                // Enable the first character image
                currentImageIndex = 0; // Reset the current index
                if (characterImages.Length > 0)
                {
                    characterImages[currentImageIndex].enabled = true;
                    UICheck = false;
                }
            }

            if (!repeating && Input.GetKeyDown(KeyCode.E) && !invisible && playerYesPress)
            {
                TriggerDialogue();

                // Enable the first character image
                currentImageIndex = 0; // Reset the current index
                if (characterImages.Length > 0)
                {
                    characterImages[currentImageIndex].enabled = true;
                    UICheck = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {

                NextSentence();
                Debug.Log("NextSentence");

                // Disable the current character image
                if (currentImageIndex < characterImages.Length)
                {
                    characterImages[currentImageIndex].enabled = false;
                }


                // Increment the current index
                currentImageIndex++;

                // Check if we reached the end of the array
                if (currentImageIndex >= characterImages.Length)
                {
                    currentImageIndex = 0; // Reset to the beginning

                }

                // Enable the next character image if it exists
                if (currentImageIndex < characterImages.Length)
                {
                    characterImages[currentImageIndex].enabled = true;
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

        while (ActiveTrigger && inRange && UICheck && !invisible && playerYesPress)
        {
            DialogueTriggerUI.SetActive(true);

            yield return null;
        }

        DialogueTriggerUI.SetActive(false);
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    public void NextSentence()
    {
        FindObjectOfType<DialogueManager>().DisplayNextSentence();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = true;
            playerObject = collision.gameObject;
            UICheck = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == buddySystem.players[buddySystem.activePlayerIndex] && !collision.CompareTag("Grabbable"))
        {

            //Debug.Log("ActiveTrigger");
            ActiveTrigger = true;
        }
        else if (collision.gameObject != buddySystem.players[buddySystem.activePlayerIndex] && !collision.CompareTag("Grabbable"))
        {
            Debug.Log("Not ActiveTrigger" + gameObject.name);
            ActiveTrigger = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = false;
            playerObject = collision.gameObject;
        }
    }
}
