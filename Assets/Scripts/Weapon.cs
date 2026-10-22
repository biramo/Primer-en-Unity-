using UnityEngine;

public class Weapon : MonoBehaviour
{ 
    [SerializeField] private LayerMask EntityMask;
    [SerializeField] private Transform origin;

    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform directionShoot;

    private int damage=30;
   [SerializeField] private float maxDistance=15f;

    private float shotForce=1500;
    private float shotRate=0.5f;
    private float shotRateTime=0;

    //Comprueba si ha pasado el shotRate y dispara la bala  
    public void Shoot()
    {   
        Vector3 rayOrigin = origin.position;
        Vector3 originBullet=spawnPoint.position;
        Vector3 direction = origin.transform.forward;
        Debug.DrawRay(rayOrigin, direction * maxDistance, Color.green, 2f);

        if (Time.time>shotRateTime)//comprueba el tiempo actual
        {   
            
            Vector3 realDirection=onHit(rayOrigin,direction);
            GameObject newBullet;

            //instanciación de una bala
            //Recibe el objeto,el origen y la rotacion
            newBullet=Instantiate(bullet,originBullet,Quaternion.LookRotation(realDirection));
            //Quaternion.LookRotation nos dala bala orientada a la dirección del disparo
            newBullet.GetComponent<Rigidbody>().AddForce(realDirection*shotForce);


            shotRateTime=Time.time+shotRate; //le suma el rate al tiempo actual limitando el disparo
        
            Destroy(newBullet,2);
        }
    }

    //Devuelve un Vector3 con la direccion des de el origen(origin) hasta el punto de impacto
    private Vector3 onHit(Vector3 origin, Vector3 direction)
    {    
        Vector3 aimPoint=origin+direction*maxDistance;//Posicion base del apuntado
        //Devuelve true si colisiona con un objeto con la mascara asignada
        if (Physics.Raycast(origin, direction, out RaycastHit hit,maxDistance,EntityMask))
            {       
                aimPoint=hit.point; //donde a llegado el disparo
                if (hit.collider.TryGetComponent(out Entity entity))//con hit sabemos quien hemos dado y podemos llamar a metodos suyos
                    {
                        entity.TakeDamage(damage);     
                                
                    }
            }        
        
        return (aimPoint-spawnPoint.position).normalized;

    }
     //GETTERS

     public float MaxDistance
    {
        get{return maxDistance;}
    }
  
}
