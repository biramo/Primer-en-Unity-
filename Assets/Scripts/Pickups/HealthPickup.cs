using UnityEngine;

public class HealthPickup : Pickup
{

    [SerializeField] private int amountHealth=20;
    protected override void collectPickup(Player player)
    {

        if(player.Health>=player.MaxHealth) return;
        player.recibeHealth(amountHealth);

        base.collectPickup(player);
    }
}
