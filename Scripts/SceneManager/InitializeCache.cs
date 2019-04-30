using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeCache : MonoBehaviour {

    public List<string> ObjectToInitialize = new List<string>();
    // Use this for initialization
    void Start()
    {
        foreach (string item in ObjectToInitialize)
        {
            SceneCacheManager.AddCashedObject(GameObject.Find(item));
        }
    }    
}
