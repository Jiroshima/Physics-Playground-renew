using System;
using UnityEngine;

public class PhysGun : MonoBehaviour
{
    [SerializeField]
    private float _maxGrabDistance = 40f; // maximum grabbing distance 
    [SerializeField]
    private float _minGrabDistance = 1f; // minimum grabbing distance 
    [SerializeField]
    private LineRenderer _pickLine; // line renderer to visualise the path between the gun and the grabbed object 
    [SerializeField] 
    private Transform _barrelPoint;

    
    public AudioClip grabSound;  // audio to play when object is grabbed
    private AudioSource audioSource;  // component to play the audio 
    private Rigidbody _grabbedObject; // the rigidbody that is currently being grabbe d
    private float _pickDistance; // current distance of the grabbed object 
    private Vector3 _pickOffset; // offset to maintain the original grab position of the object 
    private Vector3 _pickTargetPosition; // destination position for the grabbed object 
    private Vector3 _pickForce; // force applied to move the object 

    private void Start()
    {
        // fallback to using the current transform is no barrel point is set 
        if (!_barrelPoint)
        {
            _barrelPoint = transform;
        }

        // create a line renderer if none is assigned 
        if (!_pickLine)
        {
            // dynamically create a new gameobject with linerenderer
            var obj = new GameObject("PhysGun Pick Line");
            _pickLine = obj.AddComponent<LineRenderer>();

            // line renderer properties 
            _pickLine.startWidth = 0.02f;
            _pickLine.endWidth = 0.02f;
            _pickLine.useWorldSpace = true;
            _pickLine.gameObject.SetActive(false);
        }

        // get or add audio source component for sound effects 
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // if left mouse button is pressed, grab the object 
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Grab();
        }

        // if the right mouse button is pressed down, release the object and freeze it if necessary
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (_grabbedObject)
            {
                Release(true);
            }
        }

        // if the left mouse button is released, release the object
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (_grabbedObject)
            {
                Release();
            }
        }

        // adjust the grabbing distance with the mouse scroll wheel
        _pickDistance = Mathf.Clamp(_pickDistance + Input.mouseScrollDelta.y, _minGrabDistance, _maxGrabDistance);
    }

    private void LateUpdate()
    {
        // visualise the path of the grabbed object 
        if (_grabbedObject)
        {
            // calculate a midpoint with a curve 
            var midpoint = (transform.position + _pickTargetPosition) / 2f;
            midpoint += Vector3.ClampMagnitude(_pickForce / 2f, 1f);
            // draw a curved line between the gun and the object 
            DrawQuadraticBezierCurve(_pickLine, _barrelPoint.position, midpoint, _grabbedObject.worldCenterOfMass - _pickOffset);
        }
    }

    private void FixedUpdate()
    {
        if (_grabbedObject != null)
        {
            // calculate the target position based on camera center, then calculate the force to move the object smoothly
            var ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
            _pickTargetPosition = (ray.origin + ray.direction * _pickDistance) + _pickOffset;
            var forceDir = _pickTargetPosition - _grabbedObject.position;
            _pickForce = forceDir / Time.fixedDeltaTime * 0.3f / _grabbedObject.mass;
            // apply the movement force
            _grabbedObject.linearVelocity = _pickForce;
        }
    }

    private void Grab()
    {
        // cast a ray from the camera to see if there is an object within range
        var ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        // attempt to grab the object within max distance 
        if (Physics.Raycast(ray, out RaycastHit hit, _maxGrabDistance, ~0) && hit.rigidbody != null)
        {
            // calculate grab offset to maintain object's original orientation 
            _pickOffset = hit.transform.InverseTransformVector(hit.point - hit.transform.position);
            _pickDistance = hit.distance;
            _grabbedObject = hit.rigidbody;
            // physics properties for smooth manipulation 
            _grabbedObject.collisionDetectionMode = CollisionDetectionMode.Continuous;
            _grabbedObject.useGravity = false;
            _grabbedObject.freezeRotation = true;
            _grabbedObject.isKinematic = false;
            _pickLine.gameObject.SetActive(true);

            // play grab sound when an object is successfully grabbed (if not already playing)
            if (audioSource != null && grabSound != null)
            {
                audioSource.clip = grabSound;
                audioSource.loop = true;  // set to loop while grabbing
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();  // start playing if not already playing
                }
            }
        }
    }

    private void Release(bool freeze = false)
    {
        if (_grabbedObject == null) return;

        if (freeze)
        {
            // stop object movement 
            _grabbedObject.linearVelocity = Vector3.zero;
            _grabbedObject.angularVelocity = Vector3.zero;
            _grabbedObject.isKinematic = true; // set to kinematic to freeze it
        }
        else
        {
            // restore normal physics behaviour
            _grabbedObject.collisionDetectionMode = CollisionDetectionMode.Discrete;
            _grabbedObject.useGravity = true;
            _grabbedObject.freezeRotation = false;
            _grabbedObject.isKinematic = false;
        }
        // reset visual 
        _pickLine.gameObject.SetActive(false);
        _grabbedObject = null; // clear the reference

        // stop grab sound when releasing 
        if (audioSource != null)
        {
            audioSource.loop = false;  // disable looping
            audioSource.Stop();  // stop the sound
        }
    }

    // website that helped with the math of the drawing the line, i dont fully understand it yet but it works
    // https://www.codinblack.com/how-to-draw-lines-circles-or-anything-else-using-linerenderer/
    void DrawQuadraticBezierCurve(LineRenderer line, Vector3 point0, Vector3 point1, Vector3 point2)
    {
        line.positionCount = 20;
        for (int i = 0; i < line.positionCount; i++)
        {
            float t = i / (float)(line.positionCount - 1);
            Vector3 B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
            line.SetPosition(i, B);
        }
    }
}
