using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
 
    private NavMeshAgent navMeshAgent;
    [SerializeField] private float rangeDetection=10f;

    [SerializeField] private int damage=20;
    [SerializeField] private float speed = 5f;

    private EnemyState stateEnemy;
    
    private float attackRate=1f;
    private float attackRateTime=0;
    private float updatePathRate = 0.2f;
    private float nextPathUpdate=0;

    private float attackRange=2f;


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
/*___________________________________________________________________________________
|                                                                                    |
|                                                                                    |
|--------------------------------[updates]-------------------------------------------|
|                                                                                    |
|____________________________________________________________________________________|
*/
    void Update()
    {   
        UpdateState();
        ExecuteState();

    }
    private void UpdateState()
    {   
        float DistanceFromPlayer=CalculateDistanceFromPlayer();
        if(Time.time>=nextPathUpdate){
            if(DistanceFromPlayer<= attackRange){
                ChangeState(EnemyState.Attack);                              
            }
            else if (DistanceFromPlayer<=rangeDetection)
            {
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

/*___________________________________________________________________________________
|                                                                                    |
|                                                                                    |
|--------------------------------[Ejecucion de los estados]--------------------------|
|                                                                                    |
|____________________________________________________________________________________|
*/
    private void Chasing()
    {
        navMeshAgent.destination=player.transform.position;
        
    }

    private void ExecuteState()
    {
        switch (stateEnemy)
        {
            case EnemyState.Attack: Attack(); break;
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
        Debug.Log($"Estado enemigo: {stateEnemy}");
    }

/*___________________________________________________________________________________
|                                                                                    |
|                                                                                    |
|--------------------------------[Colision y Muerte]---------------------------------|
|                                                                                    |
|____________________________________________________________________________________|
*/

    private void Attack()
    {
        if (Time.time < attackRateTime) return;
        player.TakeDamage(damage);
        Debug.Log(player.Health);

        attackRateTime = Time.time + attackRate;
    }


    public override void onEntityDead()
    {     
        stateEnemy=EnemyState.Dead;
        Destroy(gameObject);  
    }
}
