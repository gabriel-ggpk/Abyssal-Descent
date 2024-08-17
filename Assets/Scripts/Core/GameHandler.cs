using UnityEngine;

public class GameHandler : MonoBehaviour {

    public HealthBar healthBar;

    private void Start() {
        HealthSystem healthSystem = new HealthSystem(100);

        healthBar.Setup(healthSystem);

        /*
        healthSystem.Damage(10);
        Debug.Log(healthSystem.GetHealthPercent());
        Debug.Log(healthBar.transform.Find("Bar").localScale);
        healthSystem.Heal(10);
        Debug.Log(healthSystem.GetHealthPercent());
        Debug.Log(healthBar.transform.Find("Bar").localScale);
        */
    }

}
