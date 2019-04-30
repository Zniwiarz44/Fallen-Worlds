using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject mainMenu;

    bool activeMenu = false;
    // Use this for initialization
    void Start()
    {
        mainMenu.SetActive(activeMenu);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!activeMenu)
            {
                activeMenu = true;
                mainMenu.SetActive(activeMenu);
            }
            else
            {
                activeMenu = false;
                mainMenu.SetActive(activeMenu);
            }            
        }
    }

    public void OnEntityDeath(object obj, EntityStatsEventArgs entity)
    {
        // Display menu
        Debug.Log(entity.EntityName + " died");
        if(entity.EntityName.Equals("Player"))
        {
            activeMenu = true;
            mainMenu.SetActive(activeMenu);
        }       
    }

    public void Load()
    {

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
