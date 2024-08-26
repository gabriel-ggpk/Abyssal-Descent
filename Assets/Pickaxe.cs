using UnityEngine;

public class Pickaxe : MonoBehaviour
{
    private Player player;
    public bool isAttacking = false;
    void Start()
    {
        player = GetComponentInParent<Player>(); // Assuming PlayerAttack is the script on the parent with StartAttack
    }

    public void StartAttack()
    {
        if (player != null)
        {
           isAttacking = true;
        }
    }

    public void EndAttack()
    {
        if (player != null)
        {
            isAttacking = false;
        }
    }
}
