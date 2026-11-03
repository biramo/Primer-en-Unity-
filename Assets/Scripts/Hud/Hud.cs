using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{   
    //----------------Player----------------
    [SerializeField ]private Image healthBar;
    [SerializeField] private Player player;

    //----------------Weapon----------------
    [SerializeField] private Weapon weapon;
    [SerializeField] private TMP_Text totalAmmoText;
    [SerializeField] private TMP_Text ammoInMagazineText;

    //----------------InfoWave----------------
    [SerializeField] private WaveSpawner waveSpawner;

    [SerializeField] private TMP_Text totalEnemiesText;
    [SerializeField] private TMP_Text actualWaveText;

    [SerializeField] private TMP_Text totalWavesText;

    private void OnEnable()
    {   
        // suscribirse al evento del arma
        // pintar el valor actual una vez, para no esperar al primer disparo
        weapon.AmmoUpdate+=UpdateAmmo;
        UpdateAmmo(weapon.Ammo,weapon.AmmoInMagazine);
        player.HealthTaken+=UpdateHealth;

        if(waveSpawner==null)return;
        waveSpawner.EnemiesAliveChanged+=UpdateEnemiesWave;
        waveSpawner.WaveStarted+=UpdateWaves;



    }

    private void OnDisable()
    {
        // darse de baja
        weapon.AmmoUpdate-=UpdateAmmo;
        player.HealthTaken-=UpdateHealth;
        if(waveSpawner==null)return;

        waveSpawner.EnemiesAliveChanged-=UpdateEnemiesWave;
        waveSpawner.WaveStarted-=UpdateWaves;
    }

    private void UpdateAmmo(int totalAmmo, int ammoInMagazine)
    {
        totalAmmoText.text=totalAmmo.ToString();
        ammoInMagazineText.text=ammoInMagazine.ToString();
    }

    private void UpdateHealth(int health, int maxHealth)
    {
        if(healthBar==null) return;
        healthBar.fillAmount=(float)health/maxHealth;
    }

    private void UpdateEnemiesWave(int enemiesInWave)
    {
        totalEnemiesText.text=enemiesInWave.ToString();

    }

    private void UpdateWaves(int actualWave,int MaxWave)
    {
        actualWaveText.text=actualWave.ToString();
        totalWavesText.text=MaxWave.ToString();
    }

}
