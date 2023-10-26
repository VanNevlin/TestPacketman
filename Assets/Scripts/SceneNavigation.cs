using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour
{
    public int sceneToLoad = 0;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
