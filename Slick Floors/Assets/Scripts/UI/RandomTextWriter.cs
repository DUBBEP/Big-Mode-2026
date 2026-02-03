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
            randomInitialTexts[i].gameObject.SetActive(i == randomIndex);
            Debug.Log("Showing text: " + randomInitialTexts[i].text);
        }
    }

    public void showTextFollowUp()
    {
        // pick a one random text mesh to show
        int randomIndex = UnityEngine.Random.Range(0, randomFollowUpTexts.Count);
        for (int i = 0; i < randomFollowUpTexts.Count; i++)
        {
            randomFollowUpTexts[i].gameObject.SetActive(i == randomIndex);
            Debug.Log("Showing text: " + randomFollowUpTexts[i].text);
        }
    }

    public void showSignText()
    {
        aSignText.gameObject.SetActive(true);
    }

    public void showReportedText()
    {
        reportedText.gameObject.SetActive(true);
    }


    public void hideAllText()
    {
        aSignText.gameObject.SetActive(false);
        foreach (var textMesh in randomInitialTexts)
        {
            textMesh.gameObject.SetActive(false);
        }
        foreach (var textMesh in randomFollowUpTexts)
        {
            textMesh.gameObject.SetActive(false);
        }
    }
}
