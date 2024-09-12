using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuController : MonoBehaviour {

    [SerializeField] private AudioClip clickSFX;
    [SerializeField] private AudioClip hoverSFX;
    [SerializeField] private AudioSource audioSource;

    public void PlayHoverSound() {
        if (hoverSFX != null) {
            audioSource.PlayOneShot(hoverSFX);
        }
    }

    public void PlayClickSound() {
        if (clickSFX != null) {
            audioSource.PlayOneShot(clickSFX);
        }
    }
    public void LoadMenuScene() {
        SceneManager.LoadScene("MenuScene");
    }
}