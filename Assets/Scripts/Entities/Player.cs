using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : Entity
{

    private PlayerController playerController;

    private bool deadHandled=false;

    protected void Awake () 
    {
        playerController=GetComponent<PlayerController>();
    }

    void Update()
    {   
        if(deadHandled) return;
        CheckIsDead();
    }
    private void CheckIsDead()
    {
        if (IsDead) OnEntityDead();
      
        
    }  
    public override void OnEntityDead()
    {  
       playerController.enabled=false;//desactivamos al jugador
      
       Rigidbody rb = GetComponent<Rigidbody>();
       rb.linearVelocity = Vector3.zero;
       rb.angularVelocity = Vector3.zero;

       deadHandled=true;
       base.OnEntityDead();

    }
    
}