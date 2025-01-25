using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// for handling the main menu functions 
/// </summary>
/// 
public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        // this ensures that the game time is running normally when the main menu scene loads
        Time.timeScale = 1f;

        // shows cursor and unlocks it 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Start the game in the main area by setting the correct spawn point 
    public void PlayMainArea()
    {
        PlayerPrefs.SetString("SpawnArea", "MainArea"); // store main spawn area 
        SceneManager.LoadScene("MainGame"); // load the maingame scene
    }

    public void PlayTutorialArea()
    {
        PlayerPrefs.SetString("SpawnArea", "TutorialArea"); // store the tutorial spawn area
        SceneManager.LoadScene("MainGame"); // load the maingame scene
    }

    public void QuitGame()
    {
        Application.Quit(); // quits the game 
    }
}
