using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public AudioClip clip;

    AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void LaunchDemo()
    {
        source.PlayOneShot(clip);
        
        SceneManager.LoadScene("DemoLevel");
    }
}
