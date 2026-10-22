using UnityEngine;

public class EnemyDistance : Enemy
{
    private Weapon weapon;

    [SerializeField] private GameObject weaponModel;
    [SerializeField] private Transform weaponHolder;//Donde visualmente aparece el arma

    [SerializeField] private LayerMask obstacleMask;

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

        if (weapon == null)
        {
            Debug.LogError(
                "El prefab del arma no tiene el componente Weapon.",
                weaponObject
            );
        }
    }

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

   
    private bool CanSeePlayer()
    {   
        Vector3 origin=transform.position;
        Vector3 target=PlayerTarget.transform.position;

        Vector3 direction=origin-target;

        if(Physics.Raycast(origin, direction, out RaycastHit hit, weapon.MaxDistance, obstacleMask)) return false;

        return true; 
    }
    protected override void Attack()
    {
        if (weapon == null || PlayerTarget==null) return;

        Vector3 direction = PlayerTarget.transform.position - weapon.transform.position;//Calcula la direccion al disparo, desde el origen hasta el player

        transform.rotation=Quaternion.LookRotation(direction);//Unity convierte ese vector en una rotación que hace que el eje forward del objeto apunte hacia esa dirección
        weapon.Shoot();
    }
}
