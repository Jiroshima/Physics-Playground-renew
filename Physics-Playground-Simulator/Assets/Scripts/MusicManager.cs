using UnityEngine;


/// <summary>
///  Script to manage music and let it play in every scene 
/// </summary>
public class MusicManager : MonoBehaviour
{
    public AudioSource backgroundMusic;
    // ensures there's only one musicmanager 
    // static to implement singleton pattern
    private static MusicManager instance;

    void Awake()
    {
        // implement singleton pattern to prevent multiple music managers
        // prevents duplicate music players from being created
        if (instance != null)
        {
            Destroy(gameObject); // if there is then detroy it 
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // set this as the single and dont destroy when loading a new scene. 
        }
    }

    void Start()
    {
        // check if background is assigned and not already playing 
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.Play(); // start playing the bg music (also prevents restarting music if its already playing )
        }
    }
}
