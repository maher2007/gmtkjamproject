using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float recoilLength;
    [SerializeField] protected float recoilFactor;
    [SerializeField] protected bool isRecoiling = false;
    [SerializeField] protected playercontroller player;
    [SerializeField] protected float speed;
    [SerializeField] protected float damage;

    protected float recoiltTimer;
    protected Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        
    }
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = playercontroller.Instanece;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        if (isRecoiling)
        {
            if(recoiltTimer < recoilLength)
            {
                recoiltTimer += Time.deltaTime;
            }
            else
            {
                isRecoiling = false ;
                recoiltTimer = 0 ;
            }
        }
    }
    public virtual void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        health -= _damageDone;
        if (!isRecoiling)
        {
            rb.AddForce(-_hitForce * recoilFactor * _hitDirection);
            isRecoiling = true;
        }
    }
    protected virtual void OnTriggerStay2D(Collider2D _other)
    {
        if (_other.CompareTag("player") && !playercontroller.Instanece.pstate.invincible )
        {
            Attack();
            playercontroller.Instanece.HitStopTime(0, 5, 0.5f);
        }
    }
    protected virtual void Attack()
    {
        playercontroller.Instanece.TakeDamge(damage);
    }
}
