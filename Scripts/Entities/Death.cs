using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour, IReceiveMsg
{
    // If we don't want to destroy an object simply disable it
    public bool disableObject = false;
    MessageHandler _msgHandler;

    public event System.EventHandler<EntityStatsEventArgs> DeathHandler;

    // Use this for initialization
    void Start()
    {
        _msgHandler = GetComponent<MessageHandler>();

        // MenuController subscribes to know when player died
        DeathHandler += FindObjectOfType<MenuController>().OnEntityDeath;
        if (_msgHandler)
        {
            _msgHandler.RegisterDelegate(OnReceiveMessage);
        }
    }

    public void OnReceiveMessage(MessageEventArgs msgEventArgs)
    {
        switch (msgEventArgs.MsgType)
        {
            case MessageType.DIED:
                DeathData deathData = msgEventArgs.MsgData as DeathData;

                if(deathData != null)
                {
                    UpdateStats(deathData.attacker, deathData.attacked);
                    // Send out info that this entity died
                    OnEntityDeath(deathData.attacked);
                    Die();
                }
                break;
        }
    }
    void UpdateStats(GameObject attacker, GameObject whoDied)
    {
        Debug.Log($"{attacker.gameObject.name} killed {whoDied.gameObject.name}");
    }

    void Die()
    {
        if(disableObject)
        {
            gameObject.SetActive(false);
        }else
        {
            Destroy(gameObject);
        }        
    }

    protected virtual void OnEntityDeath(GameObject whoDied)
    {
        if(DeathHandler != null)
        {
            DeathHandler(this, new EntityStatsEventArgs() { EntityName = whoDied.name });
        }
    }
}
