using UnityEngine;

[CreateAssetMenu(fileName = "NewScoreWeights", menuName = "ScoreWeights")]
public class LevelScoreWeightsDefinitionSO : ScriptableObject
{
    [Header("Category Point Weights")]
    public float TimeWeigth = 100f;
    public float MopWeight = 100f;
    public float StudentWeight = 100f;
}
