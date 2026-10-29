using System;
using UnityEngine;


public class Entity : MonoBehaviour
{

    [Header("Stats Entity")]
    [SerializeField] private int health=100;

    [SerializeField] private int maxHealth=100;

    public event Action<Entity> Died; //Avisa sobre la accion los subscriptores pueden hacer cosas en funcion de eso
    private bool isDead=false;
    public void TakeDamage(int damage)
    {   
        if(isDead) return;

        health = Mathf.Clamp(health, 0, maxHealth);
        health-=damage;

        if (health <= 0) isDead=true;
    }

    public void recibeHealth(int hp)
    {
        if(isDead) return;
        int healthNeeded=maxHealth-health;
        int healthToCure=Math.Min(healthNeeded,hp);

        health+=healthToCure;
        
    }

    public virtual void OnEntityDead()
    {     
        Debug.Log($"Destruido: {gameObject.name}");    
        Died?.Invoke(this);
    }

/*___________________________________________________________________________________
|                                                                                    |
|                                                                                    |
|--------------------------------[Properties]----------------------------------------|
|                                                                                    |
|____________________________________________________________________________________|
*/
    public int Health
    {
        get{return health;}
    } 
    public int MaxHealth
    {
        get{return maxHealth;}
    } 

    public bool IsDead
    {
        get{return isDead;}
    }


}