using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// script for a platform that returns player to main menu after finishing the tutorial 
/// </summary>
public class ReturnToMenu : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu"; 
    private void OnTriggerEnter(Collider other)
    {
        // check if the object that touches the object has a player tag 
        if (other.CompareTag("Player"))
        {
            // call loadmainmenu method if yes 
            LoadMainMenu();
        }
    }

    private void LoadMainMenu()
    {
        // load the main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
