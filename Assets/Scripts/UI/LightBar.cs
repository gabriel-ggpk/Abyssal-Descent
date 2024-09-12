using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBar : MonoBehaviour
{
    private LarternIntensity lanternIntensity;

    public void Setup(LarternIntensity lanternIntensity) {
        this.lanternIntensity = lanternIntensity;

        lanternIntensity.OnEnergyChanged += LanternIntensity_OnEnergyChanged;
    }

    private void LanternIntensity_OnEnergyChanged(object sender, System.EventArgs e) {
        transform.Find("Bar").localScale = new Vector3(lanternIntensity.GetIntensityPercent(), 1);
    }
}
