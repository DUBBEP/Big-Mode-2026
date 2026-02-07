using UnityEngine;
using UnityEngine.UI;

public class StudentTracker : MonoBehaviour
{
    [SerializeField] private Image studentIcon;
    [SerializeField] private StudentSlippedEventSO studentSlippedEvent;
    [SerializeField] private float iconSpacing = 1f;
    [SerializeField] private float lowOpacity = 0.3f;
    [SerializeField] private float fullOpacity = 1f;

    private int totalStudents;
    private int slippedStudents = 0;  // Track our own counter
    private Image[] studentIconImages;

    private void OnEnable()
    {
        if (studentSlippedEvent != null)
        {
            studentSlippedEvent.RegisterListener(OnStudentSlipped);
            Debug.Log("StudentTracker: Registered listener for student slipped event");
        }
        else
        {
            Debug.LogError("StudentTracker: studentSlippedEvent is NULL! Cannot register listener.");
        }
    }

    private void OnDisable()
    {
        if (studentSlippedEvent != null)
        {
            studentSlippedEvent.UnregisterListener(OnStudentSlipped);
            Debug.Log("StudentTracker: Unregistered listener for student slipped event");
        }
    }

    private void Start()
    {
        // Wait for LevelScoreCalculator to initialize
        if (LevelScoreCalculator.Instance == null)
        {
            Debug.LogError("StudentTracker: LevelScoreCalculator.Instance is null!");
            return;
        }

        totalStudents = LevelScoreCalculator.Instance.TotalStudents;
        Debug.Log($"Total students: {totalStudents}");

        if (totalStudents == 0)
        {
            Debug.LogWarning("StudentTracker: No students found in level. Skipping UI setup.");
            return;
        }

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

    private void OnStudentSlipped(StudentSlippedEventPayload payload)
    {

        if (studentIconImages == null || studentIconImages.Length == 0)
        {
            Debug.LogWarning("StudentTracker: studentIconImages is null or empty, cannot update UI");
            return;
        }

        slippedStudents++;
        // Debug.Log($"StudentTracker: slippedCount={slippedStudents}, iconArrayLength={studentIconImages.Length}");

        // Update the icon for the newly slipped student
        if (slippedStudents > 0 && slippedStudents <= studentIconImages.Length)
        {
            int iconIndex = slippedStudents - 1; // Convert to 0-based index
            Debug.Log($"StudentTracker: Updating icon at index {iconIndex}");
            Color color = studentIconImages[iconIndex].color;
            color.a = fullOpacity;
            studentIconImages[iconIndex].color = color;
            Debug.Log($"Student {iconIndex} has slipped. Setting icon to full opacity. Total slipped: {slippedStudents}");
        }
        else
        {
            Debug.LogWarning($"StudentTracker: Condition failed! slippedCount={slippedStudents}, needs to be > 0 and <= {studentIconImages.Length}");
        }
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
}
