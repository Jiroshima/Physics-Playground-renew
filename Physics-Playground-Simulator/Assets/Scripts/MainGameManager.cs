using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// The script is to manage the different spawn points for 'Play' and 'Start Tutorial' 
/// </summary>
/// 
public class MainGameManager : MonoBehaviour
{
    public Transform mainAreaSpawn;     // main area spawn point 
    public Transform tutorialAreaSpawn; //  tutorial area spawn point 
    public GameObject player;           // Player gameobject
    public PlayerMovement playerMovementScript; // PlayerMovementScript (needed for resetting playermovement as it glitches out when going in an out of the main menu )
    private bool isGamePaused = false;  // check if game is paused 

    void Start()
    {
        // check that player movement is enabled when the game begins 
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;  // enable if not 
        }

        // retrieve the spawn area preference from Playerprefs - defaults to mainArea if not set 
        string spawnArea = PlayerPrefs.GetString("SpawnArea", "MainArea");

        // set player position and rotation based on the selected spawn area 
        if (spawnArea == "MainArea" && mainAreaSpawn != null)
        {
            player.transform.position = mainAreaSpawn.position;
            player.transform.rotation = mainAreaSpawn.rotation;
        }
        else if (spawnArea == "TutorialArea" && tutorialAreaSpawn != null)
        {
            player.transform.position = tutorialAreaSpawn.position;
            player.transform.rotation = tutorialAreaSpawn.rotation;
        }
    }

    void Update()
    {
        // checks if escape key is press to go back to the main menu 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                GoToMainMenu();
            }
        }
    }

    // Function to resume the game 
    private void ResumeGame()
    {
        Time.timeScale = 1f; // resume game time 
        isGamePaused = false; // unpause game 

        // enable player movement if it is disabled
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }
    }

    // pauses the game and goes back to the main menu 
    private void GoToMainMenu()
    {
        // pause the game by stopping player movement and freezing time 
        Time.timeScale = 0f;
        isGamePaused = true; // set paused state to true 

        // disable the player movement 
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }

        // set the default spawn area to MainArea
        PlayerPrefs.SetString("SpawnArea", "MainArea");

        // load the MainMenu Scene 
        SceneManager.LoadScene("MainMenu");
    }

    // call this method when the game is resumed from the main menu 
    public void StartNewGame(string spawnArea)
    {
        // save the chosen spawn area
        PlayerPrefs.SetString("SpawnArea", spawnArea);

        // load the Main Game scene
        SceneManager.LoadScene("MainGame");
    }
}
