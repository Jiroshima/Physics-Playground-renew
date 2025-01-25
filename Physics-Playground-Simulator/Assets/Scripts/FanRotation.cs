using UnityEngine;
/// <summary>
/// Rotates the blades of the turbines
/// </summary>

public class FanRotation : MonoBehaviour
{
    public float rotationSpeed = 200f;

    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
