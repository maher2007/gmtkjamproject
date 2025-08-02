using UnityEngine;
using System.Collections;
public class AOE_Enemy : Enemy
{
    [Header("AOE Settings")]
    public float interval = 10f;          // time between pulses
    public float activeDuration = 0.5f;   // how long the AOE is active
    public float damager = 25f;            // damage dealt per pulse
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
       
    }

    private void Update()
    {
        base.Update();
        if (aoeZone == null) Destroy(gameObject);
    }
    private IEnumerator AOELoop()
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
        Destroy(this.gameObject);
         
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            StartCoroutine(AOELoop());
        }
    }


    // (Optional) Visual debug

}
