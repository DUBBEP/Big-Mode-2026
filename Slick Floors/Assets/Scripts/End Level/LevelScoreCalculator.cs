using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelScoreCalculator : MonoBehaviour
{
    public static LevelScoreCalculator Instance { get; private set; }

    [SerializeField] private LevelScoreWeightsDefinitionSO weights;
    [SerializeField] private float startTimerScore = 600;
    [SerializeField] private float timerScorePercentBonus = 20;
    [SerializeField] private float dirtyTilePenaltyWeight = 1;

    private LevelResults results;
    private float levelTimer;
    private float timerScore;

    private float slippedStudent = 0;

    public LevelResults Results { get { return results; } private set { } }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(Instance);
            Instance = this;
        }

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        levelTimer = 0f;
        timerScore = startTimerScore;
    }

    private void Update()
    {
        levelTimer += Time.deltaTime;
        timerScore -= Time.deltaTime;
    }

    public void CalculateFinalScores()
    {
        // set up our percentages
        float tileTotal = TileHandler.TotalTileCount;
        float cleanTitles = TileHandler.CleanCount;
        float dirtyTiles = TileHandler.DirtyCount;
        float tilePercent = cleanTitles / tileTotal * 100;
        float timerScorePercent = timerScore / startTimerScore * 100;
        float slippedStudentPercentage = GetStudentSlipPercent();

        // calculate any modifiers to percentages
        tilePercent = Mathf.Max(0, tilePercent - (dirtyTiles * dirtyTilePenaltyWeight));
        timerScorePercent = Mathf.Min(100, timerScorePercent + timerScorePercentBonus);

        // calculate final grade
        float finalScore = slippedStudentPercentage * weights.StudentWeight +
                           timerScorePercent * weights.TimeWeigth +
                           tilePercent * weights.MopWeight;
        
        // divide down to 100
        finalScore /= 3;

        string grade = GetGrade(finalScore);


        results = new LevelResults()
        {
            finalTime = levelTimer,
            tileTotal = tileTotal,
            cleanTitles = cleanTitles,
            dirtyTiles = TileHandler.DirtyCount,
            neutralTiles = TileHandler.NeutralCount,
            studentCount = slippedStudent,
            grade = grade,
        };
    }

    private float GetStudentSlipPercent()
    {
        ChildController[] childObjects = FindObjectsByType<ChildController>(FindObjectsSortMode.None);
        float slippedStudent = 0;
        float totalStudent = 0;

        foreach (ChildController obj in childObjects)
        {
            totalStudent++;

            if (obj.GetComponent<Animator>().GetBool("CurlLoop"))
                slippedStudent++;
        }

        return slippedStudent / totalStudent;
    }

    private string GetGrade(float finalScore)
    {
        switch (finalScore)
        {
            case >= 95:
                return " A+";
            case >= 90:
                return " A";
            case >= 80:
                return " B";
            case >= 70:
                return " C";
            case >= 60:
                return " D";
            default:
                return " F";
        }
    }
}