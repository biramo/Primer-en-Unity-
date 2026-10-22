
using UnityEngine;

public class EnemyDistance : Enemy
{
    private Weapon weapon;

    [SerializeField] private GameObject weaponModel;
    [SerializeField] private Transform weaponHolder;//Donde visualmente aparece el arma

    private void Awake()
    {
        // Instanciar el arma como hija del punto de sujeción
        GameObject weaponObject = Instantiate(
            weaponModel,
            weaponHolder
        );

        // Ajustar la posición y rotación local
        weaponObject.transform.localPosition = Vector3.zero;
        weaponObject.transform.localRotation = Quaternion.identity;

        // Obtener el script del arma
        weapon = weaponObject.GetComponent<Weapon>();

        if (weapon == null)
        {
            Debug.LogError(
                "El prefab del arma no tiene el componente Weapon.",
                weaponObject
            );
        }
    }

    protected override void Attack()
    {
        if (weapon == null) return;

        weapon.Shoot();
    }
}
