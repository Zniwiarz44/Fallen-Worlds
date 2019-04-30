using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(HealthUI))]
[RequireComponent(typeof(Death))]
[RequireComponent(typeof(TurretAI))]            // Class it self requires RangeChecker, TurretTrackingV3, Weapon
[RequireComponent(typeof(SFXController))]
[RequireComponent(typeof(MessageHandler))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ClickDetection))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BoxCollider))]

public class TurretInit : MonoBehaviour
{
    public Turret turretProfile;

    //public List<GameObject> projectileSpawns;

    // Use this for initialization
    void Awake()
    {
        Health health = GetComponent<Health>();
        health.maxHealth = turretProfile.health;
        health.canRegenerate = turretProfile.canRegenerate;
        health.regenRate = turretProfile.regenRate;

        /*HealthUI healthUI = GetComponent<HealthUI>();
        healthUI.slider = slider;*/

        TurretTrackingV4 trackingV3 = GetComponent<TurretTrackingV4>();
        trackingV3.rotatingBarrel = turretProfile.rotatingBarrel;
        trackingV3.rotationSpeed = turretProfile.rotationSpeed;
        trackingV3.barrelSpeed = turretProfile.barrelSpeed;
        trackingV3.horizontalSpeed = turretProfile.horizontalSpeed;
        trackingV3.verticalSpeed = turretProfile.verticalSpeed;
        trackingV3.followMultiplyer = turretProfile.followMultiplyer;

        Weapon weapon = GetComponent<Weapon>();
        //weapon.projectileSpawns = projectileSpawns;
        weapon.projectile = turretProfile.projectile;
        weapon.damage = turretProfile.damage;
        weapon.fileRate = turretProfile.fileRate;
        weapon.muzzleVeclocity = turretProfile.muzzleVeclocity;
        weapon.jitterX = turretProfile.jitterX;
        weapon.jitterY = turretProfile.jitterY;
        weapon.jitterOverTimeIncreaseBy = turretProfile.jitterOverTimeIncreaseBy;
        weapon.VFX_Muzzle = turretProfile.VFX_Muzzle;
        weapon.shootSoundIndex = turretProfile.shootSoundIndex;

        RangeChecker rangeChecker = GetComponent<RangeChecker>();
        rangeChecker.tags = turretProfile.tags;
        rangeChecker.maxRange = turretProfile.maxRange;
        //rangeChecker.range = range;
    }
}
