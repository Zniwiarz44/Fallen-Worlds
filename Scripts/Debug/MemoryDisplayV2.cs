using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Profiling;

/// <summary>
/// Displays memory used/allocated by the current device
/// </summary>
public class MemoryDisplayV2 : MonoBehaviour {

    public object Profler { get; private set; }
    public event Action<object, string> DisplayHandler;

    // Use this for initialization
    void Start () {
        DisplayHandler += FindObjectOfType<DebugDisplay>().OnDisplayData;
    }
    
    void Update()
    {
        long usedMemory = Profiler.GetTotalAllocatedMemoryLong() / 1048576;
        long allocMemory = Profiler.GetTotalReservedMemoryLong() / 1048576;

        string text = string.Format("Memory Used: {0} MB\nMemory Allocated: {1} MB", usedMemory.ToString(), allocMemory.ToString());

        OnDisplayData(text);
    }


    protected virtual void OnDisplayData(string data)
    {
        DisplayHandler?.Invoke(this, data);
    }
}
