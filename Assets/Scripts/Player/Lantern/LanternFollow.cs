using System;
using UnityEngine;

public class PointAtMouseSmooth : MonoBehaviour
{
    public float rotationSpeed = 10f; // Speed at which the spotlight rotates towards the cursor
    [SerializeField] float offset = 0f;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction from the spotlight to the mouse position
        Vector3 direction = mousePosition - transform.position ;

        // Calculate the target angle in degrees -90deg so that it's pointing to cursor properly
        float targetAngle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg-offset);

        // Get the current rotation angle of the spotlight
        float currentAngle = transform.rotation.eulerAngles.z;

        // Smoothly interpolate the current angle towards the target angle
        float angle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * rotationSpeed);

        // Set the rotation of the spotlight to the new smoothed angle
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
