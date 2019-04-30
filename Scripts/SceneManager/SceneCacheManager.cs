using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneCacheManager {
    // TODO: Add lister to check if object has been destroyed
    static List<GameObject> gameObjectsList = new List<GameObject>();

    public static GameObject GetCashedObject(string name)
    {
        return gameObjectsList.Find(x => x.gameObject.name == name);
    }

    public static void AddCashedObject(GameObject go)
    {
        if(go != null)
        {
            gameObjectsList.Add(go);
        }
    }
}
