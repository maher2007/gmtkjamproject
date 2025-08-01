using UnityEngine;
using System.Collections;
public class AOE_Enemy : Enemy
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
            aoe_StateList.IsWaitingForAttack = false;
            aoe_StateList.IsAttacking = true;
            yield return new WaitForSeconds(activeDuration);
            aoeZone.SetActive(false);
        }
    }



    // (Optional) Visual debug

}
