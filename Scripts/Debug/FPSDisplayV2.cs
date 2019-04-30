using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Calculates FPS
/// </summary>
public class FPSDisplayV2 : MonoBehaviour {

	float deltaTime = 0.0f;
    public event Action<object, string> DisplayHandler;

    void Start()
    {
        DisplayHandler += FindObjectOfType<DebugDisplay>().OnDisplayData;
    }

	void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);

        OnDisplayData(text);
    }

    protected virtual void OnDisplayData(string data)
    {
        DisplayHandler?.Invoke(this, data);
    }
}

