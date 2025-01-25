using UnityEngine;

// this script is to manage the collision sounds 
// collision sounds were too loud and too many were playing at once as there were too many objects. 
// fixed this by adding a cooldown period to reduce the amount of sounds playing at the same time.

public class CollisionSound : MonoBehaviour
{
    private AudioSource audioSource;  // audiosource component to play sound
    private AudioClip collisionSound;  // the audio clip to play on collision 

    [SerializeField] private float dopplerLevel = 1f;  
    [SerializeField] private float cooldownTime = 1f;  // set cooldown time to 1s 

    private float lastSoundTime = 0f;  // the time of the last sound played 

    private void Start()
    {
        // ensure the gameobject has an audio source omponent 
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // add one if it doesnt 
            audioSource = gameObject.AddComponent<AudioSource>();  // Add one if not present
        }

        // configure audio settings for 3d sound 
        audioSource.spatialBlend = 1f;  // set spatial blend to 1 
        audioSource.loop = false;  // disable looping 
        audioSource.dopplerLevel = dopplerLevel;  // apply doppler effect 
        // this could've been done in the built in settings too 
    }


    // assign the collision sound clip to the object 
    public void SetCollisionSound(AudioClip sound)
    {
        collisionSound = sound;
    }

    // play the sound with a volume and a 3d effect 
    public void PlaySound(float volume, bool is3D)
    {
        // check if enough time has passed (1s) since the last sound was played 
        if (collisionSound != null && audioSource != null && Time.time > lastSoundTime + cooldownTime)
        {
            audioSource.PlayOneShot(collisionSound, volume);  
            lastSoundTime = Time.time;  // Update  last played sound time 
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // play collision sound only if cooldown period has passed
        if (collisionSound != null && audioSource != null && Time.time > lastSoundTime + cooldownTime)
        {
            // play collision sound. (reduced the volume because it is too loud)
            audioSource.PlayOneShot(collisionSound, 0.5f);  
            lastSoundTime = Time.time;  // update the last played sound time 
        }
    }
}
