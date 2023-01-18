using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private MenuManager menuManager;
    

    private void Start()
    {
        menuManager = GetComponent<MenuManager>();
       
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu") 
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                print("I quit");
                menuManager.Quit();
            }
            else if (Input.anyKeyDown)
            {
                menuManager.LoadGame();
            }
        }
        else if (SceneManager.GetActiveScene().name == "Game")
        {
            if (Input.GetKeyDown(KeyCode.Escape) && menuManager.IsInPause == false)
            {
                menuManager.PauseGame();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && menuManager.IsInPause == true)
            {
                menuManager.UnpauseGame();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                menuManager.LoadGame();
            }
        }

    }
}
