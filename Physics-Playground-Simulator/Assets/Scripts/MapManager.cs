using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{     

    // a struct to store the initial state of a game object 
    private struct ObjectState
    {
        public Vector3 Position; // original position of the object
        public Quaternion Rotation; // original rotation of the object
        public Vector3 Scale; // original scale of the object 
    }


    // create a dictionary to store the initial states of all the gameobjects 
    private Dictionary<GameObject, ObjectState> initialStates = new Dictionary<GameObject, ObjectState>();

    [Header("Settings")]
    public Transform parentObject;  // reference to the parent object which contains all the objects in the map 

    void Start()
    {
        // check if a parent object is assigned
        if (parentObject != null)
        {
            // iterate through all child objects of the parent 
            foreach (Transform child in parentObject)
            {
                GameObject obj = child.gameObject;

                // capture the initial transform state of each object 
                var state = new ObjectState
                {
                    Position = obj.transform.position,
                    Rotation = obj.transform.rotation,
                    Scale = obj.transform.localScale
                };

                // store the initial state in the dictionary 
                initialStates[obj] = state;
            }
        }
        else
        {
            // for debugging to check if parent object is not assigned 
            Debug.LogWarning("Parent object is not assigned to the MapManager!");
        }
    }

    void Update()
    {
        // if R gets pressed, trigger map reset 
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetMap();
        }
    }

    // method to reset all the tracked object to their initial states 
    public void ResetMap()
    {
        // iterate through all stored object states 
        foreach (var kvp in initialStates)
        {
            GameObject obj = kvp.Key;
            ObjectState state = kvp.Value;

            // restore object's transform to initial states 
            obj.transform.position = state.Position;
            obj.transform.rotation = state.Rotation;
            obj.transform.localScale = state.Scale;

            // reset the physics properties if the object has a rigid body 
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // clear velocity and reset the physics-related properties 
                rb.linearVelocity = Vector3.zero;  
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;  //disabled physics 
                rb.position = state.Position;
                rb.rotation = state.Rotation;
                rb.isKinematic = false; //re-enables it 
            }
        }
        // log msg for debugging 
        Debug.Log("Map has been reset.");
    }
}
