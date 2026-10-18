using UnityEngine;

public class Weapon : MonoBehaviour
{ 
    [SerializeField] private LayerMask EntityMask;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform spawnPoint;
    private int damage=30;
    private float maxDistance=15f;

    private float shotForce=1500;
    private float shotRate=0.5f;
    private float shotRateTime=0;

    //Comprueba si ha pasado el shotRate y dispara la bala  
    public void Shoot()
    {   
        Vector3 origin = cameraTransform.position;
        Vector3 originBullet=spawnPoint.position;
        Vector3 direction = cameraTransform.transform.forward;
        Debug.DrawRay(origin, direction * maxDistance, Color.green, 2f);

        if (Time.time>shotRateTime)//comprueba el tiempo actual
        {   
            
            Vector3 realDirection=onHit(origin,direction);
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

    //Devuelve un Vector3 con el destino de la bala y su origen
    private Vector3 onHit(Vector3 origin, Vector3 direction)
    {    
        Vector3 aimPoint=cameraTransform.position+direction*maxDistance;//Posicion base del apuntado
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
  
}
