using System;
using UnityEngine;

public class LevelScoreCalculator : MonoBehaviour
{
    public static LevelScoreCalculator Instance { get; private set; }

    [SerializeField] protected LevelScoreWeightsDefinitionSO weights;
    [SerializeField] private float startTimerScore = 600;
    [SerializeField] private float timerScorePercentBonus = 20;
    [Tooltip("How much percent each dirty tile takes off the score")]
    [SerializeField] private float dirtyTilePenaltyWeight = 1;

    protected LevelResults results = null;
    private float levelTimer;
    private float timerScore;

    private int slippedStudents = 0;

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
        if (results != null) return;
        
        levelTimer = 0f;
        timerScore = startTimerScore;
    }

    private void Update()
    {
        if (results != null) return;

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
        float finalScore = slippedStudentPercentage / 100 * weights.StudentWeight +
                           timerScorePercent / 100 * weights.TimeWeigth +
                           tilePercent / 100 * weights.MopWeight;
        
        // divide down to 100
        finalScore /= 3;

        string grade = GetGrade(finalScore);


        TimeSpan time = TimeSpan.FromSeconds(levelTimer);

        results = new LevelResults()
        {
            finalTime = GetFinalString(timerScorePercent, time.ToString(@"mm\:ss\:ff")),
            tilePercentage = GetFinalString(tilePercent, tilePercent.ToString() + '%'),
            studentCount = GetFinalString(slippedStudentPercentage, slippedStudents.ToString()),
            grade = grade,
        };
    }

    private float GetStudentSlipPercent()
    {
        ChildController[] childObjects = FindObjectsByType<ChildController>(FindObjectsSortMode.None);
        slippedStudents = 0;
        float totalStudent = 0;

        foreach (ChildController obj in childObjects)
        {
            totalStudent++;

            if (obj.GetComponent<Animator>().GetBool("CurlLoop"))
                slippedStudents++;
        }

        return slippedStudents / totalStudent;
    }

    protected string GetGrade(float finalScore)
    {
        switch (finalScore)
        {
            case >= 95:
                return "<palette> A+";
            case >= 90:
                return "<color=#FFD700> A";
            case >= 80:
                return "<color=#C0C0C0> B";
            case >= 70:
                return "<color=#CD7F32> C";
            case >= 60:
                return " D";
            default:
                return "<color=red> F";
        }
    }

    protected string GetFinalString(float percentValue, string finalvalue)
    {
        switch (percentValue)
        {
            case >= 95:
                return $"<palette> {finalvalue}";
            case >= 90:
                return $"<color=#FFD700> {finalvalue}";
            case >= 80:
                return $"<color=#909090> {finalvalue}";
            case >= 70:
                return $"<color=#CD7F32> {finalvalue}";
            case >= 60:
                return $" {finalvalue}";
            default:
                return $"<color=red> {finalvalue}";
        }
    }


}