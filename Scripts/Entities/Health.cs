using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IReceiveMsg, IDamagable
{
    public int maxHealth = 500;
    public bool canRegenerate = false;
    public int regenRate = 4;  // HP per second
    int _curHealth;
    float nextRegenRate = 1;    // Seconds
    MessageHandler _msgHandler;

    // Use this for initialization
    void Start()
    {
        _curHealth = maxHealth;
        _msgHandler = GetComponent<MessageHandler>();

        if (_msgHandler)
        {
            _msgHandler.RegisterDelegate(OnReceiveMessage);
        }
    }

    void Update()
    {
        // Regenare health over time
        if(canRegenerate)
        {
            if (Time.time > nextRegenRate)
            {
                nextRegenRate = Time.time + 1;
                _curHealth += regenRate;
                _curHealth = Mathf.Clamp(_curHealth, 0, maxHealth);

                if (_msgHandler)
                {
                    HealthData healthData = new HealthData
                    {
                        maxHealth = maxHealth,
                        curHealth = _curHealth
                    };

                    MessageEventArgs messageEventArgs = new MessageEventArgs(MessageType.HEALTHCHANGED, gameObject, healthData);
                    _msgHandler.OnGiveMessage(messageEventArgs);
                }
            }
        }
    }
    public void OnReceiveMessage(MessageEventArgs msgEventArgs)
    {
        switch(msgEventArgs.MsgType)
        {
            case MessageType.DAMAGED:
                DamageData dmg = msgEventArgs.MsgData as DamageData;

                if(dmg != null)
                {
                    DoDamage(dmg.Damage, msgEventArgs.AttackerObject);
                }
                break;
        }
    }

    void DoDamage(int dmg, GameObject attacker)
    {
        _curHealth -= dmg;

        if(_curHealth <= 0)
        {
            _curHealth = 0;

            if(_msgHandler)
            {
                DeathData deathData = new DeathData();
                deathData.attacker = attacker;
                deathData.attacked = gameObject;

                MessageEventArgs messageEventArgs = new MessageEventArgs(MessageType.DIED, attacker, deathData);
                _msgHandler.OnGiveMessage(messageEventArgs);
            }
        }

        if(_msgHandler)
        {
            HealthData healthData = new HealthData
            {
                maxHealth = maxHealth,
                curHealth = _curHealth
            };

            MessageEventArgs messageEventArgs = new MessageEventArgs(MessageType.HEALTHCHANGED, gameObject, healthData);
            _msgHandler.OnGiveMessage(messageEventArgs);
        }
    }
}
