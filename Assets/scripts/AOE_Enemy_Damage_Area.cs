using UnityEngine;

public class AOE_Enemy_Damage_Area : Enemy
{
    private AOEEnemySFX aoeEnemySFX;

    void Start()
    {
        // Get reference to EnemyCombat script on the same GameObject
        aoeEnemySFX = GetComponent<AOEEnemySFX>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("player"))
        {
            aoeEnemySFX.PlayAttackAOE();
            collision.gameObject.GetComponent<playercontroller>().TakeDamge(5);
        }
    }
}
