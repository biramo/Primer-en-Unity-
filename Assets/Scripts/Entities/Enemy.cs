using UnityEngine;
using UnityEngine.AI;
    public abstract class Enemy : Entity
    {
//------------------------------Nav Mesh Variables--------------------------------
        private NavMeshAgent navMeshAgent;
        [SerializeField] private float rangeDetection=10f;

        [SerializeField] private int damage=20;
        [SerializeField] private float chasingSpeed = 5f;
        [SerializeField] private float patrolingSpeed = 3f;
        [SerializeField] private float StoppingDistance = 1f;


//------------------------------Object Variables--------------------------------
        private Renderer rdr;
        private Collider collider;

//------------------------------WayPoints--------------------------------

        [SerializeField] Transform[] wayPoints;

        private int lastWayPoint;
        private int currentWaypoint;

//------------------------------States/Movements/Attack--------------------------------

        private EnemyState stateEnemy;
        
        [SerializeField] private float attackRate=1f;
        private float attackRateTime=0;
        private float updatePathRate = 0.2f;
        private float nextPathUpdate=0;
        [SerializeField] private float attackRange=2f;
        private float DistanceToChangeWayPoint=2f;
        private Player player;

        void Start()
        {   
            collider=GetComponentInChildren<Collider>();
            rdr=GetComponentInChildren<Renderer>();
            navMeshAgent=GetComponent<NavMeshAgent>();
            player=FindAnyObjectByType<Player>();
            navMeshAgent.stoppingDistance=StoppingDistance;
            stateEnemy=EnemyState.Patrol;


            if (player == null)//Si no encuentra al player desactivamos los update
            {
                Debug.LogWarning("No se ha encontrado ningun jugador",this);
                enabled = false;
            }
            navMeshAgent.speed=chasingSpeed;
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
            CheckIsDead();
            UpdateState();
            ExecuteState();
        }
        private void UpdateState()
        {   

            if(stateEnemy==EnemyState.Dead) return;  

            float DistanceFromPlayer=CalculateDistanceFromAPoint(player.transform);
            if(Time.time>=nextPathUpdate){
                if(DistanceFromPlayer<= AttackRange){
                    ChangeState(EnemyState.Attack);                              
                }
                else if (DistanceFromPlayer<=RangeDetection)
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

        public float CalculateDistanceFromAPoint(Transform point)//Nos devuelve la distancia entre el enemigo y el player
        {
            return Vector3.Distance(transform.position,point.position);

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
            navMeshAgent.speed=chasingSpeed;
            
        }

        public void ExecuteState()
        {
            switch (stateEnemy)
            {
                case EnemyState.Attack: Attack(); break;
                case EnemyState.Chasing: Chasing(); break;
                case EnemyState.Patrol: Patrol(); break;
                case EnemyState.Dead:  break;

            }
        }
        protected virtual void Attack()
        {
            if (Time.time < attackRateTime) return;
            player.TakeDamage(damage);

            attackRateTime = Time.time + attackRate;
        }
        

       private void Patrol()
       {
            if (wayPoints.Length == 0) return;

            bool hasArrived = !navMeshAgent.pathPending
                            && navMeshAgent.remainingDistance <= DistanceToChangeWayPoint;

            if (!hasArrived) return;
            
            navMeshAgent.speed=patrolingSpeed;
            currentWaypoint = Random.Range(0,wayPoints.Length);

            if(wayPoints.Length>1){
                while (currentWaypoint == lastWayPoint)
                {
                    currentWaypoint=Random.Range(0,wayPoints.Length);
                }
            }
                navMeshAgent.destination = wayPoints[currentWaypoint].position;

                lastWayPoint=currentWaypoint;

       }

        //Cambiar el estado actual
        private void ChangeState(EnemyState state)
        {
            if(stateEnemy==state) return;

            stateEnemy=state;

            if(state==EnemyState.Dead) OnEntityDead();
        }

    /*___________________________________________________________________________________
    |                                                                                    |
    |                                                                                    |
    |--------------------------------[Muerte]---------------------------------|
    |                                                                                    |
    |____________________________________________________________________________________|
    */  
        private void CheckIsDead()
        {
            if (IsDead) ChangeState(EnemyState.Dead);
            
        }  
        public override void OnEntityDead()
        {   
            base.OnEntityDead();
            navMeshAgent.isStopped=true;
            //Desactivamos el collider y le cambiamos el color
            collider.enabled=false;
            rdr.material.color=Color.red;
            Destroy(gameObject,2);  
        }

    /*___________________________________________________________________________________
    |                                                                                    |
    |                                                                                    |
    |--------------------------------[Muerte]---------------------------------|
    |                                                                                    |
    |____________________________________________________________________________________|
    */ 
        private float AttackRange
        {
            get{return attackRange;}
        }

        private float RangeDetection
        {
            get {return rangeDetection;}   
        }
}
