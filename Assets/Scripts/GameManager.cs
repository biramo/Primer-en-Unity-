using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    [SerializeField] private Player player;

    private int enemiesCount;

    
    void Update()
    {   if(player==null) return;
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
