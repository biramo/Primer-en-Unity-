using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{   
    [SerializeField ]private Image healthBar;
    [SerializeField] private Player player;
    [SerializeField] private Weapon weapon;
    [SerializeField] private TMP_Text totalAmmoText;
    [SerializeField] private TMP_Text ammoInMagazineText;

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
        UpdateAmmo(weapon.Ammo,weapon.AmmoInMagazine);
    }

    private void OnDisable()
    {
        // darse de baja
        weapon.AmmoUpdate-=UpdateAmmo;

    }

    private void UpdateAmmo(int totalAmmo, int ammoInMagazine)
    {
        totalAmmoText.text=totalAmmo.ToString();
        ammoInMagazineText.text=ammoInMagazine.ToString();
    }

    private void UpdateHealth()
    {
        if(healthBar==null) return;
        healthBar.fillAmount=(float)player.Health/player.MaxHealth;
    }
}
