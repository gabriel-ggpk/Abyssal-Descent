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
    private bool enableMovement = true;
    private bool isInvincible = false;

    public HealthBar healthBar;
    public HealthSystem playerHealthSystem;
    private void Awake () {
        rigidBody = transform.GetComponent<Rigidbody2D>();
        capCollider2D = transform.GetComponent<CapsuleCollider2D>();
    }
    private void Start ()
    {
        playerHealthSystem = new HealthSystem(100);


        healthBar.Setup(playerHealthSystem);
        playerHealthSystem.Damage(50);
    }

    private void Update () {
        if(enableMovement)
        {

        // Handle movement
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        
        rigidBody.velocity = new Vector2(inputVector.x * moveSpeed, rigidBody.velocity.y);

        // Handle jump
        if (Input.GetButtonDown("Jump") && IsGrounded()) {
            rigidBody.AddForce(new Vector2(rigidBody.velocity.x, jump));
        }
        }
    }
    
    // Check if player is grounded
    private bool IsGrounded() {
        float extraDistance = .1f;
        RaycastHit2D raycastHit = Physics2D.Raycast(capCollider2D.bounds.center, Vector2.down, capCollider2D.bounds.extents.y + extraDistance, tileMapLayerMask);
        return raycastHit.collider != null;
    }
    public IEnumerator getHit (Vector2 force)
    {
        if (isInvincible) yield break;//check later


        enableMovement = false;
        isInvincible = true;
        gameObject.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        playerHealthSystem.Damage(10);
        yield return new WaitForSeconds(0.5f);
        enableMovement = true;
        yield return new WaitForSeconds(1);
        isInvincible = false;
    }
}
