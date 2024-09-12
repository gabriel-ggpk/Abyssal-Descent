using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassLogic : MonoBehaviour
{
    public Transform player;      // The player's transform
    public Vector3 destination; // The destination's transform
    private RectTransform compassRectTransform; // The RectTransform of the compass UI
    [SerializeField] float offset = -45f;
    void Start()
    {
        compassRectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Step 1: Get the direction vector from the player to the destination
        Vector2 direction = destination - player.position;

        // Step 2: Calculate the angle in radians and convert to degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Step 3: Set the rotation of the compass UI (around the Z axis)
        compassRectTransform.rotation = Quaternion.Euler(0, 0, (angle + offset)); // Negative to point correctly
    }
}
