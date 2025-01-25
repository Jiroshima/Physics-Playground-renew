using UnityEngine;


// Attach a collision sound to the child objects of GameObjects

public class AttachCollisionSound : MonoBehaviour
{
    // this is the audio that we will assign 
    [SerializeField] 
    private AudioClip collisionSound;  

    private void Start()
    {
        // get the parent GameObject 
        GameObject mapObjectsParent = this.gameObject;  // assume the script is attached to the parent object 

        // checks if the parent object exists 
        if (mapObjectsParent != null)
        {
            // loop through all the child objects of the parent 
            foreach (Transform child in mapObjectsParent.transform)
            {
               // check if the child has a collider component 
                if (child.GetComponent<Collider>() != null)
                {
                    // try  to get the collisionSound script on the child object 
                    CollisionSound collisionSoundScript = child.GetComponent<CollisionSound>();

                    // if its not attached then add it 
                    if (collisionSoundScript == null)
                    {
                        collisionSoundScript = child.gameObject.AddComponent<CollisionSound>();
                    }

                    // assign the collision sound to the collision script 
                    collisionSoundScript.SetCollisionSound(collisionSound);
                }
            }
        }
    }
}
