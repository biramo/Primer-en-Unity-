using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
 
    private NavMeshAgent navMeshAgent;
    [SerializeField] private float rangoDeteccion=10f;

    [SerializeField] private int damage=20;
    [SerializeField] private float speed = 5f;

    private EnemyState stateEnemy;
    
    private float attackRate=1f;
    private float attackRateTime=0;
    private float updatePathRate = 0.2f;
    private float nextPathUpdate=0;


    private Player player;
    void Start()
    {
        navMeshAgent=GetComponent<NavMeshAgent>();
        player=FindAnyObjectByType<Player>();
        stateEnemy=EnemyState.Patrol;

        if (player == null)//Si no encuentra al player desactivamos los update
        {
            Debug.LogWarning("No se ha encontrado ningun jugador",this);
            enabled = false;
        }
        navMeshAgent.speed=speed;
    }
    //--------------------------------[Updates]--------------------------------

    void Update()
    {   
        UpdateState();
        ExecuteState();

    }

    private void UpdateState()
    {
        if(Time.time>=nextPathUpdate){
            if(CalculateDistanceFromPlayer()<=rangoDeteccion){
                ChangeState(EnemyState.Chasing);                              
            }
            else
            {
                ChangeState(EnemyState.Patrol);                              

                
            }
            nextPathUpdate=Time.time+updatePathRate;
        }
    }

    private float CalculateDistanceFromPlayer()//Nos devuelve la distancia entre el enemigo y el player
    {
        return Vector3.Distance(transform.position,player.transform.position);

    }

    //--------------------------------[Ejecucion de los estados]--------------------------------

    private void Chasing()
    {
        navMeshAgent.destination=player.transform.position;
        
    }

    private void ExecuteState()
    {
        switch (stateEnemy)
        {
            case EnemyState.Attack: break;
            case EnemyState.Chasing: Chasing(); break;
            case EnemyState.Patrol: break;
            case EnemyState.Dead: onEntityDead(); break;

        }
    }

    //Cambiar el estado actual
    private void ChangeState(EnemyState state)
    {
        if(stateEnemy==state) return;

        stateEnemy=state;
    }
      private void OnCollisionStay(Collision collision)
    {   
        if (Time.time < attackRateTime) return;
        
        Player hitPlayer = collision.transform
            .GetComponentInParent<Player>();

        if (hitPlayer == null) return;

        hitPlayer.TakeDamage(damage);
        Debug.Log(hitPlayer.Health);

        attackRateTime = Time.time + attackRate;
    }
    public override void onEntityDead()
    {     
        stateEnemy=EnemyState.Dead;
        Destroy(gameObject);  
    }
}
