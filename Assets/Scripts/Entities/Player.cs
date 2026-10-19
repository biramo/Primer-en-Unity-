using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerController))]
public class Player : Entity
{

    private PlayerController playerController;

    protected void Awake () 
    {
        playerController=GetComponent<PlayerController>();
    }

    void Update()
    {
        CheckIsDead();
    }
    private void CheckIsDead()
    {
        if (IsDead) OnEntityDead();
        
    }  
    public override void OnEntityDead()
    {   
       FindAnyObjectByType<GameManager>().LoadLoseScene();
    }
    
}