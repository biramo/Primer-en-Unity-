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

    public override void onEntityDead()
    {   
       FindAnyObjectByType<GameManager>().LoadLoseScene();
    }
    
}