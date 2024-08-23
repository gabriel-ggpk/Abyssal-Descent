using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jump = 400f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask tileMapLayerMask;
    private Rigidbody2D rigidBody;
    private CapsuleCollider2D capCollider2D;
    private bool enableMovement = true

    private void Awake () {
        rigidBody = transform.GetComponent<Rigidbody2D>();
        capCollider2D = transform.GetComponent<CapsuleCollider2D>();
    }

    private void Update () {

        // Handle movement
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        
        rigidBody.velocity = new Vector2(inputVector.x * moveSpeed, rigidBody.velocity.y);

        // Handle jump
        if (Input.GetButtonDown("Jump") && IsGrounded()) {
            rigidBody.AddForce(new Vector2(rigidBody.velocity.x, jump));
        }
    }
    
    // Check if player is grounded
    private bool IsGrounded() {
        float extraDistance = .1f;
        RaycastHit2D raycastHit = Physics2D.Raycast(capCollider2D.bounds.center, Vector2.down, capCollider2D.bounds.extents.y + extraDistance, tileMapLayerMask);
        return raycastHit.collider != null;
    }
    public IEnumerator knockBack (Vector2 force)
    {
        enableMovement = false;
        gameObject.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
    }
}
