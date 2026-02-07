using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomTextWriter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private string reportedText = "L get <wave>Reported.";

    [SerializeField]
    private List<string> randomInitialTexts = new()
    {
        "My <palette> lawyers </palette>shall be hearing about this one<jump>...",
        "Shitty ass Janitor...",
        "Thats it! <!wait=0.2> Im calling ... <!wait=0.35> <palette><!delay=0.3><shake>911"
    };

    [SerializeField]
    private List<string> randomFollowUpTexts = new()
    {
        "<shake> WAIT! </shake> A<wave> <palette> Slick </palette></wave> Sign ???",
        "I guess the <dangle> insurance </dangle> should cover it...",
        "Damn. My fault.",
        "He's <palette>slick </palette>for that one :(",
        "You <palette> slick </palette> motherfucker..."
    };

    private void Start()
    {
        hideAllText();
    }

    public void showTextInitial()
    {
        if (displayText == null || randomInitialTexts.Count == 0) return;

        // pick a one random text to show
        int randomIndex = UnityEngine.Random.Range(0, randomInitialTexts.Count);
        displayText.text = randomInitialTexts[randomIndex];
        displayText.alpha = 1f;
    }

    public void showTextFollowUp()
    {
        if (displayText == null || randomFollowUpTexts.Count == 0) return;

        // pick a one random text to show
        int randomIndex = UnityEngine.Random.Range(0, randomFollowUpTexts.Count);
        displayText.text = randomFollowUpTexts[randomIndex];
        displayText.alpha = 1f;
    }

    public void showReportedText()
    {
        displayText.text = reportedText;
        displayText.alpha = 1f;
    }


    public void hideAllText()
    {
        if (displayText != null)
        {
            displayText.alpha = 0f;
            displayText.text = "";
        }
    }
}
