using System;
using UnityEngine;

public class ScoreCalculatorDummy : LevelScoreCalculator
{
    private void Start()
    {
        results = MakeDummyresults();
    }

    private LevelResults MakeDummyresults()
    {
        float timePercent = UnityEngine.Random.Range(50f, 100f);
        float tilePercent = Mathf.Round(UnityEngine.Random.Range(20f, 100f));
        float studentPercent = UnityEngine.Random.Range(50f, 100f);

        TimeSpan time = TimeSpan.FromSeconds(UnityEngine.Random.Range(60f, 1000f));
        string finalTime = time.ToString(@"mm\:ss\:ff");
        string students = UnityEngine.Random.Range(0, 6).ToString();

        float finalScore = studentPercent / 100 * weights.StudentWeight +
                   timePercent / 100 * weights.TimeWeigth +
                   tilePercent / 100 * weights.MopWeight;

        finalScore /= 3;

        Debug.Log($"time score: {timePercent}");
        Debug.Log($"tile score: {tilePercent}");
        Debug.Log($"student score: {studentPercent}");
        Debug.Log($"final score: {finalScore}");

        return new LevelResults()
        {
            finalTime = GetFinalString(timePercent, finalTime),
            tilePercentage = GetFinalString(tilePercent, tilePercent.ToString() + '%'),
            studentCount = GetFinalString(studentPercent, students),
            grade = GetGrade(finalScore),
        };
    }
}
