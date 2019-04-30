using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXMuzzleLife : MonoBehaviour {

    public float lifespan = 0.2f;

    // Update is called once per frame
    void Update()
    {
        if (lifespan <= 0)
        {
            Destroy(gameObject);
        }
        lifespan -= Time.deltaTime;
    }
}
