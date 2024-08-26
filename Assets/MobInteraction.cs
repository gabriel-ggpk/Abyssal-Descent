using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobInteraction : MonoBehaviour
{
    [Header("physics")]
    [SerializeField] float knockback = 5;

    [Header("attributes")]
    [SerializeField] HealthBar healthBar;
    [SerializeField] int life = 100;

    private HealthSystem healthSystem;
    // Start is called before the first frame update
    void Start()
    {
        healthBar = gameObject.GetComponentInChildren<HealthBar>();
        healthSystem = new HealthSystem(life);

        healthBar.Setup(healthSystem);
        healthSystem.Damage(50);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (collisionObject.CompareTag("Player"))
        {
            Vector2 direction = (collisionObject.transform.position - transform.position).normalized;
            Player player = collisionObject.GetComponent<Player>();
            player.StartCoroutine(player.getHit(direction * knockback));

        }
    }
}
