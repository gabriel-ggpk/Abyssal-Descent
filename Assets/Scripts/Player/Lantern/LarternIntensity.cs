using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class LarternIntensity : MonoBehaviour {

    [SerializeField] private Light2D lanternLight2D;
    [SerializeField] private float duration = 60f;

    private float initialIntensity;
    private const float maxIntensity = 10f;
    private Coroutine decreaseCoroutine;

    public event EventHandler OnEnergyChanged;
    public LightBar lightBar;

    // Start is called before the first frame update
    void Start() {
        if (lanternLight2D != null) {
            initialIntensity = lanternLight2D.intensity;
            decreaseCoroutine = StartCoroutine(DecreaseIntensityOverTime());
        } else {
            Debug.LogError("Lantern Light2D is not assigned!");
        }

        lightBar.Setup(this);
    }

    private IEnumerator DecreaseIntensityOverTime() {
        float elapsedTime = 0f;
        while (elapsedTime < duration) {
            lanternLight2D.intensity = Mathf.Lerp(initialIntensity, 0, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            if (OnEnergyChanged != null) OnEnergyChanged(this, EventArgs.Empty);
            yield return null;
        }
        lanternLight2D.intensity = 0;
        //OnIntensityZero(); // Game Over
    }

    private void OnIntensityZero() {
        SceneManager.LoadScene("GameOverScene");
    }

    public float GetIntensity() {
        return lanternLight2D.intensity;
    }

    public float GetIntensityPercent() {
        return lanternLight2D.intensity / maxIntensity;
    }

    public void Recharge(float rechargeAmount) {

        lanternLight2D.intensity += rechargeAmount;

        if (lanternLight2D.intensity > maxIntensity) {
            lanternLight2D.intensity = maxIntensity;
        }

        // Restart the decrease coroutine
        if (decreaseCoroutine != null) {
            StopCoroutine(decreaseCoroutine);
        }
        initialIntensity = lanternLight2D.intensity;
        decreaseCoroutine = StartCoroutine(DecreaseIntensityOverTime());

        if (OnEnergyChanged != null) OnEnergyChanged(this, EventArgs.Empty);
    }
}
