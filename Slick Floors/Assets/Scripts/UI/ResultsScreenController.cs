using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsScreenController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI slicknessText;
    [SerializeField] private TextMeshProUGUI studentsText;

    [Header("Messages")]
    [SerializeField] private string gradeMessage; 
    [SerializeField] private string timeMessage;
    [SerializeField] private string slicknessMessage; 
    [SerializeField] private string studentsMessage;

    [Header("Display Sequence Parameters")]
    [SerializeField] private float timeBetweenTextPopups;
    [SerializeField] private float startDelayTime;
    [SerializeField] private float endDelayTime;

    private void Start()
    {
        hideResultText(); 
        AddResultsToMessages();
        SetMessages();
        StartCoroutine(DisplayResultsSequence());
    }

    public IEnumerator DisplayResultsSequence()
    {
        yield return new WaitForSecondsRealtime(startDelayTime);

        for (int i = 0; i < 4;  i++)
        {
            switch (i)
            {
                case 0:
                    DisplayText(timeText);
                    break;
                case 1:
                    DisplayText(slicknessText);
                    break;
                case 2:
                    DisplayText(studentsText);
                    break;
                case 3:
                    yield return new WaitForSecondsRealtime(timeBetweenTextPopups * 2);
                    DisplayText(gradeText);
                    break;
            }
            yield return new WaitForSecondsRealtime(timeBetweenTextPopups);
        }

        yield return new WaitForSecondsRealtime(endDelayTime);
        endLevel();
    }

    public void DisplayText(TextMeshProUGUI text)
    {
        text.alpha = 1f;
        // possible also play sound effect or visual effect
        // could add arguements to method for audio clip or visual
    }

    private void SetMessages()
    {
        gradeText.text = gradeMessage;
        timeText.text = timeMessage;
        slicknessText.text = slicknessMessage;
        studentsText.text = studentsMessage;
    }

    private bool AddResultsToMessages()
    {
        if (LevelScoreCalculator.Instance == null)
        {
            Debug.LogError("Score Calculator Instance is null. There are no results to pass into UI.");
            return false;
        }
        else if (LevelScoreCalculator.Instance.Results == null)
        {
            Debug.LogError("Score Calculator Instance is null. There are no results to pass into UI.");
            return false;
        }

        LevelResults results = LevelScoreCalculator.Instance.Results;

        gradeMessage += results.grade;
        timeMessage += results.finalTime;
        slicknessMessage += results.tilePercentage;
        studentsMessage += results.studentCount;
        return true;
    }

    public void hideResultText()
    {
        gradeText.alpha = 0f;
        timeText.alpha = 0f;
        slicknessText.alpha = 0f;
        studentsText.alpha = 0f;
    }

    public void endLevel()
    {
        SceneManager.LoadScene("Hub");
    }
}
