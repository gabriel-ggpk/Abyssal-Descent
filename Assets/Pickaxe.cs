using System.Collections;
using UnityEngine;

public class Pickaxe : MonoBehaviour
{
    private Player player;
    private BoxCollider2D pickCollider;
    void Start()
    {
        player = GetComponentInParent<Player>(); // Assuming PlayerAttack is the script on the parent with StartAttack
        pickCollider = GameObject.Find("PickaxeHitbox").GetComponent<BoxCollider2D>();
    }

    public void StartAttack()
    {
        if (player != null)
        {

            pickCollider.enabled = true;
        }
    }

    public void EndAttack()
    {
      StartCoroutine( DisableCollider());
    }

    private IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(0.2f) ;
        if (player != null)
        {
            pickCollider.enabled = false;
        }
    }
}
