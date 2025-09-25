using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverMenu : MonoBehaviour
{
    public void loadMainGame()
    {
        SceneManager.LoadScene("Main game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
