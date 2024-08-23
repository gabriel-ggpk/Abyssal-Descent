using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobInteraction : MonoBehaviour
{
    [Header("physics")]
    [SerializeField] Rigidbody2D monsterRb;
    [SerializeField] float knockback = 10;

    [Header("attributes")]
    [SerializeField] HealthBar healthBar;
    [SerializeField] int life = 100;

    private HealthSystem healthSystem;
    // Start is called before the first frame update
    void Start()
    {
        monsterRb = GetComponent<Rigidbody2D>();
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
            float collisionX = collisionObject.transform.position.x;
            float collisionY = collisionObject.transform.position.y;
            float monterX = transform.position.x;
            float monterY = transform.position.y;
            Vector2 direction = (collisionObject.transform.position - transform.position).normalized;
            collisionObject.GetComponent<Rigidbody2D>().AddForce(direction*knockback,ForceMode2D.Impulse);
            Debug.Log(new Vector2(collisionX - monterX, collisionY - monterY));

        }
    }
}
