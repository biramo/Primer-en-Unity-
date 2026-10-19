using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{   

    [SerializeField ]private Image healthBar;
    [SerializeField] private Player player;

    void Update()
    {   
        if(healthBar==null) return;
        healthBar.fillAmount=(float)player.Health/player.MaxHealth;
    }
    public void LoadLoseScene()
    {
     SceneManager.LoadScene("LoseScene");   
    }

    public void LoadMainScene()
    {
     SceneManager.LoadScene("MainScene");   
    }

    
}
