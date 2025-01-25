using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    public Camera playerCamera; // reference to player's camera for aiming

    // shooting mechanics control variables
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f; // time between shots

    // bullet instantiation properties
    public GameObject bulletPrefab;     // the prefab we will be spawning
    public Transform bulletSpawn;       // the spawn point where projectiless will be created 
    public float bulletVelocity = 30;   
    public float bulletPrefabLifeTime = 3f; 

    // the max and minimum projectile velocities
    public float minVelocity = 10f;    
    public float maxVelocity = 100f;   
    public float currentVelocity = 30f; 

    // min and max values for the angles of projectile trajectory 
    public float minAngle = -45f;       
    public float maxAngle = 45f;        
    public float currentAngle = 0f;     

    // projectile sizes
    public float minSize = 0.1f;       
    public float maxSize = 4f;          
    public float currentSize = 0.3f;   

    // shooting delay min and max (for firerate)
    public float minShootingDelay = 0.05f; 
    public float maxShootingDelay = 3f;    

    // ui text to display the different properties
    public TextMeshProUGUI shootingDelayText;
    public TextMeshProUGUI velocityText;
    public TextMeshProUGUI angleText;
    public TextMeshProUGUI sizeText;

    // audio for shooting sound 
    private AudioSource audioSource;
    public AudioClip shootingSound; // sound played when firing

    private void Awake()
    {
        // initialize the weapon state 
        readyToShoot = true;

        // setup audio component 
        audioSource = GetComponent<AudioSource>();

        // set the initial ui displays 
        UpdateUI();
    }

    void Update()
    {
        // auto-shoot when mouse button is held down
        if (Input.GetKey(KeyCode.Mouse0) && readyToShoot)
        {   
            // fire weapon continuously while mouse button is held down
            FireWeapon();
        }

        // handle projectile adjustments
        HandleProjectileAdjustments();
        UpdateWeaponAngle();

        // if z is pressed then 
        if (Input.GetKeyDown(KeyCode.Z))
        {   
            // decrease shooting delay 
            shootingDelay = Mathf.Clamp(shootingDelay - 0.1f, minShootingDelay, maxShootingDelay);
            UpdateUI();
        }
        // if c i pressed then 
        else if (Input.GetKeyDown(KeyCode.C))
        {
            // increase shooting delay 
            shootingDelay = Mathf.Clamp(shootingDelay + 0.1f, minShootingDelay, maxShootingDelay);
            UpdateUI();
        }
    }

    private void HandleProjectileAdjustments()
    {
        // for changing projectile velocity via scroll wheel 
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (scrollDelta != 0 && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
        {
            currentVelocity = Mathf.Clamp(currentVelocity + scrollDelta * 10f, minVelocity, maxVelocity);
        }

        // for changing projectile size via scroll wheel + shift 
        if (scrollDelta != 0 && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            currentSize = Mathf.Clamp(currentSize + scrollDelta * 0.1f, minSize, maxSize);
        }

        // adjusting angles by pressing Q or E
        if (Input.GetKey(KeyCode.Q))
        {
            currentAngle = Mathf.Clamp(currentAngle + 30f * Time.deltaTime, minAngle, maxAngle);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            currentAngle = Mathf.Clamp(currentAngle - 30f * Time.deltaTime, minAngle, maxAngle);
        }

        // this updates the UI according to the changes 
        UpdateUI();
    }

    private void FireWeapon()
    {
        // prevents player from spam clicking or holding down the shoot button by disabling weapon firing temporarily 
        readyToShoot = false;
 
        // calculate the shooting direction
        // 1. get the initial direction based on camera's center point 
        // 2. normalise to ensure consistent projectile speed
        // 3. apply vertical angle rotation to modify the trajectory 
        Vector3 shootingDirection = CalculateDirection().normalized;
        // rotate shooting direction around x-axis using the current vertical angle
        // so you can aim slightly up or down without changing the camera position 
        shootingDirection = Quaternion.Euler(currentAngle, 0, 0) * shootingDirection;

        // projectile instantiation process
        // 1. create projectile at a specified spawn point 
        // 2. set the projectile size based on current size setting
        // 3. align the projectile's forward direction with the shooting trajectory 
        // 4. apply the initial velocity 
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        bullet.transform.localScale = Vector3.one * currentSize; 
        bullet.transform.forward = shootingDirection; 
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * currentVelocity, ForceMode.Impulse); 

        // play shooting sound 
        if (audioSource != null && shootingSound != null)
        {
            audioSource.PlayOneShot(shootingSound);
        }

        // prevent immediate refiring
        Invoke("ResetShot", shootingDelay);
    }

    private void ResetShot()
    {
        //re-enable weapon firing
        readyToShoot = true;
    }

    public Vector3 CalculateDirection()
    {
       // generate a ray from the center of the camera's viewport
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        // check if ray intersects with any object in the scene 
        if (Physics.Raycast(ray, out hit))
        {
            // if it does then use the exact point of intersection as target 
            targetPoint = hit.point;
        }
        else
        {
            // if it doesnt then project a point 100 units away 
            // this provides a fall back for shooting in open space 
            targetPoint = ray.GetPoint(100);
        }
        

        // calculate direction vector from bullet spawn point to the target point 
        Vector3 direction = targetPoint - bulletSpawn.position;
        return direction;
    }

    private void UpdateWeaponAngle()
    {
        // rotate weapon based on current vertical angle
        transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
    }

    private void UpdateUI()
    {
        // update UI text elements with current launcher properties 
        if (shootingDelayText != null)
            shootingDelayText.text = $"Delay: {shootingDelay:F1}s";
        
        if (velocityText != null)
            velocityText.text = $"Velocity: {currentVelocity:F1}";
        
        if (angleText != null)
            angleText.text = $"Angle: {currentAngle:F1}°";
        
        if (sizeText != null)
            sizeText.text = $"Size: {currentSize:F1}";
    }
}
