using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Smoothly tracks the motion of the game object specified in <c>target</c>.
/// </summary>
public class CameraTrack : MonoBehaviour
{
    /// <summary>
    /// A reference to the object that will be tracked.
    /// </summary>
    public Transform target;
    
    /// <summary>
    /// A multiplier for the speed at which the camera will catch up to the tracked object.
    /// </summary>
    public float smoothTime = 0.3F;
    
    /// <summary>
    /// A multiplier for the speed at which the camera's rotation will line up with the object's.
    /// </summary>
    public float rotationSpeed = 5f;
    
    /// <summary>
    /// The current speed of the camera.
    /// </summary>
    private Vector3 velocity = Vector3.zero;
    
    /// <summary>
    /// The original relative position of the camera in relation to the tracked object.
    /// </summary>
    private Vector3 distance = Vector3.zero;
    
    /// <summary>
    /// Which difference in angular there was between the camera and the tracked object at the last frame.
    /// </summary>
    private float previousAngle = 0;

    void Start()
    {
        distance = target.InverseTransformPoint(transform.position);
    }

    void Update()
    {
        // Define a target position above and behind the target transform
        Vector3 targetPosition = target.TransformPoint(distance);
        Quaternion targetRotation = Quaternion.Euler(15, target.transform.eulerAngles.y, 0);
        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        float adjustedSpeed = smallestDifference(transform.rotation, targetRotation) * rotationSpeed;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, adjustedSpeed * Time.deltaTime);
    }

    /// <summary>
    /// An approximate calculation of the angle between the camera and the tracked object in the <c>y</c> plane.
    /// </summary>
    /// <param name="one"> the rotation of one object to measure.</param>
    /// <param name="two"> The rotation of the other object to measure.</param>
    /// <returns>
    /// A positive angle between 1 and 180 of the difference of where the two objects are pointing.
    /// </returns>
    float smallestDifference(Quaternion one, Quaternion two)
    {

        float angle =one.eulerAngles.y % 180 - two.eulerAngles.y % 180;
        angle = Mathf.Abs(angle % 180);
        if (angle < 1) {
            angle = 0;
        }
        // This is required, otherwise there will be a jump when one angle moves between positive and negative value.
        if (angle - previousAngle > 100) {
            angle = previousAngle;
        }
        previousAngle = angle;
        return angle;
    }
}
