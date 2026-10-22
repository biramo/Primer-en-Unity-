using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{   

    [SerializeField ]private Image healthBar;
    [SerializeField] private Player player;

    [SerializeField] private List<Enemy> enemies;

    private int enemiesCount;

    void Awake()
    {
        enemiesCount=enemies.Count;
    }
    void Update()
    {   
        if(healthBar==null) return;
        healthBar.fillAmount=(float)player.Health/player.MaxHealth;
        if(enemiesCount<=0) LoadScene("WinScene");
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
