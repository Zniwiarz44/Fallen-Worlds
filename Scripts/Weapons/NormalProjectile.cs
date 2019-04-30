using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalProjectile : BaseProjectile {

    public float lifespan = 1;
    public GameObject vfxProjectileImpact;

    int _damage;
    float _muzzleVelocity;
    GameObject _launcher;
    GameObject _target;
   // Vector3 _direction;


    public override void FireProjectile(GameObject launcher, GameObject target, int damage, float muzzleVelocity)
    {
        //_direction = (target.transform.position - launcher.transform.position).normalized;
        _launcher = launcher;
        _target = target;
        _damage = damage;
        _muzzleVelocity = muzzleVelocity;
    }


    void FixedUpdate()
    {
        MoveProjectile();
    }

    void MoveProjectile()
    {
        // TODO: Try with rigidbodies again, only one object from 2 colliding requires rigidbody

        //myRigidbody.MovePosition(Vector3.forward * _muzzleVelocity * Time.fixedDeltaTime);
        transform.Translate(Vector3.forward * _muzzleVelocity * Time.fixedDeltaTime);
        //myRigidbody.velocity = transform.TransformDirection(Vector3.forward * _muzzleVelocity);
        if (lifespan <= 0)
        {
            Destroy(gameObject);
        }
        lifespan -= Time.deltaTime;
    }

    void OnCollisionEnter(Collision col)
    {
        Instantiate(vfxProjectileImpact, transform.position, Quaternion.identity);

        IDamagable damagable = col.gameObject.GetComponent<IDamagable>();
        if(damagable != null)
        {
            DamageData dmgData = new DamageData
            {
                Damage = _damage
            };

            MessageHandler messageHandler = col.gameObject.GetComponent<MessageHandler>();
            if(messageHandler)
            {
                MessageEventArgs messageEventArgs = new MessageEventArgs(MessageType.DAMAGED,_launcher,dmgData);
                messageHandler.OnGiveMessage(messageEventArgs);
            }
        }
        Destroy(gameObject);
    }
}
