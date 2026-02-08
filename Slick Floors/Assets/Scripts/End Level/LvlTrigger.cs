using UnityEngine;
using UnityEngine.SceneManagement;

public class LvlTrigger : MonoBehaviour
{

    [SerializeField] private string sceneToLoad;

    public bool atDoor = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // SceneManager.LoadScene(sceneToLoad);
        atDoor = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        atDoor = false;
    }

    public void LoadNextScene()
    {
        if (atDoor) SceneManager.LoadScene(sceneToLoad);
    }
}
