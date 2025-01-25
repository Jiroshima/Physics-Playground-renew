using UnityEngine;


/// <summary>
/// Script to allow playermovement 
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    // reference to charactercontroller component for movement 
    private CharacterController controller;
    
    // parameteres for movement 
    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    // for checking if player isGrounded
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    // velocity to track for gravity and if player is grounded (for jumping )
    Vector3 velocity; 
    bool isGrounded;

    // audio for walking + jumping 
    private AudioSource walkingAudioSource;
    private AudioSource jumpAudioSource;

    public AudioClip walkingSound; // foot step sounds 
    public AudioClip jumpSound;    // jumpsound 
    private bool isWalking; // track walking state 

    void Start()
    {   
        // get charactercontroller component attached to the gameobject 
        controller = GetComponent<CharacterController>();
        
        // create a seperate audio source for walking + jumping 
        walkingAudioSource = gameObject.AddComponent<AudioSource>();
        jumpAudioSource = gameObject.AddComponent<AudioSource>();

        // loop audio source for continuous sound 
        walkingAudioSource.loop = true;
        walkingAudioSource.spatialBlend = 1f; // set spatial blend to 1 for 3D (not really necessary but good practice for multiplayer games)
    }

    void Update()
    {
        // check if player is touching the ground using a sphere game object 
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // reset the vertical velocity when grounded to prevent the continuous downward acceleration 
        if (isGrounded && velocity.y < 0)
        { 
            velocity.y = -2f;
        }

        // get input for horizontal movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // calculate movement direction relative to player orientation
        Vector3 move = transform.right * x + transform.forward * z;

        // check if player is moving 
        if (move.magnitude > 0f && isGrounded)
        {
            if (!isWalking) // olay walking sound if the player starts walking
            {
                isWalking = true;
                walkingAudioSource.clip = walkingSound;
                walkingAudioSource.Play();
            }
        }
        else
        {
            if (isWalking) // stop walking sound if the player stops
            {
                isWalking = false;
                walkingAudioSource.Stop();
            }
        }

        // move the player horizontally
        controller.Move(move * speed * Time.deltaTime);

        // handle jumping mechanics
        if (Input.GetButtonDown("Jump") && isGrounded)
        {   
            // calculate the jump velocity 
            // v = sqrt(2gh)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // play jump sound when jumping
            jumpAudioSource.PlayOneShot(jumpSound);
        }

        // apply gravity 
        velocity.y += gravity * Time.deltaTime;

        // move player vertically (apply gravity) 
        // time.deltatime so frame-rate independent 
    
        controller.Move(new Vector3(0, velocity.y, 0) * Time.deltaTime);
    }
}
