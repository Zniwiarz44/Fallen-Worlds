using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCamera : MonoBehaviour
{
    GameObject player;

    private Vector3 offset;
    private Vector3 temp;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        transform.position = new Vector3(-8.91f, 7.35f, 30f);
        //transform.rotation = new Quaternion(42.91f, 134.29f, 9.39f, 0f);
        transform.rotation.Set(42.91f, 134.29f, 9.39f, 0f);
        offset = transform.position - player.transform.position;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
             // transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y + 1f, transform.position.z + 2f);
            temp = transform.position;
            transform.Translate(0.5f, 1f, 1f);
        }
        if (!player) { return; }
        transform.position = player.transform.position + offset + temp;
    }
}
