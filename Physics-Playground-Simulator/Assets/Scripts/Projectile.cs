using System;
using UnityEngine;


/// <summary>
/// script for projectiles, initially i was going to have a system where when targets were hit you would get a pop up message but i realised it can be very overwhelming because of how many 
/// projectiles u can launch at a time 
/// so this isn't really used 
/// </summary>
public class Projectile : MonoBehaviour
{
    private void OnCollissionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            print("hit " + collision.gameObject.name + " !");
        }
    }
}
