using UnityEngine;


public class Entity : MonoBehaviour
{

    [Header("Stats Player")]

    [Header("Stats Player")]
    [SerializeField] private int health=100;

    [SerializeField] private int maxHealth=100;

    private bool isDead=false;
    public void TakeDamage(int damage)
    {   
        if(isDead) return;

        health = Mathf.Clamp(health, 0, maxHealth);
        health-=damage;

        if (health <= 0)
        {
            isDead=true;
            
        }        
       
    }

    public virtual void OnEntityDead()
    {     
        Destroy(gameObject);
        Debug.Log($"Destruido: {gameObject.name}");    
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