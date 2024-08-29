using UnityEngine;

public class Pickaxe : MonoBehaviour
{
    private Player player;
    void Start()
    {
        player = GetComponentInParent<Player>(); // Assuming PlayerAttack is the script on the parent with StartAttack
    }

    public void StartAttack()
    {
        if (player != null)
        {
        PolygonCollider2D collider = gameObject.GetComponent<PolygonCollider2D>();
            collider.enabled = true;
        }
    }

    public void EndAttack()
    {
        if (player != null)
        {
            PolygonCollider2D collider = gameObject.GetComponent<PolygonCollider2D>();
            collider.enabled = false;
        }
    }
}
