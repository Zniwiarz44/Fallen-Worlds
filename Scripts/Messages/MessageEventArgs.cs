using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MessageType { DAMAGED, HEALTHCHANGED, DIED };

/// <summary>
/// Message template used by all object.
/// </summary>
public class MessageEventArgs : System.EventArgs
{

    public MessageType MsgType { get; set; }
    public GameObject AttackerObject { get; set; }
    public MessageData MsgData { get; set; }

    public MessageEventArgs(MessageType msgType, GameObject attackerObject, MessageData msgData)
    {
        MsgType = msgType;
        AttackerObject = attackerObject;
        MsgData = msgData;
    }
}

public abstract class MessageData { };

public class DamageData : MessageData
{
    public int Damage { get; set; }
}

public class DeathData : MessageData
{
    public GameObject attacker;        // Who did it
    public GameObject attacked;        // Who died
}

public class HealthData : MessageData
{
    public int maxHealth;
    public int curHealth;
}