using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Displays debug information of all subscribed objects
/// </summary>
public class DebugDisplay : MonoBehaviour {

    private string data;
    private Dictionary<object, string> instanceList = new Dictionary<object, string>();
    public TextAnchor textAnchor;

    public void OnDisplayData(object instanceID, string data)
    {
        if(instanceList.ContainsKey(instanceID))
        {
            // Update value if exists
            instanceList[instanceID] = data;
        }
        else
        {
            // Create new entry
            instanceList.Add(instanceID, data);
        }
        Display();
    }

    private void Display()
    {
        data = "";
        foreach (KeyValuePair<object,string> entry in instanceList)
        {
            data += entry.Value + "\n";
        }
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = textAnchor;
        style.fontSize = h * 2 / 50;
        style.normal.textColor = new Color(0.0f, 0.9f, 0.4f, 1.0f);
        GUI.Label(rect, data, style);
    }
}
