using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Transform = UnityEngine.Transform;

public class DistanceMeasurer : Enemy
{
    public Transform object1;
    public Transform object2;

    void Update()
    {
        base.Update();
        float distance = Vector3.Distance(object1.position, transform.position);
        if (distance < 5f)
        {
            followtheplayer();
        }
    }
    private void followtheplayer()
    {
     if (!isRecoiling)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(playercontroller.Instanece.transform.position.x, playercontroller.Instanece.transform.position.y), speed* Time.deltaTime);
}
    }
    public override void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyHit(_damageDone, _hitDirection, _hitForce);
    }

}