using Pathfinding;
using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{ 

    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 2f, jumpForce = 10f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;

    public float jumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true, isJumping, isInAir;
    public bool directionLookEnabled = true;
    public float jumpCD = 1f;

    [SerializeField] Vector3 startOffset;

    private Path path;
    private int currentWaypoint = 0;
    [SerializeField] public RaycastHit2D isGrounded;
    [SerializeField] private LayerMask tileMapLayerMask;
    Seeker seeker;
    Rigidbody2D rb;
    Collider2D collider2d;
    private bool isOnCoolDown;

    [Header("physics")]
    [SerializeField] float knockback = 5;

    [Header("attributes")]
    [SerializeField] HealthBar healthBar;
    [SerializeField] int life = 100;

    private HealthSystem healthSystem;
    private bool enableMovement = true;
    private bool isInvincible = false;



    public void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        isJumping = false;
        isInAir = false;
        isOnCoolDown = false;
        GameObject targetGO = GameObject.FindGameObjectWithTag("Player");
        if (targetGO != null) target = targetGO.GetComponent<Transform>();
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
        healthBar = gameObject.GetComponentInChildren<HealthBar>();
        healthSystem = new HealthSystem(life);

        healthBar.Setup(healthSystem);
    }

    private void FixedUpdate()
    {
        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
        }
    }

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }
    private bool IsGrounded()
    {
        float extraDistance = .1f;
        RaycastHit2D raycastHit = Physics2D.Raycast(collider2d.bounds.center, Vector2.down, collider2d.bounds.extents.y + extraDistance, tileMapLayerMask);
        return raycastHit.collider != null;
    }

    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }
        if (!enableMovement) return;
        // Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed;

        // Jump
        if (jumpEnabled && IsGrounded() && !isInAir && !isOnCoolDown)
        {

            if (direction.y > jumpNodeHeightRequirement)
            {
                //if (isInAir) return;
                isJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                StartCoroutine(JumpCoolDown());

            }
        }
        if (IsGrounded())
        {
            isJumping = false;
            isInAir = false;
        }
        else
        {
            isInAir = true;
        }

        // Movement
        rb.velocity = new Vector2(force.x, rb.velocity.y);

        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Direction Graphics Handling
        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    IEnumerator JumpCoolDown()
    {
        isOnCoolDown = true;
        yield return new WaitForSeconds(jumpCD);
        isOnCoolDown = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (collisionObject.CompareTag("Weapon"))
        {
            Pickaxe pickaxe = collisionObject.GetComponent<Pickaxe>();
            if (pickaxe.isAttacking)
            {
                Vector2 direction = ( transform.position- collisionObject.transform.position).normalized;

                StartCoroutine(getHit(direction * knockback));
            }


        }
        else if (collisionObject.CompareTag("Player"))
        {
            Vector2 direction = (collisionObject.transform.position - transform.position).normalized;
            Player player = collisionObject.GetComponent<Player>();
            player.StartCoroutine(player.getHit(direction * knockback, 34));

        }
    }
    public IEnumerator getHit(Vector2 force)
    {
        if (isInvincible) yield break;

        enableMovement = false;
        isInvincible = true;
        gameObject.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        healthSystem.Damage(10);
        if (healthSystem.GetHealth() == 0)
        {
            Destroy(gameObject);
            StopAllCoroutines();
        }
     
        yield return new WaitForSeconds(0.5f);
        enableMovement = true;
        yield return new WaitForSeconds(1);
        isInvincible = false;
    }
}