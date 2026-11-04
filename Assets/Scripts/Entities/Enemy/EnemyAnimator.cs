using UnityEngine;

public class EnemyAnimator: MonoBehaviour
{
    [SerializeField] private Animator animator;

    private int lastAttack;

    void Awake()
    {   
        if(animator==null)animator=GetComponentInChildren<Animator>();
    }

    public void SetState(EnemyState state)
    {
        if (animator == null) return;

        animator.SetInteger(AnimParams.State, (int)state);
    }
    public void PlayAttack()
    {   
        if(animator==null)return;

        int attackType;
        do
        {
            attackType=Random.Range(0,3);

        }while(attackType==lastAttack);

        lastAttack=attackType;
        animator.SetInteger(AnimParams.AttackType,attackType);
        animator.SetTrigger(AnimParams.Attack);
    }



}