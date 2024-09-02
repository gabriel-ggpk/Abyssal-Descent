using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using Assets.Scripts.ProceduralGeneration;
public class Player : MonoBehaviour {

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jump = 400f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask tileMapLayerMask;
    [SerializeField] private GameObject pickaxe;
    [SerializeField] private Tilemap enviroment;
    private Animator animator;
    private Rigidbody2D rigidBody;
    private CapsuleCollider2D capCollider2D;
    private bool enableMovement = true;
    private bool isInvincible = false;
    private bool facingLeft = true;

    public HealthBar healthBar;
    public HealthSystem playerHealthSystem;
    private void Awake () {
        rigidBody = transform.GetComponent<Rigidbody2D>();
        capCollider2D = transform.GetComponent<CapsuleCollider2D>();
        animator = pickaxe.GetComponent<Animator>();

    }
    private void Start ()
    {
        playerHealthSystem = new HealthSystem(100);

        healthBar.Setup(playerHealthSystem);
    }

    private void Update () {
        if(enableMovement)
        {

        // Handle movement
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
            bool isSlowed = (inputVector.x > 0 && !facingLeft || inputVector.x <= 0 && facingLeft);
        rigidBody.velocity = new Vector2(inputVector.x * moveSpeed * (isSlowed ? 0.75f : 1f), rigidBody.velocity.y);

        // Handle jump
        if (Input.GetButtonDown("Jump") && IsGrounded()) {
            rigidBody.AddForce(new Vector2(rigidBody.velocity.x, jump));
        }
        if(Input.GetMouseButtonDown(0))
            {
                Debug.Log("attack");
                attack();
            }
        }
    }

    private void FixedUpdate () {

        FlipPlayer();
    }

    private void CalcPlayerLookDirection()
    {
        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction from the spotlight to the mouse position
        Vector3 direction = mousePosition - transform.position;
        if (direction.x > 0) facingLeft = true;
        else facingLeft = false;
    }
    private void FlipPlayer()
    {
        CalcPlayerLookDirection();
        if (facingLeft)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (!facingLeft)
        {
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    // Check if player is grounded
    public bool IsGrounded() {
        float extraDistance = .1f;
        RaycastHit2D raycastHit = Physics2D.Raycast(capCollider2D.bounds.center, Vector2.down, capCollider2D.bounds.extents.y + extraDistance, tileMapLayerMask);
        return raycastHit.collider != null;
    }
    public IEnumerator getHit (Vector2 force,int damage)
    {
        if (isInvincible) yield break;//check later


        enableMovement = false;
        isInvincible = true;
        gameObject.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        playerHealthSystem.Damage(damage);
        yield return new WaitForSeconds(0.5f);
        enableMovement = true;
        yield return new WaitForSeconds(1);
        isInvincible = false;
    }

    public void attack()
    {
        animator.SetTrigger("Attack");
        MapController mapController = enviroment.GetComponent<MapController>();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(transform.position, mouseWorldPos);
        Debug.Log(distance);
        if (mapController != null &&  distance<2)
        {
            mapController.RemoveTile(enviroment.WorldToCell(mouseWorldPos));
        }
    }
}
