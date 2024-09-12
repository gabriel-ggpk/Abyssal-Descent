using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class LarternIntensity : MonoBehaviour {

    [SerializeField] private Light2D lanternLight2D;
    private float initialIntensity;
    private float duration = 60f;

    // Start is called before the first frame update
    void Start() {
        if (lanternLight2D != null) {
            initialIntensity = lanternLight2D.intensity;
            StartCoroutine(DecreaseIntensityOverTime());
        }
    }

    private IEnumerator DecreaseIntensityOverTime() {
        float elapsedTime = 0f;
        while (elapsedTime < duration) {
            lanternLight2D.intensity = Mathf.Lerp(initialIntensity, 0, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        lanternLight2D.intensity = 0;
    }
}
