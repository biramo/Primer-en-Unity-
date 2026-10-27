using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{   
    [SerializeField ]private Image healthBar;
    [SerializeField] private Player player;
    [SerializeField] private Weapon weapon;
    [SerializeField] private TMP_Text totalAmmoText;

   void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth();
        
    }

     private void OnEnable()
    {   
        // suscribirse al evento del arma
        // pintar el valor actual una vez, para no esperar al primer disparo
        weapon.AmmoUpdate+=UpdateAmmo;
        UpdateAmmo(weapon.Ammo);
    }

    private void OnDisable()
    {
        // darse de baja
        weapon.AmmoUpdate-=UpdateAmmo;

    }

    private void UpdateAmmo(int totalAmmo)
    {
        totalAmmoText.text=totalAmmo.ToString();
    }

    private void UpdateHealth()
    {
        if(healthBar==null) return;
        healthBar.fillAmount=(float)player.Health/player.MaxHealth;
    }
}
