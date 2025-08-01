using UnityEngine;
using System.Collections;
public class AOE_Enemy : MonoBehaviour
{
    [Header("AOE Settings")]
    public float interval = 10f;          // time between pulses
    public float activeDuration = 0.5f;   // how long the AOE is active
    public float damage = 25f;            // damage dealt per pulse
    public string targetTag = "Player";    // tag to identify valid targets

    [Header("References")]
    [Tooltip("Child object with a Trigger Collider2D used as the AOE zone.")]
    public GameObject aoeZone;

    [Header("State List")]
    [SerializeField] AOE_EnemyStateList aoe_StateList;

    private Collider2D[] hitsBuffer = new Collider2D[10]; // small buffer to avoid allocations

    private void Start()
    {
        if (aoeZone == null)
        {
            Debug.LogError("AOE Zone not assigned on " + name);
            enabled = false;
            return;
        }

        // Ensure its collider is disabled initially
        var col = aoeZone.GetComponent<Collider2D>();
        if (col == null || !col.isTrigger)
        {
            Debug.LogError("AOE Zone needs a Collider2D set as Trigger.");
        }

        aoeZone.SetActive(false);
        StartCoroutine(AOELoop());
    }

    private IEnumerator AOELoop()
    {
        while (true)
        {
            aoe_StateList.IsWaitingForAttack = true;
            aoe_StateList.IsAttacking = false;
            yield return new WaitForSeconds(interval);
            // Activate AOE
            aoeZone.SetActive(true);
            ApplyDamageInZone(); // immediate application when it appears (optional: could delay slightly)
            aoe_StateList.IsWaitingForAttack = false;
            aoe_StateList.IsAttacking = true;
            yield return new WaitForSeconds(activeDuration);
            aoeZone.SetActive(false);
        }
    }

    private void ApplyDamageInZone()
    {
        Collider2D col = aoeZone.GetComponent<Collider2D>();
        if (col == null) return;

        // Use OverlapCollider to get current overlaps without allocating GC
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter(); // can customize to layers if needed

        int count = aoeZone.GetComponent<Collider2D>().Overlap(filter, hitsBuffer);
        for (int i = 0; i < count; i++)
        {
            Collider2D hit = hitsBuffer[i];
            if (hit != null && hit.CompareTag(targetTag))
            {
                // Try to damage
                playercontroller dmg = hit.GetComponent<playercontroller>();
                if (dmg != null)
                {
                    dmg.TakeDamge(damage);
                }
                else
                {
                    // fallback: send message (optional)
                    hit.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }

    // (Optional) Visual debug
    private void OnDrawGizmosSelected()
    {
        if (aoeZone != null)
        {
            Collider2D c = aoeZone.GetComponent<Collider2D>();
            if (c is CircleCollider2D circle)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(circle.transform.position + (Vector3)circle.offset, circle.radius * circle.transform.lossyScale.x);
            }
            else if (c is BoxCollider2D box)
            {
                Gizmos.color = Color.red;
                Vector3 size = new Vector3(box.size.x * box.transform.lossyScale.x, box.size.y * box.transform.lossyScale.y, 1);
                Gizmos.DrawWireCube(box.transform.position + (Vector3)box.offset, size);
            }
        }
    }
}
