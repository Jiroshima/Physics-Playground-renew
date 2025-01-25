using UnityEngine;

/// <summary>
/// Script which allows player to switch between the different weapons 
/// </summary>
public class WeaponSwitching : MonoBehaviour
{
    // array of weapon transforms to manage multiple weapons in the player's inventory
    // SerializeField allows private variables to be set in the Unity Inspector
    [SerializeField] private Transform[] weapons;

    // array of HUD GameObjects corresponding to each weapon
    [SerializeField] private GameObject[] weaponHUDs;

    // array of KeyCodes to define which keys trigger weapon switching
    [SerializeField] private KeyCode[] keys;

    // delay to prevent rapid-weapon switching
    [SerializeField] private float switchTime = 0.5f;

    // tracks the currently selected weapon index
    // and initializes to 0 (first weapon in the array)
    private int selectedWeapon = 0;

    // tracks time elapsed since the last weapon switch
    private float timeSinceLastSwitch = 0f;

    private void Start()
    {
        //check to prevent errors if no weapons are assigned
        if (weapons.Length == 0 || weaponHUDs.Length == 0) return;

        // initial setup of weapons and their visibility
        SetWeapons();

        // select the first weapon by default
        Select(selectedWeapon);
    }

    private void SetWeapons()
    {
        // check if the keys array matches the number of weapons
        if (keys.Length != weapons.Length)
        {
            keys = new KeyCode[weapons.Length];
        }

        // initialize all weapons and their HUDs to be invisible
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(false);

            // only deactivate the HUDs if there are enough defined
            if (i < weaponHUDs.Length)
            {
                weaponHUDs[i].SetActive(false);
            }
        }
    }

    private void Update()
    {
        // increment the time since last weapon switch
        timeSinceLastSwitch += Time.deltaTime;

        // check for weapon switch input
        for (int i = 0; i < keys.Length; i++)
        {
            // only switch if the corresponding key is pressed 
            // and if enough time has passed since the last switch
            if (Input.GetKeyDown(keys[i]) && timeSinceLastSwitch >= switchTime)
            {
                // update the selected weapon to the index of the pressed key
                selectedWeapon = i;

                // reset the timer for cooldown
                timeSinceLastSwitch = 0f;
            }
        }
        // ensure the correct weapon is selected and displayed
        Select(selectedWeapon);
    }

    private void Select(int weaponIndex)
    {
        // only change if the weapon is not already active
        if (weapons[weaponIndex].gameObject.activeSelf) return;

        // deactivate all weapons and their corresponding HUDs
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(false);

            // only deactivate HUDs if there are enough defined
            if (i < weaponHUDs.Length)
            {
                weaponHUDs[i].SetActive(false);
            }
        }

        // activate the selected weapon
        weapons[weaponIndex].gameObject.SetActive(true);

        // activate the corresponding weapon HUD if available
        if (weaponIndex < weaponHUDs.Length)
        {
            weaponHUDs[weaponIndex].SetActive(true);
        }
    }
}