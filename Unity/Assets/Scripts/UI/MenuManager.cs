using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private UIManager uIManager;
    public bool IsInPause { get; private set; }
    public bool IsInEvaluation { get; private set; }
    private void Start()
    {
        uIManager = GetComponent<UIManager>();
    }
    public void PauseGame() 
    {
        uIManager.ShowPause();
        IsInPause = true;
        Time.timeScale = 0;
    }
    public void UnpauseGame() 
    {
        uIManager.HidePause();
        IsInPause = false;
        Time.timeScale = 1;
    }

    public void OpenEvaluation(int score, int days, string grade) 
    {
        IsInEvaluation = true;
        uIManager.ShowEvaluation(score,days,grade);
    }
    public void CloseEvaluation()
    {
        Time.timeScale = 1;
        IsInEvaluation = false;
        uIManager.HideEvaluation();
    }
    public void LoadGame() 
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    public void Quit() 
    {
        Application.Quit();
    }
}
