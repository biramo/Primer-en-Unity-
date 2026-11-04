using Unity.VisualScripting;
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


//------------------------------Object Variables--------------------------------
        private Collider collider;

        private Animator animator;

//------------------------------WayPoints--------------------------------

        [SerializeField] Transform[] wayPoints;

        private int lastWayPoint;
        private int currentWaypoint;

//------------------------------States/Movements/Attack--------------------------------

        private EnemyState stateEnemy;
        
        [SerializeField]

        private int lastAttack;
     
        private float updatePathRate = 0.2f;
        private float nextPathUpdate=0;
        [SerializeField] private float attackRange=2f;
        private float DistanceToChangeWayPoint=2f;
        private Player player;
        
        //Constructor
        public void Initialize(Transform[] wayPoints)
        {
            this.wayPoints=wayPoints;
        }

        void Start()
        {   
            collider=GetComponentInChildren<Collider>();
            navMeshAgent=GetComponent<NavMeshAgent>();
            player=FindAnyObjectByType<Player>();
            animator=GetComponent<Animator>();
            stateEnemy=EnemyState.Patrol;


            if (player == null||animator==null)//Si no encuentra al player desactivamos los update
            {
                Debug.LogWarning("No se ha encontrado ningun jugador o componente animator",this);
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
            UpdateAnimation();

        }

        private void UpdateAnimation()
        {
            animator.SetInteger(AnimParams.State,(int)stateEnemy);
            
        }
        protected virtual void UpdateState()
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

        protected float CalculateDistanceFromAPoint(Transform point)//Nos devuelve la distancia entre el enemigo y el player
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
            navMeshAgent.isStopped=false;
            navMeshAgent.destination=player.transform.position;
            navMeshAgent.speed=chasingSpeed;
            
        }

        protected void ExecuteState()
        {
            switch (stateEnemy)
            {
                case EnemyState.Attack: 
                    Attack();
                    break;
                case EnemyState.Chasing:Chasing(); 
                    break;
                case EnemyState.Patrol: 
                    navMeshAgent.stoppingDistance=DistanceToChangeWayPoint; 
                    Patrol(); break;
                case EnemyState.Dead:  
                    break;

            }
        }
        protected abstract void Attack();
       

        public void DealDamage()
        {
            if (IsDead || player == null || player.IsDead) return;
            if (CalculateDistanceFromAPoint(player.transform) > attackRange) return;

            player.TakeDamage(damage);
        }
        

       private void Patrol()
       {
            if (wayPoints.Length == 0||wayPoints==null) return;

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
        protected void ChangeState(EnemyState state)
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
            Destroy(gameObject,2);  
        }

    /*___________________________________________________________________________________
    |                                                                                    |
    |                                                                                    |
    |--------------------------------[Getters]---------------------------------|
    |                                                                                    |
    |____________________________________________________________________________________|
    */ 
        public float AttackRange
        {
            get{return attackRange;}
        }

        public float RangeDetection
        {
            get {return rangeDetection;}   
        }

        public Player PlayerTarget
        {
            get{return player;}
        }

        public float NextPathUpdate
        {
            get{return nextPathUpdate;}
            set{nextPathUpdate=value;}
        }

        public EnemyState StateEnemy
        {
            get{return stateEnemy;}
        }

        public float UpdatePathRate
        {
            get{return updatePathRate;}
        } 

        public NavMeshAgent Agent
        {
           get{return navMeshAgent;} 
        }
}
