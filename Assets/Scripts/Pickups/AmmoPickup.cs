using UnityEngine;

public class AmmoPickup : Pickup
{
   [SerializeField] private int amountAmoo=50;
    protected override void collectPickup(Player player)
    {

       Weapon weapon=player.GetComponentInChildren<Weapon>();
    
       if(weapon==null)return;

       if(weapon.Ammo>=weapon.MaxAmmo) return;

        weapon.RecibeAmmo(amountAmoo);

        base.collectPickup(player);
    }
}
