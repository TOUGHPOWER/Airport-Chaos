using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseUI;
    [SerializeField]
    private GameObject evaluationUI;
    [SerializeField]
    private GameObject popPrefab;
    [SerializeField]
    private List<GameObject> strikes;
    [SerializeField]
    private TextMeshProUGUI clockText;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI daysText;
    [SerializeField]
    private TextMeshProUGUI gradeText;


    [SerializeField] private Transform scrollArea;
    [field: SerializeField] public List<GameObject> Messages { get; private set; }

   public GameObject CreatePopUp(string message) 
    {
        popPrefab.GetComponentInChildren<TextMeshProUGUI>().text = message;
        GameObject popUp = Instantiate(popPrefab,scrollArea);
        Messages.Add(popUp);
        return popUp;
    }
    public void RemovePopUp(GameObject popUp) 
    {
        Messages.Remove(popUp);
        Destroy(popUp);
    }
    public void IncreaseStrikes(int currentStrike) 
    {
        strikes[currentStrike].SetActive(true);
    }
    public void ResetStrikes() 
    {
        foreach (GameObject strike in strikes)
        {
            strike.SetActive(false);
        }
    }
    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }
    public void UpdateClock(float clock)
    {
        clockText.text = clock.ToString();
    }
    public void UpdateDays(int numDays)
    {
        daysText.text = numDays.ToString();
    }
    public void UpdateGrade(string gradeLetter) 
    {
        gradeText.text = gradeLetter;
    }
    public void ShowPause() 
    {
        pauseUI.SetActive(true);
    }
    public void HidePause()
    {
        pauseUI.SetActive(false);
    }
    public void ShowEvaluation(int score, int days, string grade)
    {
        UpdateDays(days);
        UpdateScore(score);
        UpdateGrade(grade);
        evaluationUI.SetActive(true);
    }
    public void HideEvaluation()
    {
        evaluationUI.SetActive(false);
    }
    
}