using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SpriteAnimator : MonoBehaviour {

    [SerializeField] private Sprite[] frameArray;

    private int currentFrame;
    private float timer;
    private float framerate = .1f;
    private SpriteRenderer spriteRenderer;
    private bool loop = true;
    private bool isPlaying = true;
    private int loopCounter = 0;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (frameArray != null) {
            PlayAnimation(frameArray, framerate);
        } else {
            isPlaying = false;
        }
    }

    
    private void Update() {

        if (!isPlaying) {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= framerate) {
            timer -= framerate;
            currentFrame = (currentFrame + 1) % frameArray.Length;
            if (!loop && currentFrame == 0) {
                StopPlaying();
            } else {
                spriteRenderer.sprite = frameArray[currentFrame];
            }
        }
    }
    
    private void StopPlaying() {
        isPlaying = false;
    }

    public void PlayAnimation(Sprite[] frameArray, float framerate, bool loop = true) {
        this.frameArray = frameArray;
        this.framerate = framerate;
        this.loop = loop;
        isPlaying = true;
        currentFrame = 0;
        timer = 0f;
        loopCounter = 0;
        spriteRenderer.sprite = frameArray[currentFrame];
    }
}
