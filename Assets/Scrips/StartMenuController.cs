using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{

    public GameObject Panel;

    public void OnStartClick()
    {
        SceneManager.LoadScene("CharacterSelection");
    }

    public void OnNewGameClick()
    {
        Panel.SetActive(false);
    }

    public void OnExitClick()
    {
        // UnityEditor.EditorApplication.isPlaying = false; //for testing in unity
        Application.Quit();
    }

    public void OnPlayClick()
    {
        SceneManager.LoadScene("PondOne");
    }


    public void BackButton()
    {
        // Panel.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }

    public void OnClickYes()
    {
        Panel.SetActive(false);
    }

    public void OnClickNo()
    {
        Panel.SetActive(false);
    }

}
