using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // set push power to a default value
    private float pushPower = 4f; 
    
    // method that allows response gbetween character controller and other colliders
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // save the rigid body that the character controller just "hit"
        Rigidbody body = hit.collider.attachedRigidbody;
        // check if body does not have a rigit body or if body is kinematic then return function / skip 
        if(body == null || body.isKinematic)
            return;
        // checks if character controller is moving at a steep angle then just skip again
        if(hit.moveDirection.y < -0.3f)
            return;
        // save push direction vector
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.linearVelocity = pushDir * pushPower;
    }
}
