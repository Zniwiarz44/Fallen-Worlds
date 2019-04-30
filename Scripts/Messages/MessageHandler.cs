using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageHandler : MonoBehaviour
{
    public List<MessageType> messages;
    public List<Action<MessageEventArgs>> _messageDelegates = new List<Action<MessageEventArgs>>();
    public event System.EventHandler<MessageEventArgs> MessageEventHandler;

    internal void RegisterDelegate(Action<MessageEventArgs> onReceiveMessage)
    {
        _messageDelegates.Add(onReceiveMessage);
    }

    public virtual void OnGiveMessage(MessageEventArgs msgEventArgs)
    {
        bool valid = false;
        // Check if this object has the message type, if so we can send out the info, e.g: if weapon deals only shield dmg and current object has no shields 
        // there is no need to inform other components
        for (int i = 0; i < messages.Count; i++)
        {
            if (messages[i] == msgEventArgs.MsgType)
            {
                valid = true;
                break;
            }
        }

        if (!valid)
        {
            return;
        }

        // Send out the message to all subscribers
        for (int i = 0; i < _messageDelegates.Count; i++)
        {
            _messageDelegates[i](msgEventArgs);
        }

        if (MessageEventHandler != null)
        {
            MessageEventHandler(this, msgEventArgs);
            /*MessageEventHandler(this, new MessageEventArgs()
            {
                msgType = msgEventArgs.msgType,
                attackerObject = msgEventArgs.attackerObject,
                msgData = msgEventArgs.msgData
            });*/
        }
    }
}
