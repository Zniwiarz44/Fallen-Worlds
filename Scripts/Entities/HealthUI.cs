using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour, IReceiveMsg {

    public Slider slider;
    MessageHandler _msgHandler;

    // Use this for initialization
    void Start () {
        _msgHandler = GetComponent<MessageHandler>();

        if (_msgHandler)
        {
            _msgHandler.RegisterDelegate(OnReceiveMessage);
        }
    }

    public void OnReceiveMessage(MessageEventArgs msgEventArgs)
    {
        switch (msgEventArgs.MsgType)
        {
            case MessageType.HEALTHCHANGED:
                HealthData health = msgEventArgs.MsgData as HealthData;

                if (health != null)
                {
                    UpdateUI(health.maxHealth, health.curHealth);
                }
                break;
        }
    }

    void UpdateUI(int maxHealth, int curHealth)
    {
        // Slider works in 0-1
        slider.value = (1.0f / maxHealth) * curHealth;  // Get percentage of current health
    }
}
