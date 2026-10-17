using UnityEngine;


public class Entity : MonoBehaviour
{

    [Header("Stats Player")]

    [Header("Stats Player")]
    [SerializeField] private int health=100;
    private bool isDead=false;
    public void TakeDamage(int damage)
    {   
        if(isDead) return;

        health = Mathf.Clamp(health, 0, 100);
        health-=damage;

        if (health <= 0)
        {
            isDead=true;
            onEntityDead();
            
        }        
       
    }

    public virtual void onEntityDead()
    {     
        Destroy(gameObject);
        Debug.Log($"Destruido: {gameObject.name}");    
    }

    //Properties
    public int Health
    {
        get{return health;}
    } 

    public bool IsDead
    {
        get{return isDead;}
    }


}