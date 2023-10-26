using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneManager : MonoBehaviour
{
    public int nextSceneIndex; // Define the next scene in the build index through the Unity Inspector
    public EndSceneTrigger[] endSceneTriggers; // An array of EndSceneTrigger objects

    public Animator transition;
    public float transitionTime = 1f;

    private void Start()
    {
        // Initialize the array of triggers through FindObjectsOfType to find all active instances
        endSceneTriggers = FindObjectsOfType<EndSceneTrigger>();
    }

    private void Update()
    {
        if (AllTriggersDetected())
        {
            LoadNextScene();
        }

        if (nextSceneIndex % 2 == 0) // With the assumption that loading screens are on odd build indexes
        {
            StartCoroutine(EndLoadingScreen(nextSceneIndex));
        }
    }

    // Check if all triggers have detected set to true
    private bool AllTriggersDetected()
    {
        foreach (EndSceneTrigger trigger in endSceneTriggers)
        {
            if (!trigger.detected)
            {
                return false; // If any trigger is not detected, return false
            }
        }
        return true; // All triggers have detected set to true
    }

    // Load the next scene
    private void LoadNextScene()
    {
        StartCoroutine(LoadLevel(nextSceneIndex));
        //Debug.Log("Loading Scene with Index :" + nextSceneIndex);
    }

    IEnumerator LoadLevel(int nextSceneIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(nextSceneIndex);
    }

    IEnumerator EndLoadingScreen(int nextSceneIndex)
    {
        yield return new WaitForSeconds(4);
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(nextSceneIndex);
    }
}
