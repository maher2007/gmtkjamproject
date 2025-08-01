using UnityEngine;

public class AOE_Enemy_Damage_Area : Enemy
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("player"))
        {
            collision.gameObject.GetComponent<playercontroller>().TakeDamge(5);
        }
    }
}
