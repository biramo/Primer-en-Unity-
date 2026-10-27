using UnityEngine;
using System;


public class Weapon : MonoBehaviour
{ 

    //---------------Mascaras de entidades---------------
    [SerializeField] private LayerMask entityMask;
    [SerializeField] private LayerMask obstacleMask;

    //---------------Posiciones---------------

    [SerializeField] private Transform origin;
    [SerializeField] private Transform spawnPoint;
    
    
    //---------------Prefab bala---------------
    [SerializeField] private GameObject bullet;

    //---------------Stats del arma---------------
    [SerializeField] private int damage=20;
   [SerializeField] private float maxDistance=15f;

    [SerializeField] private int ammo=120;

    [SerializeField] private int ammoInCharger=7;//Cuantas balas caben en el cargador

    [SerializeField] private int maxAmmo=300;
    private float shotForce=1500;
    private float shotRate=0.5f;
    private float shotRateTime=0;

    //Avisa sobre la accion los subscriptores pueden hacer cosas en funcion de eso
    public event Action<int> AmmoUpdate; 

    private bool infiniteAmmo=false;


    private void showAmmoEvent(int ammo)
    {
        AmmoUpdate?.Invoke(ammo);
    }
    public void Shoot()
    {   
        Vector3 rayOrigin = origin.position;
        Vector3 originBullet=spawnPoint.position;
        Vector3 direction = origin.transform.forward;

        bool isAnEntity=Time.time>shotRateTime&&infiniteAmmo;//Comprueba si ha pasada el tiempo y si es una entidad no tiene limite de balas
        bool hasBulletsAndTimePassed=Time.time>shotRateTime&&ammo>0;//Comprueba si ha pasado el shotRate y tiene balas entonces dispara la bala

        if ( hasBulletsAndTimePassed|| isAnEntity) 
        {   
            Debug.DrawRay(rayOrigin, direction * maxDistance, Color.green, 2f);
            Vector3 realDirection=onHit(rayOrigin,direction);
            GameObject newBullet;

            //instanciación de una bala
            //Recibe el objeto,el origen y la rotacion
            newBullet=Instantiate(bullet,originBullet,Quaternion.LookRotation(realDirection));
            //Quaternion.LookRotation nos dala bala orientada a la dirección del disparo
            newBullet.GetComponent<Rigidbody>().AddForce(realDirection*shotForce);


            shotRateTime=Time.time+shotRate; //le suma el rate al tiempo actual limitando el disparo
        
            Destroy(newBullet,2);

            if(!infiniteAmmo) showAmmoEvent(--ammo);
        }
    }

    //Devuelve un Vector3 con la direccion des de el origen(origin) hasta el punto de impacto
    private Vector3 onHit(Vector3 origin, Vector3 direction)
    {    
        Vector3 aimPoint=origin+direction*maxDistance;//Posicion base del apuntado
        //Primero comprobamos que no hemos colisionado con un obstaculo
        if(!Physics.Raycast(origin,direction,maxDistance,obstacleMask)){
        //Devuelve true si colisiona con un objeto con la mascara asignada
            if (Physics.Raycast(origin, direction, out RaycastHit hit,maxDistance,entityMask))
                {       
                    aimPoint=hit.point; //donde a llegado el disparo
                    if (hit.collider.TryGetComponent(out Entity entity))//con hit sabemos quien hemos dado y podemos llamar a metodos suyos
                        {
                            entity.TakeDamage(damage);     
                                    
                        }
                }
            }        
        
        return (aimPoint-spawnPoint.position).normalized;

    }
     
    //GETTERS
    public float MaxDistance
    {
        get{return maxDistance;}
    }

    public int Ammo
    {
        get{return ammo;}
    }

    public bool InfiniteAmmo
    {
        set{infiniteAmmo=value;}
    }
  
}
