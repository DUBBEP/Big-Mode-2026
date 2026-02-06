using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LevelScoreCalculator : MonoBehaviour
{
    public static LevelScoreCalculator Instance { get; private set; }

    [SerializeField] private LevelScoreInfoDefinitionSO weights;
    [SerializeField] private float startTimerScore;

    private LevelResults results;
    private float levelTimer;
    private float timerScore;

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
        float tileTotal = TileHandler.TotalTileCount;
        float cleanTitles = TileHandler.CleanCount;

        results = new LevelResults()
        {
            finalTime = levelTimer,
            finalTimeScore = timerScore,
            tileTotal = tileTotal,
            cleanTitles = cleanTitles,
            dirtyTiles = TileHandler.DirtyCount,
            neutralTiles = TileHandler.NeutralCount,
            tilePercentage = cleanTitles / tileTotal,
            studentCount = GetStudentCount(),
        };
    }

    private float GetStudentCount()
    {
        ChildController[] childObjects = FindObjectsByType<ChildController>(FindObjectsSortMode.None);
        float childCount = 0;

        foreach (ChildController obj in childObjects)
        {
            if (obj.GetComponent<Animator>().GetBool("CurlLoop"))
            {
                childCount++;
            }
        }

        return childCount;
    }
}