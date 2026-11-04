using UnityEngine;

public class EnemyMele : Enemy
{   
    [SerializeField] private EnemyAnimator enemyAnimator;
    [SerializeField]private float attackRate=3f;
    private float attackRateTime=0;
    void Awake()
    {   

        if(enemyAnimator==null)enemyAnimator=GetComponentInChildren<EnemyAnimator>();
    }
    protected override void Attack()
    {
        if (Time.time < attackRateTime) return;

        attackRateTime = Time.time + attackRate;
        enemyAnimator.PlayAttack();
    }

      
}
