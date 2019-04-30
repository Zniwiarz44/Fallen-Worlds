using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{

    public List<AudioClip> audioClips;
    public bool useVolumeRange = true;
    public float volLowRange = 0.8f;
    public float volHighRange = 1f;

    AudioSource source;

    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayCustomSound(int index)
    {
        if ((index < 0) || (index > audioClips.Count) || (audioClips.Capacity == 0))
        {
            Debug.Log("Warning! No sound attached to SFXController");
            return;
        }
        if (!useVolumeRange)
        {
            source.PlayOneShot(audioClips[index]);
        }
        else
        {
            float vol = Random.Range(volLowRange, volHighRange);
            source.PlayOneShot(audioClips[index], vol);
        }
    }
}
