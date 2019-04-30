using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SFXController))]
[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour {

    public List<GameObject> projectileSpawns;

    public GameObject projectile;
    public int damage = 10;
    public float fileRate = 400;        // ms between shots;
    public float muzzleVeclocity = 40;
    public float jitterX = 0.3f;
    public float jitterY = 0.1f;
    public float jitterOverTimeIncreaseBy = 0.05f;
    public GameObject VFX_Muzzle;
    public int shootSoundIndex = 0;

    float nextShotTime;
    GameObject _target;
    SFXController sfxAudio;

    void Start()
    {
        sfxAudio = GetComponent<SFXController>();
    }

    public void Shoot()
    {
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + fileRate / 1000; // From ms to s

            for (int i = 0; i < projectileSpawns.Count; i++)
            {
                if (projectileSpawns[i])
                {
                    // Spawn the projectile
                    GameObject newPro = Instantiate(projectile, projectileSpawns[i].transform.position, projectileSpawns[i].transform.rotation) as GameObject;
                    // Adds jitter on X and Y axis
                    //newPro.transform.Rotate(Random.Range(-jitterY, jitterY), Random.Range(-jitterX, jitterX), 0);
                    newPro.GetComponent<BaseProjectile>().FireProjectile(projectileSpawns[i], _target, damage, muzzleVeclocity);

                    // Instantiate the muzzle effect
                    Instantiate(VFX_Muzzle, projectileSpawns[i].transform.position, projectileSpawns[i].transform.rotation);
                }
            }
            if(projectileSpawns.Count > 0)
            {
                sfxAudio.PlayCustomSound(shootSoundIndex);
            }
        }
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }
}
