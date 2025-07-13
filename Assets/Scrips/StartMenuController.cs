using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void OnStartClick()
    {
        SceneManager.LoadScene("CharacterSelection");
    }

    public void OnExitClick()
    {
        Application.Quit();
    }

    public void OnPlayClick()
    {
        SceneManager.LoadScene("PondOne");
    }
}
