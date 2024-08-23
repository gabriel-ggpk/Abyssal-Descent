using UnityEngine;

public class GameHandler : MonoBehaviour {

    public HealthBar healthBar;
    public HealthSystem playerHealthSystem;
    private void Start() {
        playerHealthSystem = new HealthSystem(100);

        healthBar.Setup(playerHealthSystem);

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
