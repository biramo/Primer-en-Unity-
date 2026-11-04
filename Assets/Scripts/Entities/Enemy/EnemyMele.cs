using UnityEngine;

public class EnemyMele : Enemy
{   
    [SerializeField]private float attackRate=3f;
    private float attackRateTime=0;

    void OnEnable()
    {
        enemyAnimator.OnDealDamage+=TakeDamagePlayer;
    }

    void OnDisable()
    {
        enemyAnimator.OnDealDamage-=TakeDamagePlayer;
    }
    protected override void Attack()
    {
        if (Time.time < attackRateTime) return;

        attackRateTime = Time.time + attackRate;
        enemyAnimator.PlayAttack();
    }

    

      
}
