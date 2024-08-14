using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jump = 300f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask tileMapLayerMask;
    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider2D;

    private void Awake () {
        rigidBody = transform.GetComponent<Rigidbody2D>();
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
    }

    private void Update () {

        // Handle movement
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, 0f);
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        // Handle jump
        if (Input.GetButtonDown("Jump") && IsGrounded()) {
            rigidBody.AddForce(new Vector2(rigidBody.velocity.x, jump));
        }
    }
    
    // Check if player is grounded
    private bool IsGrounded() {
        float extraDistance = .1f;
        RaycastHit2D raycastHit = Physics2D.Raycast(boxCollider2D.bounds.center, Vector2.down, boxCollider2D.bounds.extents.y + extraDistance, tileMapLayerMask);
        return raycastHit.collider != null;
    }
}
