using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour {

    public abstract void FireProjectile(GameObject launcher, GameObject target, int damage, float muzzleVelocity);
}
