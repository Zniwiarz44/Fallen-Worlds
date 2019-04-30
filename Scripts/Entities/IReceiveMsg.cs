using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReceiveMsg {

    void OnReceiveMessage(MessageEventArgs msgEventArgs);
}
