using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    [SerializeField] private Player player;
    [SerializeField] private float loseDelay = 1.5f;
    private bool gameEnded;
    
    void Update()
    {   if(player==null) return;
    }

    private void HandlePlayerDead(Entity e)
    {
        if (gameEnded) return;
        gameEnded = true;
        Invoke(nameof(LoadLoseScene), loseDelay);//Invocamos la pantalla de perdido con un delay
    }
    void OnEnable()
    {   
        player.Died+=HandlePlayerDead;
    }

    void OnDisable()
    {
        player.Died-=HandlePlayerDead;
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void LoadLoseScene()
    {
        SceneManager.LoadScene("LoseScene");   
    }

    public void LoadMainScene()
    {
     SceneManager.LoadScene("MainScene");   
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }
    
}
