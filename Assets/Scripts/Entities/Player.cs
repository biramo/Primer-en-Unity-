using UnityEngine;
using UnityEngine.SceneManagement;

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
        deadHandled=true;
        
    }  
    public override void OnEntityDead()
    {   
       FindAnyObjectByType<GameManager>().LoadLoseScene();
    }
    
}