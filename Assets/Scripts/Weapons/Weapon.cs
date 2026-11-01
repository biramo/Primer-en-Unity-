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

    //---------------[Stats del arma]---------------
    [SerializeField] private int damage=20;
   [SerializeField] private float maxDistance=15f;

    //---------------[Stats de la municion]---------------

    [SerializeField] private int ammo=120;

    [SerializeField] private int maxAmmoInCharger=7;//Cuantas balas caben en el cargador
    [SerializeField] private int ammoInMagazine = 7; //Cuantas tiene actualmente

    [SerializeField] private int maxAmmo=300;

    //---------------[Stats del disparo]---------------

    private float shotForce=300;
    private float shotRate=0.5f;
    private float shotRateTime=0;

    //Avisa sobre la accion los subscriptores pueden hacer cosas en funcion de eso
    public event Action<int,int> AmmoUpdate; //Pasamos la municion total y la que tiene en el cargador

    private bool infiniteAmmo=false;


    private void ShowAmmoEvent(int ammo, int ammoInMagazine)
    {
        AmmoUpdate?.Invoke(ammo,ammoInMagazine);
    }

    public void Recharge()
    {
        if(ammoInMagazine>=maxAmmoInCharger || ammo<=0) return;

        int ammoNeeded = maxAmmoInCharger - ammoInMagazine;
        int ammoToLoad = Math.Min(ammoNeeded, ammo);//Coge el valor mas pequeño, si hay pocas balas las de ammo si no las que necesita

        ammoInMagazine += ammoToLoad;
        ammo -= ammoToLoad;

        ShowAmmoEvent(ammo,ammoInMagazine);
    }

    public void RecibeAmmo(int munition)
    {
        int ammoNeeded=maxAmmo-ammo;
        int ammoToLoad=Math.Min(ammoNeeded,munition);

        ammo+=ammoToLoad;

        ShowAmmoEvent(ammo,ammoInMagazine);
    }
    public void Shoot()
    {   
        Vector3 rayOrigin = origin.position;
        Vector3 originBullet=spawnPoint.position;
        Vector3 direction = origin.transform.forward;

        bool isAnEntity=Time.time>shotRateTime&&infiniteAmmo;//Comprueba si ha pasada el tiempo y si es una entidad no tiene limite de balas
        bool hasBulletsAndTimePassed=Time.time>shotRateTime&&ammoInMagazine>0;//Comprueba si ha pasado el shotRate y tiene balas entonces en el cargador dispara la bala

        if ( hasBulletsAndTimePassed|| isAnEntity) 
        {   
            Debug.DrawRay(rayOrigin, direction * maxDistance, Color.green, 2f);
            Vector3 realDirection=onHit(rayOrigin,direction);
            GameObject newBullet;

            //instanciación de una bala
            //Recibe el objeto,el origen y la rotacion
            newBullet=Instantiate(bullet,originBullet,Quaternion.LookRotation(realDirection)*bullet.transform.rotation);
            //Quaternion.LookRotation nos dala bala orientada a la dirección del disparo
            newBullet.GetComponent<Rigidbody>().AddForce(realDirection*shotForce);


            shotRateTime=Time.time+shotRate; //le suma el rate al tiempo actual limitando el disparo
        
            Destroy(newBullet,2);

            if(!infiniteAmmo) ShowAmmoEvent(ammo,--ammoInMagazine);
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

    public int MaxAmmo
    {
        get{return maxAmmo;}
    }


    public int AmmoInMagazine
    {
        get{return ammoInMagazine;}
    }

    public bool InfiniteAmmo
    {
        set{infiniteAmmo=value;}
    }
  
}
