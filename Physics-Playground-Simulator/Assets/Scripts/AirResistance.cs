using UnityEngine;
using UnityEngine.Rendering;


// Apply air resistance to a game object when it enters a collision box called windArea
public class AirResistanceTest : MonoBehaviour
{
    // check if the object is in the zone 
    public bool inWindZone = false;
    public GameObject windZone;
    
    // rigid body component to reference
    Rigidbody rb; 

    private void Start()
    {
        // get and store the rigid body component attached to the game object 
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        // If  the object is in the wind zone, apply force based on the wind direction and the strength
        if(inWindZone){
            rb.AddForce(windZone.GetComponent<FanAirFlow>().direction * windZone.GetComponent<FanAirFlow>().strength);
        }
    }

    // call when the collider enters triggerr collider
    void OnTriggerEnter(Collider coll){
        // if collided object has the tag wind area 
        if(coll.gameObject.tag =="windArea"){

            // store the wind zone reference and set inWindZone to true 
            windZone = coll.gameObject;
            inWindZone = true;
        }
    }
    
    // when it exits the trigger colider 
    void OnTriggerExit(Collider coll) {
        // check if the object has tag windArea
        if(coll.gameObject.tag =="windArea") {
            // set inWindZone to false when leaving 
            inWindZone = false;
        }
    }

}