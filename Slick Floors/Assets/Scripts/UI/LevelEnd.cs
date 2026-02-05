using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelEnd : MonoBehaviour
{
    public TextMeshProUGUI gradeText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI slicknessText;
    public TextMeshProUGUI studentsText;

    private void Start()
    {
        hideResultText();
    }

    public void displayResults(string grade, string time, string slickness, string students)
    {
        gradeText.alpha = 1f;
        timeText.alpha = 1f;
        slicknessText.alpha = 1f;
        studentsText.alpha = 1f;

        gradeText.text = grade;
        timeText.text = time;
        slicknessText.text = slickness;
        studentsText.text = students;
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
