using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomTextWriter : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> randomInitialTexts;
    [SerializeField] private List<TextMeshProUGUI> randomFollowUpTexts;
    [SerializeField] private TextMeshProUGUI aSignText;
    [SerializeField] private TextMeshProUGUI reportedText;

    public void showTextInitial()
    {
        // pick a one random text mesh to show
        int randomIndex = UnityEngine.Random.Range(0, randomInitialTexts.Count);
        for (int i = 0; i < randomInitialTexts.Count; i++)
        {
            randomInitialTexts[i].alpha = (i == randomIndex) ? 1f : 0f;
        }
    }

    public void showTextFollowUp()
    {
        // pick a one random text mesh to show
        int randomIndex = UnityEngine.Random.Range(0, randomFollowUpTexts.Count);
        for (int i = 0; i < randomFollowUpTexts.Count; i++)
        {
            randomFollowUpTexts[i].alpha = (i == randomIndex) ? 1f : 0f;
        }
    }

    public void showSignText()
    {
        aSignText.alpha = 1f;
    }

    public void showReportedText()
    {
        reportedText.alpha = 1f;
    }


    public void hideAllText()
    {
        aSignText.alpha = 0f;
        reportedText.alpha = 0f;
        foreach (var textMesh in randomInitialTexts)
        {
            textMesh.alpha = 0f;
        }
        foreach (var textMesh in randomFollowUpTexts)
        {
            textMesh.alpha = 0f;
        }
    }

    private void Start()
    {
        hideAllText();
    }
}
