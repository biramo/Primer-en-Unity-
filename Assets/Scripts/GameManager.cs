using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private Player player;

    private int enemiesCount;

    void Awake()
    {
        enemiesCount=enemies.Count;
    }
    void Update()
    {   if(player==null) return;
        if(enemiesCount<=0 && !player.IsDead) LoadScene("WinScene");
    }

    void OnEnable()
    {
        foreach(Entity e in enemies)//Suscribimos al evento la funcion handleEnemy, para gestionar las muertes
        {
            e.Died+=HandleEnemyDead;
        }
    }

    void OnDisable()//Al eliminar el objeto hay que desuscribirse
    {
        foreach(Entity e in enemies)
        {
            e.Died-=HandleEnemyDead;
        }
    }

    private void HandleEnemyDead(Entity e)
    {
        if(e.IsDead) enemiesCount--;
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
