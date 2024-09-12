using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHeart : MonoBehaviour {

    [SerializeField] private int healAmount = 20;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip healSFX;

    private void OnTriggerEnter2D(Collider2D collision) {
        
        Player player = collision.GetComponent<Player>();

        if (player != null) {
            if (player.playerHealthSystem.GetHealth() < player.playerHealthSystem.GetMaxHealth()) {  // If the player's health is full, don't heal
                player.GetHeal(healAmount);
                audioSource.PlayOneShot(healSFX);
                StartCoroutine(DestroyAfterDelay()); // Destroy the heart object after healing
            }
        }
    }

    private IEnumerator DestroyAfterDelay() {
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }

}