using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEnergy : MonoBehaviour {

    [SerializeField] private float rechargeAmount = .5f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip rechargeSFX;

    private void OnTriggerEnter2D(Collider2D collision) {

        Player player = collision.GetComponent<Player>();

        //Debug.Log("teste");

        LarternIntensity lantern = player.GetComponentInChildren<LarternIntensity>();
        if (player != null) {
            if (lantern.GetIntensity() < 10) {  // If the player's lantern energy is full, don't recharge
                lantern.Recharge(rechargeAmount);
                audioSource.PlayOneShot(rechargeSFX);
                //StartCoroutine(DestroyAfterDelay()); // Destroy the energy object after recharging
            }
        }
    }

    private IEnumerator DestroyAfterDelay() {
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }

}
