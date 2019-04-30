using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalWeaponTracking : MonoBehaviour {

    GameObject _target;

	// Update is called once per frame
	void Update () {
        if (_target != null)
        {
            Vector3 lookAtDirection = (_target.transform.position - transform.position).normalized;

            Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAtDirection), 5f * Time.deltaTime);
            transform.localRotation = rot;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, 0);
        }
    }

    public void SetTarget(GameObject target)
    {
        if(target != null)
        {
            _target = target;
        }
    }
}
