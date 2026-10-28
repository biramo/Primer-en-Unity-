using UnityEngine;

public class EnemyDistance : Enemy
{
    private Weapon weapon;

    [SerializeField] private GameObject weaponModel;
    [SerializeField] private Transform weaponHolder;//Donde visualmente aparece el arma

    [SerializeField] private LayerMask obstacleMask;

    private float aimAtTargetRate = 1.5f;
    private float aimAtTargetUpdate=0;

    private void Awake()
    {
        // Instanciar el arma como hija del punto de sujeción
        GameObject weaponObject = Instantiate(
            weaponModel,
            weaponHolder
        );

        // Ajustar la posición y rotación local
        weaponObject.transform.localPosition = Vector3.zero;
        weaponObject.transform.localRotation = Quaternion.identity;

        // Obtener el script del arma
        weapon = weaponObject.GetComponent<Weapon>();

        weapon.InfiniteAmmo=true;

        if (weapon == null)
        {
            Debug.LogError(
                "El prefab del arma no tiene el componente Weapon.",
                weaponObject
            );
        }
    }

    //Override a la clase padre para comprobar tambien si el enemigo ve al player
    protected override void UpdateState()
    {
        if(StateEnemy==EnemyState.Dead) return;  

            float DistanceFromPlayer=CalculateDistanceFromAPoint(PlayerTarget.transform);
            if(Time.time>=NextPathUpdate){
                if(DistanceFromPlayer<= AttackRange&&CanSeePlayer()){
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
                NextPathUpdate=Time.time+UpdatePathRate;
            }   
    }

   
    private bool CanSeePlayer()//Comprueba con raycast si tiene vision del target
    {   
        Vector3 origin=transform.position;
        Vector3 target=PlayerTarget.transform.position;

        Vector3 direction=target-origin;


        if(Physics.Raycast(origin, direction, out RaycastHit hit,weapon.MaxDistance, obstacleMask)) return false;

        return true; 
    }

    private void AimAtTarget()//Comprueba la direccion del player
    {
        Vector3 direction=PlayerTarget.transform.position - weapon.transform.position;//Calcula la direccion al disparo, desde el origen hasta el player
        weapon.transform.rotation=Quaternion.LookRotation(direction);//Unity convierte ese vector en una rotación que hace que el eje forward del objeto apunte hacia esa dirección
         
    }
    protected override void Attack() 
    {
        if (weapon == null || PlayerTarget==null) return;
        Agent.isStopped=true;

        if (Time.time >= aimAtTargetUpdate)//Cada cierto tiempo comprueba la direccion si no tendria aimbot
        {
            AimAtTarget();
            aimAtTargetUpdate=Time.time+aimAtTargetRate;
        }
        weapon.Shoot();
    }
}
