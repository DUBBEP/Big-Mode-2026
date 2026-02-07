using System;
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

    [Header("Audio")]
    [SerializeField] private AudioClip normalTextClip;

    [SerializeField] private AudioClip thudClip;

    private int currentStep = 0;

    private float minimumSkipTimer = 0f;
    private float minimumSkipDuration = 0.5f;

    private void Start()
    {
        hideResultText();
        AddResultsToMessages();
        SetMessages();
        minimumSkipTimer = minimumSkipDuration;
    }

    private void Update()
    {
        if (minimumSkipTimer < minimumSkipDuration)
            minimumSkipTimer += Time.unscaledDeltaTime;
    }

    // Call this method to progress through each result screen step
    public void ProgressScreen()
    {
        if (minimumSkipTimer < minimumSkipDuration)
            return;

        switch (currentStep)
        {
            case 0:
                SoundFXManager.Instance.playSoundFXClip(normalTextClip, transform, volume: 0.8f);
                DisplayText(timeText);
                break;
            case 1:
                SoundFXManager.Instance.playSoundFXClip(normalTextClip, transform, volume: 0.8f);
                DisplayText(slicknessText);
                break;
            case 2:
                SoundFXManager.Instance.playSoundFXClip(normalTextClip, transform, volume: 0.8f);
                DisplayText(studentsText);
                break;
            case 3:
                SoundFXManager.Instance.playSoundFXClip(thudClip, transform, volume: 0.8f);
                DisplayText(gradeText);
                break;
            case 4:
                LoadHub();
                return;
        }

        currentStep++;
        minimumSkipTimer = 0f;
    }

    public void DisplayText(TextMeshProUGUI text)
    {
        text.alpha = 1f;
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

    private void LoadHub()
    {
        SceneManager.LoadScene("Hub");
    }
}
