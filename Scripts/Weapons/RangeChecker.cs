using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeChecker : MonoBehaviour
{
    public List<string> tags = new List<string>();                                   // List of tags considered as enemy
    public int maxRange = 20;
    public SphereCollider range;

    List<GameObject> _targets = new List<GameObject>();

    void Start()
    {
        range.radius = maxRange;
        if(tags.Count <= 0)
        {
            Debug.LogWarning("Range checker should have at least one tag");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        bool invalid = true;
        for (int i = 0; i < tags.Count; i++)
        {
            if (other.CompareTag(tags[i]))       // Find out if object entered is the enemy, if it is break so we don't have to loop through rest of the tags
            {
                invalid = false;
                break;
            }
        }

        // If object is not an enemy stop
        if (invalid)
        {
            return;
        }

        _targets.Add(other.gameObject);        // Add to enemy list
    }

    void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < _targets.Count; i++)
        {
            if (other.gameObject == _targets[i])
            {
                _targets.Remove(other.gameObject);
                return;
            }
        }
    }

    public List<GameObject> GetValidTargets()
    {
        for (int i = 0; i < _targets.Count; i++)
        {
            if (_targets[i] == null || Vector3.Distance(_targets[i].gameObject.transform.position, gameObject.transform.position) > maxRange || !_targets[i].activeSelf)
            {
                _targets.RemoveAt(i);
                i--;
            }
        }
        return _targets;
    }

    public bool InRange(GameObject go)
    {
        for (int i = 0; i < _targets.Count; i++)
        {
            if (go == _targets[i])
            {
                return true;
            }
        }
        return false;
    }
}
