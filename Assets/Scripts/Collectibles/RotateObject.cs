using UnityEngine;

// Script to rotate objects
public class RotateObject : MonoBehaviour
{
    //  Variable to set rotate speed for the object
    public float rotateSpeed = 0.5f;
    void Update()
    {
        // Rotate the object relative to the world space around the object
        transform.Rotate(0f, rotateSpeed, 0f, Space.World);
    }
}
