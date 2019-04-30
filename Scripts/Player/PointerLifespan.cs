using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerLifespan : MonoBehaviour {

    public float minDistanceFromPointer = 0.5f;
    GameObject player;

	// Use this for initialization
	void Start () {
        player = SceneCacheManager.GetCashedObject("Player");
	}
	
	// Update is called once per frame
	void Update () {
        // Check for null if player is destroyed
        if(player == null) { return; }
        if (Vector3.Distance(player.transform.position, gameObject.transform.position) <= minDistanceFromPointer)
        {
            Destroy(gameObject);
        }
	}
}
