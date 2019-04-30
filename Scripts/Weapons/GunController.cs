using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public Transform weaponHold;
    public Weapon defaultWeapon;

    Weapon equippedGun;

	// Use this for initialization
	void Start () {
        if(defaultWeapon != null)
        {
            EquipWeapon(defaultWeapon);
        }		
	}
	
    void EquipWeapon(Weapon gunToEquip)
    {
        if(equippedGun != null)
        {
            Destroy(equippedGun);
        }
        equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation) as Weapon;
        equippedGun.transform.parent = weaponHold;
    }
    public void SetTarget(GameObject target)
    {
        if(equippedGun != null && target != null)
        {
            equippedGun.SetTarget(target);
            // Controls weapon pitch to shot elevated targets
            equippedGun.GetComponent<PersonalWeaponTracking>().SetTarget(target);
        }
    }

    public void Shoot()
    {
        if(equippedGun != null)
        {
            equippedGun.Shoot();
        }
    }
}
