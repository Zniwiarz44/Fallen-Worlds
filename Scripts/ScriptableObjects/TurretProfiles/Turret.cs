using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Template for Turret Profiles
[CreateAssetMenu(fileName = "Short Range Turret", menuName = "Turret Profile")]
public class Turret : ScriptableObject {

    // Health
    public int health;
    public int regenRate = 0;
    public bool canRegenerate = false;

    // TurretTrackingV4
    public bool rotatingBarrel = false;
    public float rotationSpeed = 10.0f;
    public float barrelSpeed = 800.0f;
    public float horizontalSpeed = 6f;
    public float verticalSpeed = 6f;
    public float followMultiplyer = 4f;

    // Weapon
    public GameObject projectile;
    public int damage = 10;
    public float fileRate = 400;        // ms between shots;
    public float muzzleVeclocity = 40;
    public float jitterX = 0.3f;
    public float jitterY = 0.1f;
    public float jitterOverTimeIncreaseBy = 0.05f;
    public GameObject VFX_Muzzle;
    public int shootSoundIndex = 0;

    // SFX Controller

    // Range Checker
    public List<string> tags = new List<string>();                                   // List of tags considered as enemy
    public int maxRange = 30;

    // Message Handler
}
