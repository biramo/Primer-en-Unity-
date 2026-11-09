using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    [SerializeField] private Player player;
    [SerializeField] private WaveSpawner spawner;

    [SerializeField] private float endDelay = 0f;
    private bool gameEnded;
    private void HandlePlayerDead(Entity e)
    {
        if (gameEnded) return;
        spawner.StopWaves();
        gameEnded = true;
        Invoke(nameof(LoadLoseScene), endDelay);//Invocamos la pantalla de perdido con un delay
    }

    void OnEnable()
    {   
        if(player==null||spawner==null)return;
        player.Died+=HandlePlayerDead;
        spawner.WavesCompleted+=LoadWinScene;
    }

    

    void OnDisable()
    {   
        player.Died-=HandlePlayerDead;
        spawner.WavesCompleted-=LoadWinScene;
    }

    public void LoadWinScene()
    {
        SceneManager.LoadScene("WinScene");
    }
    public void LoadLoseScene()
    {
        SceneManager.LoadScene("LoseScene");   
    }

    public void LoadMainScene()
    {
     SceneManager.LoadScene("Level01");   
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
