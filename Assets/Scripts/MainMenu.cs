using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Load Scene Game
    public void StartGame()
    {
        SceneManager.LoadScene("Scene Utama");
    }

    // Keluar dari game
    public void ExitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}
