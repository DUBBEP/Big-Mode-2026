using UnityEngine;
using UnityEngine.UI;

public class StudentTracker : MonoBehaviour
{
    [SerializeField] private Image studentIcon;
    [SerializeField] private float iconSpacing = 1f;
    [SerializeField] private float lowOpacity = 0.3f;
    [SerializeField] private float fullOpacity = 1f;

    private float totalStudents;
    private float slippedStudents;
    private Image[] studentIconImages;

    private void Start()
    {
        totalStudents = LevelScoreCalculator.Instance.getStudentTotal();
        Debug.Log($"Total students: {totalStudents}");

        if (studentIcon == null)
        {
            Debug.LogError("StudentTracker: studentIcon is not assigned in inspector!");
            return;
        }

        if (studentIcon.sprite == null)
        {
            Debug.LogError("StudentTracker: studentIcon sprite is not assigned!");
            return;
        }

        Color color = studentIcon.color;
        color.a = lowOpacity;
        studentIcon.color = color;

        CreateStudentIconRow();
    }

    private void FixedUpdate()
    {
        updateCompletion();
    }

    private void CreateStudentIconRow()
    {
        studentIconImages = new Image[(int)totalStudents];

        // Use the existing first icon as template
        studentIconImages[0] = studentIcon;
        studentIcon.transform.localPosition = Vector3.zero;
        studentIcon.gameObject.name = "StudentIcon_0";

        Debug.Log($"Created first icon at position {studentIcon.transform.localPosition}");

        // Duplicate it to the right for remaining students
        for (int i = 1; i < totalStudents; i++)
        {
            GameObject iconObj = Instantiate(studentIcon.gameObject, transform);
            iconObj.name = $"StudentIcon_{i}";
            iconObj.transform.localPosition = new Vector3(i * iconSpacing, 0, 0);

            Image img = iconObj.GetComponent<Image>();
            if (img != null)
            {
                studentIconImages[i] = img;
                Debug.Log($"Created icon {i} at position {iconObj.transform.localPosition}");
            }
            else
            {
                Debug.LogError($"Failed to get Image component on duplicated icon {i}");
            }
        }
    }

    public void updateCompletion()
    {
        slippedStudents = LevelScoreCalculator.Instance.getSlippedStudentTotal();

        for (int i = 0; i < studentIconImages.Length; i++)
        {
            if (i < slippedStudents)
            {
                Debug.Log($"Student {i} has slipped. Setting icon to full opacity.");
                // Student has slipped - show at full opacity
                Color color = studentIconImages[i].color;
                color.a = fullOpacity;
                studentIconImages[i].color = color;
            }
            else
            {
                // Student hasn't slipped - show at low opacity
                Color color = studentIconImages[i].color;
                color.a = lowOpacity;
                studentIconImages[i].color = color;
            }
        }
    }
}
