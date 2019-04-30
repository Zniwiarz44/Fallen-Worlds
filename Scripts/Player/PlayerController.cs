using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerMovementV2 player;
    GunController gunController;
    Weapon weapon;

    GameObject _curTarget = null;
    // Use this for initialization
    void Start()
    {
        player = GetComponent<PlayerMovementV2>();
        gunController = GetComponent<GunController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.InCombat)
        {
            if(player.CurrentTarget != _curTarget)
            {               
                _curTarget = player.CurrentTarget;
                gunController.SetTarget(player.CurrentTarget);
            }            
            gunController.Shoot();
        }
        else if(_curTarget != null)
        {
            _curTarget = null;
        }
    }
}
